//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;
//using MediatR;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using NPOI.SS.Formula.Functions;
//using SFA.DAS.ApplyService.Application.Apply;
//using SFA.DAS.ApplyService.Domain.Apply;
//
//namespace SFA.DAS.ApplyService.Application.Import
//{
//    public class ExtractAssetsAndWorkflowHandler : IRequestHandler<ExtractAssetsAndWorkflowRequest>
//    {
//        private readonly IApplyRepository _applyRepository;
//        private readonly ILogger<ExtractAssetsAndWorkflowHandler> _logger;
//
//        public ExtractAssetsAndWorkflowHandler(IApplyRepository applyRepository, ILogger<ExtractAssetsAndWorkflowHandler> logger)
//        {
//            _applyRepository = applyRepository;
//            _logger = logger;
//        }
//        
//        public async Task<Unit> Handle(ExtractAssetsAndWorkflowRequest request, CancellationToken cancellationToken)
//        {
//            //await _applyRepository.ClearAssets();
//            
//            //var applicationSections = await _applyRepository.GetApplicationSections();
//
//
//            var workflowSections = await _applyRepository.GetWorkflowSections();
//
//            foreach (var workflowSection in workflowSections)
//            {
//                var a = workflowSection;
//            }
//
////            var reference = 0;
////
////            var assets = new Dictionary<string, string>();
////            
////            foreach (var applicationSection in applicationSections)
////            {
////                var qnaDataObject = applicationSection.QnAData;
////                
////                foreach (var page in qnaDataObject.Pages)
////                {
////                    reference++;
////                    assets.Add($"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{reference}", page.Title);
////                    page.Title = $"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{reference}";
////                    
////                    reference++;
////                    assets.Add($"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{reference}", page.BodyText);
////                    page.BodyText = $"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{reference}";
////                    
////                    reference++;
////                    assets.Add($"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{reference}", page.InfoText);
////                    page.InfoText = $"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{reference}";
////                    
////                    reference++;
////                    assets.Add($"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{reference}", page.LinkTitle);
////                    page.LinkTitle = $"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{reference}";
////
////                    page.PageOfAnswers = new List<PageOfAnswers>();
////                    
////                    foreach (var question in page.Questions)
////                    {
////                        reference++;
////                        assets.Add($"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{question.QuestionId}-{reference}", question.Hint);
////                        question.Hint = $"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{question.QuestionId}-{reference}";
////                        
////                        reference++;
////                        assets.Add($"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{question.QuestionId}-{reference}", question.Label);
////                        question.Label = $"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{question.QuestionId}-{reference}";
////                        
////                        reference++;
////                        assets.Add($"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{question.QuestionId}-{reference}", question.ShortLabel);
////                        question.ShortLabel = $"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{question.QuestionId}-{reference}";
////                        
////                        reference++;
////                        assets.Add($"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{question.QuestionId}-{reference}", question.QuestionBodyText);
////                        question.QuestionBodyText = $"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{question.QuestionId}-{reference}";
////
////                        _logger.LogInformation($"Page: {page.PageId} QuestionId : {question.QuestionId}");
////                        
////                        if (question.Input.Type == "ComplexRadio" && question.Input.Options != null)
////                        {
////                            foreach (var option in question.Input.Options)
////                            {
////                                if (option.FurtherQuestions != null)
////                                {
////                                    foreach (var furtherQuestion in option.FurtherQuestions)
////                                    {
////                                        string hint = furtherQuestion.Hint;
////                                        reference++;
////                                        assets.Add($"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{furtherQuestion.QuestionId}-{reference}", hint ?? "");
////                                        furtherQuestion.Hint = $"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{furtherQuestion.QuestionId}-{reference}";
////
////                                        string label = furtherQuestion.Label;
////                                        reference++;
////                                        assets.Add($"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{furtherQuestion.QuestionId}-{reference}", label ?? "");
////                                        furtherQuestion.Label = $"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{furtherQuestion.QuestionId}-{reference}";
////
////                                        string shortLabel = furtherQuestion.ShortLabel;
////                                        reference++;
////                                        assets.Add($"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{furtherQuestion.QuestionId}-{reference}", shortLabel ?? "");
////                                        furtherQuestion.ShortLabel = $"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{furtherQuestion.QuestionId}-{reference}";
////
////                                        string questionBodyText = furtherQuestion.QuestionBodyText;
////                                        reference++;
////                                        assets.Add($"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{furtherQuestion.QuestionId}-{reference}", questionBodyText ?? "");
////                                        furtherQuestion.QuestionBodyText = $"SQ-{applicationSection.SequenceId}-SE-{applicationSection.SectionId}-PG-{page.PageId}-{furtherQuestion.QuestionId}-{reference}";
////                                    }
////                                }   
////                            }  
////                        }
////                    }
////                }
////
////                applicationSection.QnAData = qnaDataObject;
////            }
//
////            await _applyRepository.UpdateSections(applicationSections);
////
////            await _applyRepository.AddAssets(assets);
//            
//            return Unit.Status;
//        }
//    }
//}