using System;
using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Services;

namespace SFA.DAS.ApplyService.Web.UnitTests.Services
{
    [TestFixture]
    public class RoatpTaskListWorkflowServiceTests
    {
        private RoatpTaskListWorkflowService _service;


        [SetUp]
        public void Before_each_test()
        {
            _service = new RoatpTaskListWorkflowService();
        }


        [Test]
        public void Get_SectionStatus_Empty_When_Null_Sequences_Used()
        {
            var expectedResult = string.Empty;
            var actualResult = _service.SectionStatus(null, null, 0, 0, null);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Get_SectionStatus_Empty_When_Empty_Sequences_Used()
        {
            var expectedResult = string.Empty;
            var actualResult = _service.SectionStatus(new List<ApplicationSequence>(), null, 0, 0, null);
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public void Get_SectionStatus_Empty_When_Empty_Sections_Used()
        {
            var expectedResult = string.Empty;
            var actualResult = _service.SectionStatus(new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = new Guid(), SequenceId = 1 } }, null, 1, 0, null);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Get_SectionStatus_Empty_When_Unmatched_Sections_Used()
        {
            var expectedResult = string.Empty;
            var actualResult = _service.SectionStatus(new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = new Guid(), SequenceId = 1, Sections = new List<ApplicationSection> { new ApplicationSection { SectionId = 2 } } } }, null, 1, 3, null);
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public void Get_SectionStatus_Empty_When_Matched_Sections_Used_But_No_Other_Setup()
        {
            var sectionId = 2;
            var expectedResult = string.Empty;
            var actualResult = _service.SectionStatus(new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = new Guid(), SequenceId = 1,
                Sections = new List<ApplicationSection> { new ApplicationSection { SectionId = sectionId,
                                                                                    QnAData = new QnAData {Pages = new List<Page>()}} } } }, 
                null, 1, sectionId, null);
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
                    ConditionalCheckField = "ProviderTypeId",
                    MustEqual = applicationRouteId,
                    SequenceId = sequenceId,
                    SectionId = sectionId
                }
            };


            var actualResult = _service.SectionStatus(new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = new Guid(), SequenceId = sequenceId,
                    Sections = new List<ApplicationSection> { new ApplicationSection { SectionId = sectionId,
                        QnAData = new QnAData {Pages = new List<Page>()}} } } },
                notRequiredOverrides, sequenceId, sectionId, applicationRouteId);
            Assert.AreEqual(expectedResult, actualResult.ToLower());
        }
    }
}
