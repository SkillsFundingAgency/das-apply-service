using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;

namespace SFA.DAS.ApplyService.Web.UnitTests.Services
{
    [TestFixture]
    public class RoatpTaskListWorkflowServiceTests
    {
        private Mock<IQnaApiClient> _qnaApiClient;
        private Mock<INotRequiredOverridesService> _notRequiredOverridesService;
        private RoatpTaskListWorkflowService _service;
        private Guid _applicationId;

        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();
            _qnaApiClient = new Mock<IQnaApiClient>();
            _notRequiredOverridesService = new Mock<INotRequiredOverridesService>();
            _notRequiredOverridesService.Setup(x => x.GetNotRequiredOverrides(_applicationId))
                                        .Returns(new List<NotRequiredOverrideConfiguration>());
            
            var config = new Mock<IOptions<List<TaskListConfiguration>>>();
            var logger = new Mock<ILogger<RoatpTaskListWorkflowService>>();

            _service = new RoatpTaskListWorkflowService(_qnaApiClient.Object, _notRequiredOverridesService.Object, config.Object, logger.Object);
        }

        [Test]
        public void Get_SectionStatus_Empty_When_Null_Sequences_Used()
        {
            var expectedResult = string.Empty;
            var actualResult = _service.SectionStatus(_applicationId, 0, 0, null, null);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Get_SectionStatus_Empty_When_Empty_Sequences_Used()
        {
            var expectedResult = string.Empty;
            var actualResult = _service.SectionStatus(_applicationId, 0, 0, new List<ApplicationSequence>(), null);
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public void Get_SectionStatus_Empty_When_Empty_Sections_Used()
        {
            var expectedResult = string.Empty;
            var actualResult = _service.SectionStatus(_applicationId, 1, 0, new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = new Guid(), SequenceId = 1 } }, null);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Get_SectionStatus_Empty_When_Unmatched_Sections_Used()
        {
            var expectedResult = string.Empty;
            var actualResult = _service.SectionStatus(_applicationId, 1, 3, new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = new Guid(), SequenceId = 1, Sections = new List<ApplicationSection> { new ApplicationSection { SectionId = 2 } } } }, null);
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public void Get_SectionStatus_Empty_When_Matched_Sections_Used_But_No_Other_Setup()
        {
            var sectionId = 2;
            var expectedResult = string.Empty;
            var actualResult = _service.SectionStatus(_applicationId, 1, sectionId, new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = new Guid(), SequenceId = 1,
                Sections = new List<ApplicationSection> { new ApplicationSection { SectionId = sectionId,
                                                                                    QnAData = new QnAData {Pages = new List<Page>()}} } } }, 
                null);
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public void Get_SectionStatus_Not_Required_When_Matched_Sections_Used_And_Overrides_Set()
        {
            var sequenceId = 1;
            var sectionId = 2;
            var applicationRouteId = "3";
            var expectedResult = "not required";
            var notRequiredOverrides = new List<NotRequiredOverrideConfiguration>
            {
                new NotRequiredOverrideConfiguration
                {
                    Conditions = new List<NotRequiredCondition>
                    {
                        new NotRequiredCondition
                        {
                            ConditionalCheckField = "ProviderTypeId",
                            MustEqual = applicationRouteId,
                            Value = applicationRouteId
                        }
                    },
                    SequenceId = sequenceId,
                    SectionId = sectionId
                }
            };
            _notRequiredOverridesService.Setup(x => x.GetNotRequiredOverrides(_applicationId)).Returns(notRequiredOverrides);


            var actualResult = _service.SectionStatus(_applicationId, sequenceId, sectionId, new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = new Guid(), SequenceId = sequenceId,
                    Sections = new List<ApplicationSection> { new ApplicationSection { SectionId = sectionId,
                        QnAData = new QnAData {Pages = new List<Page>()}} } } },
                null);
            Assert.AreEqual(expectedResult, actualResult.ToLower());
        }

        [Test]
        public void Get_SectionStatus_Not_Required_When_Multiple_Conditions_Matched()
        {
            var sequenceId = 1;
            var sectionId = 2;
            var applicationRouteId = "3";
            var orgType = "HEI";
            var expectedResult = "not required";
            var notRequiredOverrides = new List<NotRequiredOverrideConfiguration>
            {
                new NotRequiredOverrideConfiguration
                {
                    Conditions = new List<NotRequiredCondition>
                    {
                        new NotRequiredCondition
                        {
                            ConditionalCheckField = "ProviderTypeId",
                            MustEqual = applicationRouteId,
                            Value = applicationRouteId
                        },
                        new NotRequiredCondition
                        {
                            ConditionalCheckField = "OrganisationType",
                            MustEqual = orgType,
                            Value = orgType
                        }
                    },
                    SequenceId = sequenceId,
                    SectionId = sectionId
                }
            };
            _notRequiredOverridesService.Setup(x => x.GetNotRequiredOverrides(_applicationId)).Returns(notRequiredOverrides);

            var actualResult = _service.SectionStatus(_applicationId, sequenceId, sectionId, new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = new Guid(), SequenceId = sequenceId,
                    Sections = new List<ApplicationSection> { new ApplicationSection { SectionId = sectionId,
                        QnAData = new QnAData {Pages = new List<Page>()}} } } },
                null);
            Assert.AreEqual(expectedResult, actualResult.ToLower());
        }

        [Test]
        public void Get_SectionStatus_Only_One_Of_Multiple_Not_Required_Conditions_Matched()
        {
            var sequenceId = 1;
            var sectionId = 2;
            var applicationRouteId = "3";
            var orgType = "HEI";

            var notRequiredOverrides = new List<NotRequiredOverrideConfiguration>
            {
                new NotRequiredOverrideConfiguration
                {
                    Conditions = new List<NotRequiredCondition>
                    {
                        new NotRequiredCondition
                        {
                            ConditionalCheckField = "ProviderTypeId",
                            MustEqual = applicationRouteId,
                            Value = applicationRouteId
                        },
                        new NotRequiredCondition
                        {
                            ConditionalCheckField = "OrganisationType",
                            MustEqual = orgType,
                            Value = "unmatched"
                        }
                    },
                    SequenceId = sequenceId,
                    SectionId = sectionId
                }
            };
            _notRequiredOverridesService.Setup(x => x.GetNotRequiredOverrides(_applicationId)).Returns(notRequiredOverrides);

            var actualResult = _service.SectionStatus(_applicationId, sequenceId, sectionId, new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = new Guid(), SequenceId = sequenceId,
                    Sections = new List<ApplicationSection> { new ApplicationSection { SectionId = sectionId,
                        QnAData = new QnAData {Pages = new List<Page>()}} } } },
                null);
            Assert.IsEmpty(actualResult);
        }
    }
}
