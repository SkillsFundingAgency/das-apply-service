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
    public class StartApplicationHandler : IRequestHandler<StartApplicationRequest, StartApplicationResponse>
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

        public async Task<StartApplicationResponse> Handle(StartApplicationRequest request, CancellationToken cancellationToken)
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
                var pagesToMakeNotRequired = applicationSection.QnAData.Pages.Where(p => p.NotRequiredOrgTypes != null && p.NotRequiredOrgTypes.Contains(org.OrganisationType));

                foreach (var page in pagesToMakeNotRequired)
                {
                    page.NotRequired = true;
                    page.Complete = true;
                }

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

            return new StartApplicationResponse() {ApplicationId = applicationId};
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
                            var answer = await GetDataFedAnswer(applicationId, question);
                            if (answer != null)
                            {
                                questionsDataFed++;
                                if (question.Input.Type == "ComplexRadio")
                                {
                                    await DataFeedComplexRadioQuestions(question, page, answer);
                                }
                                else
                                {
                                    page.PageOfAnswers = new List<PageOfAnswers>()
                                    {
                                        new PageOfAnswers()
                                        {
                                            Answers = new List<Answer>()
                                            {
                                                new Answer()
                                                {
                                                    QuestionId = question.QuestionId, Value = answer.Answer,
                                                    DataFed = true
                                                }
                                            }
                                        }
                                    };

                                }
                            }
                        }
                        
                        page.Complete = questionsDataFed == page.Questions.Count;
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
        
        private async Task DataFeedComplexRadioQuestions(Question question, Page page, DataFedAnswerResult answer)
        {
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
        
        private void DisableSequencesAndSectionsAsAppropriate(Organisation org, List<ApplicationSequence> sequences, List<ApplicationSection> sections)
        {
            bool isEpao = IsOrganisationOnEPAORegister(org);
            if (isEpao)
            {
                RemoveSectionsOneAndTwo(sections);
            }

            bool isFinancialExempt = IsFinancialExempt(org.OrganisationDetails?.FHADetails);
            if (isFinancialExempt)
            {
                RemoveSectionThree(sections);
            }

            if (isEpao && isFinancialExempt)
            {
                RemoveSequenceOne(sequences);
            }
        }

        private static bool IsOrganisationOnEPAORegister(Organisation org)
        {
            if (org is null) return false;

            return org.RoEPAOApproved;
        }

        private static bool IsFinancialExempt(FHADetails financials)
        {
            if (financials is null) return false;

            bool financialExempt = financials.FinancialExempt ?? false;
            bool financialIsNotDue = (financials.FinancialDueDate?.Date ?? DateTime.MinValue) > DateTime.Today;

            return financialExempt || financialIsNotDue;
        }

        private void RemoveSequenceOne(List<ApplicationSequence> sequences)
        {
            var stage1 = sequences.Single(seq => seq.SequenceId == SequenceId.Stage1);
            stage1.IsActive = false;
            stage1.NotRequired = true;
            stage1.Status = ApplicationSequenceStatus.Approved;

            SetSubmissionData(stage1.ApplicationId, stage1.SequenceId).GetAwaiter().GetResult();

            sequences.Single(seq => seq.SequenceId == SequenceId.Stage2).IsActive = true;
        }

        private async Task SetSubmissionData(Guid applicationId, SequenceId sequenceId)
        {
            var application = await _applyRepository.GetApplication(applicationId);

            if (application != null)
            {
                if(application.ApplicationData == null)
                {
                    application.ApplicationData = new ApplicationData();
                }

                if (sequenceId == SequenceId.Stage1)
                {
                    application.ApplicationData.LatestInitSubmissionDate = DateTime.UtcNow;
                    application.ApplicationData.InitSubmissionClosedDate = DateTime.UtcNow;
                }
                else if (sequenceId == SequenceId.Stage2)
                {
                    application.ApplicationData.LatestStandardSubmissionDate = DateTime.UtcNow;
                    application.ApplicationData.StandardSubmissionClosedDate = DateTime.UtcNow;
                }

                await _applyRepository.UpdateApplicationData(application.Id, application.ApplicationData);
            }
        }

        private void RemoveSectionThree(List<ApplicationSection> sections)
        {
            foreach(var sec in sections.Where(s => s.SectionId == 3))
            {
                sec.NotRequired = true;
                sec.Status = ApplicationSectionStatus.Evaluated;

                if (sec.QnAData.FinancialApplicationGrade is null)
                {
                    sec.QnAData.FinancialApplicationGrade = new FinancialApplicationGrade();
                }

                sec.QnAData.FinancialApplicationGrade.SelectedGrade = FinancialApplicationSelectedGrade.Exempt;
                sec.QnAData.FinancialApplicationGrade.GradedDateTime = DateTime.UtcNow;
            }
        }

        private void RemoveSectionsOneAndTwo(List<ApplicationSection> sections)
        {
            foreach (var sec in sections.Where(s => s.SectionId == 1 || s.SectionId == 2))
            {
                sec.NotRequired = true;
                sec.Status = ApplicationSectionStatus.Evaluated;
            }
        }
    }
}