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
        [Test]
        public void Get_SectionStatus_Empty_When_Null_Sequences_Used()
        {
            var expectedResult = string.Empty;
            var actualResult = RoatpTaskListWorkflowService.SectionStatus(null, null, 0, 0);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Get_SectionStatus_Empty_When_Empty_Sequences_Used()
        {
            var expectedResult = string.Empty;
            var actualResult = RoatpTaskListWorkflowService.SectionStatus(new List<ApplicationSequence>(), null, 0, 0);
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public void Get_SectionStatus_Empty_When_Empty_Sections_Used()
        {
            var expectedResult = string.Empty;
            var actualResult = RoatpTaskListWorkflowService.SectionStatus(new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = new Guid(), SequenceId = 1 } }, null, 1, 0);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Get_SectionStatus_Empty_When_Unmatched_Sections_Used()
        {
            var expectedResult = string.Empty;
            var actualResult = RoatpTaskListWorkflowService.SectionStatus(new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = new Guid(), SequenceId = 1, Sections = new List<ApplicationSection> { new ApplicationSection { SectionId = 2 } } } }, null, 1, 3);
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public void Get_SectionStatus_Empty_When_Matched_Sections_Used_But_No_Other_Setup()
        {
            var sectionId = 2;
            var expectedResult = string.Empty;
            var actualResult = RoatpTaskListWorkflowService.SectionStatus(new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = new Guid(), SequenceId = 1,
                Sections = new List<ApplicationSection> { new ApplicationSection { SectionId = sectionId,
                                                                                    QnAData = new QnAData {Pages = new List<Page>()}} } } }, 
                null, 1, sectionId);
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


            var actualResult = RoatpTaskListWorkflowService.SectionStatus(new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = new Guid(), SequenceId = sequenceId,
                    Sections = new List<ApplicationSection> { new ApplicationSection { SectionId = sectionId,
                        QnAData = new QnAData {Pages = new List<Page>()}} } } },
                notRequiredOverrides, sequenceId, sectionId);
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

            var actualResult = RoatpTaskListWorkflowService.SectionStatus(new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = new Guid(), SequenceId = sequenceId,
                    Sections = new List<ApplicationSection> { new ApplicationSection { SectionId = sectionId,
                        QnAData = new QnAData {Pages = new List<Page>()}} } } },
                notRequiredOverrides, sequenceId, sectionId);
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

            var actualResult = RoatpTaskListWorkflowService.SectionStatus(new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = new Guid(), SequenceId = sequenceId,
                    Sections = new List<ApplicationSection> { new ApplicationSection { SectionId = sectionId,
                        QnAData = new QnAData {Pages = new List<Page>()}} } } },
                notRequiredOverrides, sequenceId, sectionId);
            Assert.IsEmpty(actualResult);
        }
    }
}
