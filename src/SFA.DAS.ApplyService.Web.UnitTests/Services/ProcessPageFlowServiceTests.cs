using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Infrastructure.Services;

namespace SFA.DAS.ApplyService.Web.UnitTests.Services
{
    [TestFixture]
    public class ProcessPageFlowServiceTests
    {
        private ProcessPageFlowService _service;
        private Mock<IQnaApiClient> _qnaApiClient;
        private Mock<IOptions<List<TaskListConfiguration>>> _configuration;

        private Guid _applicationId;
    

        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();
            _qnaApiClient = new Mock<IQnaApiClient>();
            _configuration = new Mock<IOptions<List<TaskListConfiguration>>>();
        }


        [TestCase( "Page-Seq1-ProviderType1", 1, 1)]
        [TestCase("Page-Seq2-ProviderType1", 2, 1)]
        [TestCase("Page-Seq3-ProviderType1", 3, 1)]
        [TestCase("Page-Seq1-ProviderType2", 1, 2)]
        [TestCase("Page-Seq2-ProviderType2", 2, 2)]
        [TestCase("Page-Seq3-ProviderType2", 3, 2)]

        public void Get_IntroductionPageId_For_Given_Sequence(string pageId, int sequenceId, int providerTypeId)
        {
            var startupPage = new StartupPage {PageId = pageId, ProviderTypeId = providerTypeId};
            var taskListConfiguration = new TaskListConfiguration {Id =sequenceId, Title="test configuration", StartupPages = new List<StartupPage>
                 {
                     startupPage,
                 new StartupPage { PageId = pageId+"A", ProviderTypeId = providerTypeId + 1 },
                 new StartupPage { PageId = pageId+"Z", ProviderTypeId = providerTypeId - 1 },
                 }
             };
             _configuration.Setup(x => x.Value).Returns(new List<TaskListConfiguration> {taskListConfiguration});
             _service = new ProcessPageFlowService(_qnaApiClient.Object, _configuration.Object);
             var returnedPageId = _service.GetIntroductionPageIdForSequence(sequenceId, providerTypeId).Result;

             Assert.AreEqual(pageId,returnedPageId);
        }

        [TestCase("1", 1)]
        [TestCase("2", 2)]
        [TestCase("3", 3)]
        [TestCase("4", 4)]
        [TestCase(null, 1)]
        [TestCase("", 1)]
        [TestCase(" ", 1)]
        [TestCase("other", 1)]
        public void Get_Application_ProviderTypeId_For_Given_Sequence(string providerTypeId, int expectedProviderTypeId)
        {
            var answer = new Answer {QuestionId = "test-x" ,Value = providerTypeId};
            _qnaApiClient.Setup(x => x.GetAnswerByTag(_applicationId, RoatpWorkflowQuestionTags.ProviderRoute, It.IsAny<string>()))
                .ReturnsAsync(answer);
            _service = new ProcessPageFlowService(_qnaApiClient.Object, _configuration.Object);
            var returnedProviderTypeId = _service.GetApplicationProviderTypeId(_applicationId).Result;

            Assert.AreEqual(expectedProviderTypeId, returnedProviderTypeId);
        }
    }
}
