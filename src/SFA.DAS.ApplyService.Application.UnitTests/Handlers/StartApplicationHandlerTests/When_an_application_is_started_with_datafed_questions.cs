using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.DataFeeds;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.StartApplicationHandlerTests
{
    public class When_an_application_is_started_with_datafed_questions : StartApplicationHandlerTestsBase
    {
        private void Init()
        {
            OrganisationRepository.Setup(r => r.GetUserOrganisation(UserId)).ReturnsAsync(new Organisation
            {
                Id = ApplyingOrganisationId,
                OrganisationType = "",
                RoEPAOApproved = false
            });

            ApplyRepository.Setup(r => r.CopyWorkflowToApplication(ApplicationId, LatestWorkflowId, It.IsAny<string>())).ReturnsAsync(new List<ApplicationSection>
            {
                new ApplicationSection
                {
                    SectionId = 1, QnAData = new QnAData
                    {
                        Pages = new List<Page>
                        {
                            new Page()
                            {
                                Questions = new List<Question>
                                {
                                    new Question()
                                    {
                                        QuestionId = "Q1",
                                        DataFedAnswer = new DataFedAnswer()
                                        {
                                            Type = "CompanyNumber"
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                new ApplicationSection {SectionId = 2, QnAData = new QnAData {Pages = new List<Page>()}},
                new ApplicationSection {SectionId = 3, QnAData = new QnAData {Pages = new List<Page>()}},
                new ApplicationSection {SectionId = 4, QnAData = new QnAData {Pages = new List<Page>()}}
            });
        }

        
        [Test]
        public void Then_DataFeedFactory_is_asked_for_correct_data_feed_classes()
        {
            Init();
            Handler.Handle(new StartApplicationRequest(UserId), new CancellationToken()).Wait();

            DataFeedFactory.Verify(dff => dff.GetDataField("CompanyNumber"));
        }

        [Test]
        public void Then_section_is_saved_with_data_fed_answer()
        {
            Init();

            var datafeed = new Mock<IDataFeed>();
            datafeed.Setup(df => df.GetAnswer(It.IsAny<Guid>())).ReturnsAsync(new DataFedAnswerResult() {Answer = "Feed from data"});

            DataFeedFactory.Setup(dff => dff.GetDataField("CompanyNumber")).Returns(datafeed.Object);
            
            Handler.Handle(new StartApplicationRequest(UserId), new CancellationToken()).Wait();
            
            ApplyRepository.Verify(r => r.UpdateSections(It.Is<List<ApplicationSection>>(response => response.Any(
                section => 
                    section.SectionId == 1
                    && section.QnAData.Pages[0].PageOfAnswers[0].Answers[0].QuestionId == "Q1"
                    && section.QnAData.Pages[0].PageOfAnswers[0].Answers[0].Value == "Feed from data"))));
        }
        
    }
}