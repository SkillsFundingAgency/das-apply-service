using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.DataFeeds;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Application.Organisations;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.StartApplication
{
    public class StartApplicationHandler : IRequestHandler<StartApplicationRequest>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IDataFeedFactory _dataFeedFactory;

        public StartApplicationHandler(IApplyRepository applyRepository, IOrganisationRepository organisationRepository, IDataFeedFactory dataFeedFactory)
        {
            _applyRepository = applyRepository;
            _organisationRepository = organisationRepository;
            _dataFeedFactory = dataFeedFactory;
        }

        public async Task<Unit> Handle(StartApplicationRequest request, CancellationToken cancellationToken)
        {
            var assets = await _applyRepository.GetAssets();

            var org = await _organisationRepository.GetUserOrganisation(request.UserId);

            var workflowId = await _applyRepository.GetLatestWorkflow("EPAO");
            var applicationId =
                await _applyRepository.CreateApplication("EPAO", org.Id, request.UserId, workflowId);

            var sections =
                await _applyRepository.CopyWorkflowToApplication(applicationId, workflowId, org.OrganisationType);

            foreach (var applicationSection in sections)
            {
                string QnADataJson = JsonConvert.SerializeObject(applicationSection.QnAData);
                foreach (var asset in assets)
                {
                    QnADataJson = QnADataJson.Replace(asset.Reference, HttpUtility.JavaScriptStringEncode(asset.Text));
                }

                applicationSection.QnAData = JsonConvert.DeserializeObject<QnAData>(QnADataJson);
            }

            var sequences = await _applyRepository.GetSequences(applicationId);

            DisableSequencesAndSectionsAsAppropriate(org, sequences, sections);

            await DataFeedAnswers(sections, applicationId);

            await _applyRepository.UpdateSections(sections);
            await _applyRepository.UpdateSequences(sequences);

            return Unit.Value;
        }

        private async Task DataFeedAnswers(List<ApplicationSection> sections, Guid applicationId)
        {
            foreach (var section in sections)
            {
                foreach (var page in section.QnAData.Pages)
                {
                    if (page.Questions == null) continue;
                    var questionsDataFed = 0;
                    foreach (var question in page.Questions)
                    {
                        if (question.DataFedAnswer != null)
                        {
                            questionsDataFed++;
                            if (question.Input.Type == "ComplexRadio")
                            {
                                await DataFeedComplexRadioQuestions(applicationId, question, page);
                            }
                            else
                            {
                                var answer = await GetDataFedAnswer(applicationId, question);
                                if (answer != null)
                                {
                                    page.PageOfAnswers = new List<PageOfAnswers>() {new PageOfAnswers()
                                    {
                                        Answers = new List<Answer>()
                                        {
                                            new Answer() {QuestionId = question.QuestionId, Value = answer.Answer, DataFed = true}
                                        }
                                    }};
                                }
                            }
                        }
                        
                        page.Complete = questionsDataFed == page.Questions.Count;
                    }
                }
            }
        }

        private async Task DataFeedComplexRadioQuestions(Guid applicationId, Question question, Page page)
        {
            var answer = await GetDataFedAnswer(applicationId, question);
            if (answer != null)
            {
                foreach (var inputOption in question.Input.Options)
                {
                    if (inputOption.FurtherQuestions != null)
                    {
                        var dataFeedAppliesTo = inputOption.FurtherQuestions.SingleOrDefault(q => q.QuestionId == question.DataFedAnswer.AppliesTo);
                        if (dataFeedAppliesTo != null)
                        {
                            page.PageOfAnswers = new List<PageOfAnswers>
                            {
                                new PageOfAnswers
                                {
                                    Answers = new List<Answer>
                                    {
                                        new Answer
                                        {
                                            QuestionId = question.QuestionId, Value = inputOption.Value, DataFed = true
                                        },
                                        new Answer
                                        {
                                            QuestionId = dataFeedAppliesTo.QuestionId, Value = answer.Answer, DataFed = true
                                        }
                                    }
                                }
                            };
                        }
                    }
                }
            }
        }

        private async Task<DataFedAnswerResult> GetDataFedAnswer(Guid applicationId, Question question)
        {
            var datafeed = _dataFeedFactory.GetDataField(question.DataFedAnswer.Type);
            var answer = await datafeed.GetAnswer(applicationId);
            return answer;
        }

        private void DisableSequencesAndSectionsAsAppropriate(Organisation org, List<ApplicationSequence> sequences, List<ApplicationSection> sections)
        {
            if (OrganisationIsNotOnEPAORegister(org)) return;

            RemoveSectionsOneAndTwo(sections);

            if (FinancialAssessmentRequired(org.OrganisationDetails.FHADetails)) return;

            RemoveSectionThree(sections);
            RemoveSequenceOne(sequences);
        }

        private static bool OrganisationIsNotOnEPAORegister(Organisation org)
        {
            return !org.RoEPAOApproved;
        }

        private void RemoveSequenceOne(List<ApplicationSequence> sequences)
        {
            var stage1 = sequences.Single(seq => seq.SequenceId == SequenceId.Stage1);
            stage1.IsActive = false;
            stage1.NotRequired = true;

            sequences.Single(seq => seq.SequenceId == SequenceId.Stage2).IsActive = true;
        }

        private void RemoveSectionThree(List<ApplicationSection> sections)
        {
            sections.Where(s => s.SectionId == 3).ToList().ForEach(s => s.NotRequired = true);
        }

        private void RemoveSectionsOneAndTwo(List<ApplicationSection> sections)
        {
            sections.Where(s => s.SectionId == 1 || s.SectionId == 2).ToList().ForEach(s => s.NotRequired = true);
        }

        private static bool FinancialAssessmentRequired(FHADetails financials)
        {
            return (financials == null ||
                       financials.FinancialDueDate.HasValue && financials.FinancialDueDate.Value <= DateTime.Today)
                   || (financials.FinancialExempt.HasValue && !financials.FinancialExempt.Value);
        }
    }
}