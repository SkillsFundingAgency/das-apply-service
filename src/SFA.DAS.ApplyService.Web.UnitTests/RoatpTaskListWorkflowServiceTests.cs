using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using System;
using System.Collections.Generic;
using NotRequiredOverride = SFA.DAS.ApplyService.Domain.Entities.NotRequiredOverride;
using NotRequiredCondition = SFA.DAS.ApplyService.Domain.Entities.NotRequiredCondition;

namespace SFA.DAS.ApplyService.Web.UnitTests
{
    [TestFixture]
    public class RoatpTaskListWorkflowServiceTests
    {
        private RoatpTaskListWorkflowService _service;
        private Mock<IQnaApiClient> _qnaApiClient;
        private Mock<INotRequiredOverridesService> _notRequiredOverridesService;
        private Mock<IOptions<List<TaskListConfiguration>>> _configuration;
        private Guid _applicationId;

        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();
            _qnaApiClient = new Mock<IQnaApiClient>();
            _notRequiredOverridesService = new Mock<INotRequiredOverridesService>(); 
            _notRequiredOverridesService.Setup(x => x.GetNotRequiredOverrides(It.IsAny<Guid>())).ReturnsAsync(new List<NotRequiredOverride>());
            _configuration = new Mock<IOptions<List<TaskListConfiguration>>>();

            var config = new List<TaskListConfiguration>
            {
                new TaskListConfiguration
                {
                    Id = 1,
                    Sequential = true,
                    Title = "Your organisation"
                },
                new TaskListConfiguration
                {
                    Id = 2,
                    Sequential = false,
                    Title = "Financial evidence"
                }
                ,new TaskListConfiguration
                {
                    Id = 3,
                    Sequential = false,
                    Title = "Criminal and compliance checks"
                },
                new TaskListConfiguration
                {
                    Id = 4,
                    Sequential = false,
                    Title = "Protecting your apprentices"
                },
                new TaskListConfiguration
                {
                    Id = 5,
                    Sequential = false,
                    Title = "Readiness to engage"
                },
                new TaskListConfiguration
                {
                    Id = 6,
                    Sequential = false,
                    Title = "Planning apprenticeship training"
                },
                new TaskListConfiguration
                {
                    Id = 7,
                    Sequential = false,
                    Title = "Delivering apprenticeship training"
                },
                new TaskListConfiguration
                {
                    Id = 8,
                    Sequential = false,
                    Title = "Evaluating apprenticeship training"
                },
                new TaskListConfiguration
                {
                    Id = 9,
                    Sequential = true,
                    Title = "Finish"
                }
            };
            _configuration.Setup(x => x.Value).Returns(config);

            _service = new RoatpTaskListWorkflowService(_qnaApiClient.Object, _notRequiredOverridesService.Object, _configuration.Object);
        }

        [Test]
        public void Get_SectionStatus_Empty_When_Null_Sequences_Used()
        {
            var expectedResult = string.Empty;
            var sequenceId = 0;
            var sectionId = 0;
            List<ApplicationSequence> applicationSequences = null;
            var organisationVerificationStatus = new OrganisationVerificationStatus();
            var actualResult = _service.SectionStatus(_applicationId, sequenceId, sectionId, applicationSequences, organisationVerificationStatus);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Get_SectionStatus_Empty_When_Empty_Sequences_Used()
        {
            var expectedResult = string.Empty;
            var sequenceId = 0;
            var sectionId = 0;
            var applicationSequences = new List<ApplicationSequence>();
            var organisationVerificationStatus = new OrganisationVerificationStatus();
            var actualResult = _service.SectionStatus(_applicationId, sequenceId, sectionId, applicationSequences, organisationVerificationStatus);
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public void Get_SectionStatus_Empty_When_Empty_Sections_Used()
        {
            var expectedResult = string.Empty;
            var sequenceId = 1;
            var sectionId = 0;
            var applicationSequences = new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = Guid.NewGuid(), SequenceId = 1 } };
            var organisationVerificationStatus = new OrganisationVerificationStatus();
            var actualResult = _service.SectionStatus(_applicationId, sequenceId, sectionId, applicationSequences, organisationVerificationStatus);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Get_SectionStatus_Empty_When_Unmatched_Sections_Used()
        {
            var expectedResult = string.Empty;
            var sequenceId = 1;
            var sectionId = 3;
            var applicationSequences = new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = Guid.NewGuid(), SequenceId = 1, Sections = new List<ApplicationSection> { new ApplicationSection { SectionId = 2 } } } };
            var organisationVerificationStatus = new OrganisationVerificationStatus(); 
            var actualResult = _service.SectionStatus(_applicationId, sequenceId, sectionId, applicationSequences, organisationVerificationStatus);
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public void Get_SectionStatus_Empty_When_Matched_Sections_Used_But_No_Other_Setup()
        {
            var sequenceId = 1;
            var sectionId = 2;
            var expectedResult = string.Empty;
            var applicationSequences = new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = Guid.NewGuid(), SequenceId = 1,
                Sections = new List<ApplicationSection> { new ApplicationSection { SectionId = sectionId,
                                                                                    QnAData = new QnAData {Pages = new List<Page>()}} } } };
            var organisationVerificationStatus = new OrganisationVerificationStatus();
            var actualResult = _service.SectionStatus(_applicationId, sequenceId, sectionId, applicationSequences, organisationVerificationStatus);
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        public void Get_SectionStatus_Not_Required_When_Matched_Sections_Used_And_Overrides_Set()
        {
            var sequenceId = 1;
            var sectionId = 2;
            var applicationRouteId = "3";
            var expectedResult = "not required";
            var notRequiredOverrides = new List<NotRequiredOverride>
            {
                new NotRequiredOverride
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
            _notRequiredOverridesService.Setup(x => x.GetNotRequiredOverrides(_applicationId)).ReturnsAsync(notRequiredOverrides);

            var applicationSequences = new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = Guid.NewGuid(), SequenceId = sequenceId,
                    Sections = new List<ApplicationSection> { new ApplicationSection { SectionId = sectionId,
                        QnAData = new QnAData {Pages = new List<Page>()}} } } };
            var organisationVerificationStatus = new OrganisationVerificationStatus();

            var actualResult = _service.SectionStatus(_applicationId, sequenceId, sectionId, applicationSequences, organisationVerificationStatus);

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
            var notRequiredOverrides = new List<NotRequiredOverride>
            {
                new NotRequiredOverride
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
            _notRequiredOverridesService.Setup(x => x.GetNotRequiredOverrides(_applicationId)).ReturnsAsync(notRequiredOverrides);

            var applicationSequences = new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = Guid.NewGuid(), SequenceId = sequenceId,
                    Sections = new List<ApplicationSection> { new ApplicationSection { SectionId = sectionId,
                        QnAData = new QnAData {Pages = new List<Page>()}} } } };
            var organisationVerificationStatus = new OrganisationVerificationStatus();

            var actualResult = _service.SectionStatus(_applicationId, sequenceId, sectionId, applicationSequences, organisationVerificationStatus);

            Assert.AreEqual(expectedResult, actualResult.ToLower());
        }

        [Test]
        public void Get_SectionStatus_Only_One_Of_Multiple_Not_Required_Conditions_Matched()
        {
            var sequenceId = 1;
            var sectionId = 2;
            var applicationRouteId = "3";
            var orgType = "HEI";

            var notRequiredOverrides = new List<NotRequiredOverride>
            {
                new NotRequiredOverride
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
            _notRequiredOverridesService.Setup(x => x.GetNotRequiredOverrides(_applicationId)).ReturnsAsync(notRequiredOverrides);

            var applicationSequences = new List<ApplicationSequence> { new ApplicationSequence { ApplicationId = Guid.NewGuid(), SequenceId = sequenceId,
                    Sections = new List<ApplicationSection> { new ApplicationSection { SectionId = sectionId,
                        QnAData = new QnAData {Pages = new List<Page>()}} } } };
            var organisationVerificationStatus = new OrganisationVerificationStatus();

            var actualResult = _service.SectionStatus(_applicationId, sequenceId, sectionId, applicationSequences, organisationVerificationStatus);
            Assert.IsEmpty(actualResult);
        }

        [Test]
        public void Section_status_is_Next_for_first_your_organisation_section_if_not_complete()
        {
            var sequenceId = 1;
            var sectionId = 1;

            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    SequenceId = 1,
                    Sequential = true,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = 1,
                            QnAData = new QnAData
                            {
                                Pages = new List<Domain.Apply.Page>
                                {                                    
                                    new Domain.Apply.Page
                                    {
                                        PageId = "YO-100",
                                        Questions = new List<Domain.Apply.Question>
                                        {
                                            new Domain.Apply.Question
                                            {
                                                QuestionId = "YO-10"
                                            }
                                        },
                                        Active = true,
                                        Complete = false,
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>()
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var status = _service.SectionStatus(Guid.NewGuid(), sequenceId, sectionId, applicationSequences, new OrganisationVerificationStatus());

            status.Should().Be(TaskListSectionStatus.Next);
        }

        [Test]
        public void Section_status_is_Next_for_second_your_organisation_if_first_completed_and_not_complete()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    SequenceId = 1,
                    Sequential = true,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = 1,
                            QnAData = new QnAData
                            {
                                Pages = new List<Domain.Apply.Page>
                                {
                                    new Domain.Apply.Page
                                    {
                                        PageId = "YO-100",
                                        Questions = new List<Domain.Apply.Question>
                                        {
                                            new Domain.Apply.Question
                                            {
                                                QuestionId = "YO-10"
                                            }
                                        },
                                        Active = true,
                                        Complete = true,
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>
                                        {
                                            new Domain.Apply.PageOfAnswers
                                            {
                                                Id = Guid.NewGuid(),
                                                Answers = new List<Domain.Apply.Answer>
                                                {
                                                    new Domain.Apply.Answer
                                                    {
                                                        QuestionId = "YO-10",
                                                        Value = "Yes"
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = 2,
                            QnAData = new QnAData
                            {
                                Pages = new List<Domain.Apply.Page>
                                {
                                    new Domain.Apply.Page
                                    {
                                        PageId = "YO-110",
                                        Questions = new List<Domain.Apply.Question>
                                        {
                                            new Domain.Apply.Question
                                            {
                                                QuestionId = "YO-20"
                                            }
                                        },
                                        Active = true,
                                        Complete = false,
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>()
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var firstSectionStatus = _service.SectionStatus(Guid.NewGuid(), sequenceId: 1, sectionId: 1, applicationSequences, new OrganisationVerificationStatus());
            var secondSectionStatus = _service.SectionStatus(Guid.NewGuid(), sequenceId: 1, sectionId: 2, applicationSequences, new OrganisationVerificationStatus());

            firstSectionStatus.Should().Be(TaskListSectionStatus.Completed);
            secondSectionStatus.Should().Be(TaskListSectionStatus.Next);
        }

        [Test]
        public void Section_statuses_for_completed_your_organisation_section()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    SequenceId = 1,
                    Sequential = true,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = 1,
                            QnAData = new QnAData
                            {
                                Pages = new List<Domain.Apply.Page>
                                {
                                    new Domain.Apply.Page
                                    {
                                        PageId = "YO-100",
                                        Questions = new List<Domain.Apply.Question>
                                        {
                                            new Domain.Apply.Question
                                            {
                                                QuestionId = "YO-10"
                                            }
                                        },
                                        Active = true,
                                        Complete = true,
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>
                                        {
                                            new Domain.Apply.PageOfAnswers
                                            {
                                                Id = Guid.NewGuid(),
                                                Answers = new List<Domain.Apply.Answer>
                                                {
                                                    new Domain.Apply.Answer
                                                    {
                                                        QuestionId = "YO-10",
                                                        Value = "Yes"
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = 2,
                            QnAData = new QnAData
                            {
                                Pages = new List<Domain.Apply.Page>
                                {
                                    new Domain.Apply.Page
                                    {
                                        PageId = "YO-110",
                                        Questions = new List<Domain.Apply.Question>
                                        {
                                            new Domain.Apply.Question
                                            {
                                                QuestionId = "YO-20"
                                            }
                                        },
                                        Active = true,
                                        Complete = true,
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>
                                        {
                                            new Domain.Apply.PageOfAnswers
                                            {
                                                Id = Guid.NewGuid(),
                                                Answers = new List<Domain.Apply.Answer>
                                                {
                                                    new Domain.Apply.Answer
                                                    {
                                                        QuestionId = "YO-20",
                                                        Value = "No"
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var firstSectionStatus = _service.SectionStatus(Guid.NewGuid(), sequenceId: 1, sectionId: 1, applicationSequences, new OrganisationVerificationStatus());
            var secondSectionStatus = _service.SectionStatus(Guid.NewGuid(), sequenceId: 1, sectionId: 2, applicationSequences, new OrganisationVerificationStatus());

            firstSectionStatus.Should().Be(TaskListSectionStatus.Completed);
            secondSectionStatus.Should().Be(TaskListSectionStatus.Completed);
        }

        [Test]
        public void Section_status_blank_for_sequential_sections_if_previous_sections_are_blank()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    SequenceId = 1,
                    Sequential = true,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = 1,
                            QnAData = new QnAData
                            {
                                Pages = new List<Domain.Apply.Page>
                                {
                                    new Domain.Apply.Page
                                    {
                                        PageId = "YO-100",
                                        Questions = new List<Domain.Apply.Question>
                                        {
                                            new Domain.Apply.Question
                                            {
                                                QuestionId = "YO-10"
                                            }
                                        },
                                        Active = true,
                                        Complete = false,
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>()
                                    }
                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = 2,
                            QnAData = new QnAData
                            {
                                Pages = new List<Domain.Apply.Page>
                                {
                                    new Domain.Apply.Page
                                    {
                                        PageId = "YO-110",
                                        Questions = new List<Domain.Apply.Question>
                                        {
                                            new Domain.Apply.Question
                                            {
                                                QuestionId = "YO-20"
                                            }
                                        },
                                        Active = true,
                                        Complete = false,
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>()
                                    }
                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = 3,
                            QnAData = new QnAData
                            {
                                Pages = new List<Domain.Apply.Page>
                                {
                                    new Domain.Apply.Page
                                    {
                                        PageId = "YO-120",
                                        Questions = new List<Domain.Apply.Question>
                                        {
                                            new Domain.Apply.Question
                                            {
                                                QuestionId = "YO-30"
                                            }
                                        },
                                        Active = true,
                                        Complete = false,
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>()
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var firstSectionStatus = _service.SectionStatus(Guid.NewGuid(), sequenceId: 1, sectionId: 1, applicationSequences, new OrganisationVerificationStatus());
            var secondSectionStatus = _service.SectionStatus(Guid.NewGuid(), sequenceId: 1, sectionId: 2, applicationSequences, new OrganisationVerificationStatus());
            var thirdSectionStatus = _service.SectionStatus(Guid.NewGuid(), sequenceId: 1, sectionId: 3, applicationSequences, new OrganisationVerificationStatus());

            firstSectionStatus.Should().Be(TaskListSectionStatus.Next);
            secondSectionStatus.Should().Be(TaskListSectionStatus.Blank);
            thirdSectionStatus.Should().Be(TaskListSectionStatus.Blank);
        }

        [Test]
        public void Section_status_correct_for_incomplete_non_sequential_sections()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    SequenceId = 2,
                    Sequential = false,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = 1,
                            QnAData = new QnAData
                            {
                                Pages = new List<Domain.Apply.Page>
                                {
                                    new Domain.Apply.Page
                                    {
                                        PageId = "FHA-100",
                                        Questions = new List<Domain.Apply.Question>
                                        {
                                            new Domain.Apply.Question
                                            {
                                                QuestionId = "FHA-10"
                                            }
                                        },
                                        Active = true,
                                        Complete = true,
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>
                                        {
                                            new Domain.Apply.PageOfAnswers
                                            {
                                                Id = Guid.NewGuid(),
                                                Answers = new List<Domain.Apply.Answer>
                                                {
                                                    new Domain.Apply.Answer
                                                    {
                                                        QuestionId = "FHA-10",
                                                        Value = "1 million dollars"
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = 2,
                            QnAData = new QnAData
                            {
                                Pages = new List<Domain.Apply.Page>
                                {
                                    new Domain.Apply.Page
                                    {
                                        PageId = "FHA-110",
                                        Questions = new List<Domain.Apply.Question>
                                        {
                                            new Domain.Apply.Question
                                            {
                                                QuestionId = "FHA-20"
                                            }
                                        },
                                        Active = true,
                                        Complete = false,
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>()
                                    }
                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = 3,
                            QnAData = new QnAData
                            {
                                Pages = new List<Domain.Apply.Page>
                                {
                                    new Domain.Apply.Page
                                    {
                                        PageId = "FHA-120",
                                        Questions = new List<Domain.Apply.Question>
                                        {
                                            new Domain.Apply.Question
                                            {
                                                QuestionId = "FHA-30"
                                            }
                                        },
                                        Active = true,
                                        Complete = false,
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>()
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var firstSectionStatus = _service.SectionStatus(Guid.NewGuid(), sequenceId: 2, sectionId: 1, applicationSequences, new OrganisationVerificationStatus());
            var secondSectionStatus = _service.SectionStatus(Guid.NewGuid(), sequenceId: 2, sectionId: 2, applicationSequences, new OrganisationVerificationStatus());
            var thirdSectionStatus = _service.SectionStatus(Guid.NewGuid(), sequenceId: 2, sectionId: 3, applicationSequences, new OrganisationVerificationStatus());

            firstSectionStatus.Should().Be(TaskListSectionStatus.Completed);
            secondSectionStatus.Should().Be(TaskListSectionStatus.Blank);
            thirdSectionStatus.Should().Be(TaskListSectionStatus.Blank);
        }

        [Test]
        public void Whos_in_control_section_status_shows_as_next_if_companies_house_verified_and_not_confirmed()
        {
            var organisationVerificationStatus = new OrganisationVerificationStatus
            {
                VerifiedCompaniesHouse = true,
                VerifiedCharityCommission = false,
                CompaniesHouseDataConfirmed = false,
                CharityCommissionDataConfirmed = false,
                WhosInControlConfirmed = false
            };

            var applicationSequences = GenerateSectionsCompletedUpToWhosInControl();

            var status = _service.SectionStatus(Guid.NewGuid(), RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, 
                                   applicationSequences, organisationVerificationStatus);

            status.Should().Be(TaskListSectionStatus.Next);
        }

        [Test]
        public void Whos_in_control_section_status_shows_as_next_if_charity_commission_verified_and_not_confirmed()
        {
            var organisationVerificationStatus = new OrganisationVerificationStatus
            {
                VerifiedCompaniesHouse = false,
                VerifiedCharityCommission = true,
                CompaniesHouseDataConfirmed = false,
                CharityCommissionDataConfirmed = false,
                WhosInControlConfirmed = false
            };

            var applicationSequences = GenerateSectionsCompletedUpToWhosInControl();

            var status = _service.SectionStatus(Guid.NewGuid(), RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                                   applicationSequences, organisationVerificationStatus);

            status.Should().Be(TaskListSectionStatus.Next);
        }

        [Test]
        public void Whos_in_control_section_status_shows_as_complete_if_companies_house_verified_and_confirmed()
        {
            var organisationVerificationStatus = new OrganisationVerificationStatus
            {
                VerifiedCompaniesHouse = true,
                VerifiedCharityCommission = false,
                CompaniesHouseDataConfirmed = true,
                CharityCommissionDataConfirmed = false,
                WhosInControlConfirmed = false
            };

            var applicationSequences = GenerateSectionsCompletedUpToWhosInControl();

            var status = _service.SectionStatus(Guid.NewGuid(), RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                                   applicationSequences, organisationVerificationStatus);

            status.Should().Be(TaskListSectionStatus.Completed);
        }

        [Test]
        public void Whos_in_control_section_status_shows_as_in_progress_if_companies_house_and_charity_commission_verified_and_only_confirmed_company()
        {
            var organisationVerificationStatus = new OrganisationVerificationStatus
            {
                VerifiedCompaniesHouse = true,
                VerifiedCharityCommission = true,
                CompaniesHouseDataConfirmed = true,
                CharityCommissionDataConfirmed = false,
                WhosInControlConfirmed = false
            };

            var applicationSequences = GenerateSectionsCompletedUpToWhosInControl();

            var status = _service.SectionStatus(Guid.NewGuid(), RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                                   applicationSequences, organisationVerificationStatus);

            status.Should().Be(TaskListSectionStatus.InProgress);
        }

        [Test]
        public void Whos_in_control_section_status_shows_as_in_progress_if_not_verified_by_companies_house_or_charity_commission_and_whos_in_control_has_started_but_not_confirmed()
        {
            var applicationSequences = GenerateSectionsCompletedUpToWhosInControl();

            var organisationVerificationStatus = new OrganisationVerificationStatus
            {
                VerifiedCompaniesHouse = true,
                VerifiedCharityCommission = true,
                CompaniesHouseDataConfirmed = true,
                CharityCommissionDataConfirmed = false,
                WhosInControlConfirmed = false,
                WhosInControlStarted = true
            };

            var status = _service.SectionStatus(Guid.NewGuid(), RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                                   applicationSequences, organisationVerificationStatus);

            status.Should().Be(TaskListSectionStatus.InProgress);
        }

        [Test]
        public void Whos_in_control_section_status_shows_as_in_progress_if_companies_house_and_charity_commission_verified_and_only_confirmed_charity()
        {
            var organisationVerificationStatus = new OrganisationVerificationStatus
            {
                VerifiedCompaniesHouse = true,
                VerifiedCharityCommission = true,
                CompaniesHouseDataConfirmed = false,
                CharityCommissionDataConfirmed = true,
                WhosInControlConfirmed = false
            };

            var applicationSequences = GenerateSectionsCompletedUpToWhosInControl();

            var status = _service.SectionStatus(Guid.NewGuid(), RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                                   applicationSequences, organisationVerificationStatus);

            status.Should().Be(TaskListSectionStatus.InProgress);
        }

        [Test]
        public void Whos_in_control_section_status_shows_as_completed_if_companies_house_and_charity_commission_verified_and_both_confirmed()
        {
            var organisationVerificationStatus = new OrganisationVerificationStatus
            {
                VerifiedCompaniesHouse = true,
                VerifiedCharityCommission = true,
                CompaniesHouseDataConfirmed = true,
                CharityCommissionDataConfirmed = true,
                WhosInControlConfirmed = false
            };

            var applicationSequences = GenerateSectionsCompletedUpToWhosInControl();

            var status = _service.SectionStatus(Guid.NewGuid(), RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                                   applicationSequences, organisationVerificationStatus);

            status.Should().Be(TaskListSectionStatus.Completed);
        }

        [Test]
        public void Whos_in_control_section_status_shows_as_next_if_not_verified_by_companies_house_or_charity_commission_and_whos_in_control_not_confirmed()
        {
            var organisationVerificationStatus = new OrganisationVerificationStatus
            {
                VerifiedCompaniesHouse = false,
                VerifiedCharityCommission = false,
                CompaniesHouseDataConfirmed = false,
                CharityCommissionDataConfirmed = false,
                WhosInControlConfirmed = false
            };

            var applicationSequences = GenerateSectionsCompletedUpToWhosInControl();

            var status = _service.SectionStatus(Guid.NewGuid(), RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                                   applicationSequences, organisationVerificationStatus);

            status.Should().Be(TaskListSectionStatus.Next);
        }

        [Test]
        public void Whos_in_control_section_status_shows_as_completed_if_not_verified_by_companies_house_or_charity_commission_and_whos_in_control_confirmed()
        {
            var organisationVerificationStatus = new OrganisationVerificationStatus
            {
                VerifiedCompaniesHouse = false,
                VerifiedCharityCommission = false,
                CompaniesHouseDataConfirmed = false,
                CharityCommissionDataConfirmed = false,
                WhosInControlConfirmed = true
            };

            var applicationSequences = GenerateSectionsCompletedUpToWhosInControl();

            var status = _service.SectionStatus(Guid.NewGuid(), RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                                   applicationSequences, organisationVerificationStatus);

            status.Should().Be(TaskListSectionStatus.Completed);
        }

        [Test]
        public void Whos_in_control_section_status_shows_as_complete_if_companies_house_verified_and_not_confirmed_and_manual_entry_confirmed()
        {
            var organisationVerificationStatus = new OrganisationVerificationStatus
            {
                VerifiedCompaniesHouse = true,
                VerifiedCharityCommission = false,
                CompaniesHouseDataConfirmed = false,
                CompaniesHouseManualEntry = true,
                CharityCommissionDataConfirmed = false,
                WhosInControlConfirmed = false
            };

            var applicationSequences = GenerateSectionsCompletedUpToWhosInControl();

            var status = _service.SectionStatus(Guid.NewGuid(), RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                                   applicationSequences, organisationVerificationStatus);

            status.Should().Be(TaskListSectionStatus.Completed);
        }

        [Test]
        public void Whos_in_control_section_status_shows_as_completed_if_companies_house_and_charity_commission_verified_and_neither_confirmed_but_manual_entry_confirmed()
        {
            var organisationVerificationStatus = new OrganisationVerificationStatus
            {
                VerifiedCompaniesHouse = true,
                VerifiedCharityCommission = true,
                CompaniesHouseDataConfirmed = false,
                CompaniesHouseManualEntry = true,
                CharityCommissionDataConfirmed = false,
                CharityCommissionManualEntry = true,
                WhosInControlConfirmed = false
            };

            var applicationSequences = GenerateSectionsCompletedUpToWhosInControl();

            var status = _service.SectionStatus(Guid.NewGuid(), RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                                   applicationSequences, organisationVerificationStatus);

            status.Should().Be(TaskListSectionStatus.Completed);
        }
        
        [Test]
        public void Finish_application_checks_shows_blank_if_not_all_sequences_completed()
        {
            var applicationSequences = new List<ApplicationSequence>();

            var status = _service.FinishSectionStatus(Guid.NewGuid(), RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks, applicationSequences, false);
            
            status.Should().Be(TaskListSectionStatus.Blank);
        }

        [Test]
        public void Finish_application_checks_shows_next_if_all_sequences_completed_and_section_not_started()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    SequenceId = RoatpWorkflowSequenceIds.Finish,
                    Sequential = true,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks,
                            QnAData = new QnAData
                            {
                                Pages = new List<Domain.Apply.Page>
                                {
                                    new Domain.Apply.Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        PageId = "FIN-1",
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>()
                                    }
                                }
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()))
                         .ReturnsAsync(applicationSequences[0].Sections[0]);

            var status = _service.FinishSectionStatus(Guid.NewGuid(), RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks, applicationSequences, true);

            status.Should().Be(TaskListSectionStatus.Next);
        }

        [Test]
        public void Finish_application_checks_shows_in_progress_if_all_not_questions_answered()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    SequenceId = RoatpWorkflowSequenceIds.Finish,
                    Sequential = true,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks,
                            QnAData = new QnAData
                            {
                                Pages = new List<Domain.Apply.Page>
                                {
                                    new Domain.Apply.Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        PageId = "FIN-1",
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>
                                        {
                                            new Domain.Apply.PageOfAnswers
                                            {
                                                Id = Guid.NewGuid(),
                                                Answers = new List<Domain.Apply.Answer>
                                                {
                                                    new Domain.Apply.Answer
                                                    {
                                                        QuestionId = "FIN-1",
                                                        Value = "Yes"
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Domain.Apply.Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        PageId = "FIN-2",
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>()
                                    }
                                }
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()))
                         .ReturnsAsync(applicationSequences[0].Sections[0]);

            var status = _service.FinishSectionStatus(Guid.NewGuid(), RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks, applicationSequences, true);

            status.Should().Be(TaskListSectionStatus.InProgress);
        }

        [TestCase("Yes", "No", "Yes")]
        [TestCase("No", "No", "Yes")]
        [TestCase("Yes", "No", "No")]
        [TestCase("No", "Yes", "Yes")]
        [TestCase("Yes", "Yes", "No")]
        [TestCase("No", "Yes", "No")]
        public void Finish_application_checks_shows_in_progress_if_questions_answered_and_shutter_page_shown_for_conditions_not_met(string answer1, string answer2, string answer3)
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    SequenceId = RoatpWorkflowSequenceIds.Finish,
                    Sequential = true,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks,
                            QnAData = new QnAData
                            {
                                Pages = new List<Domain.Apply.Page>
                                {
                                    new Domain.Apply.Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        PageId = "FIN-1",
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>
                                        {
                                            new Domain.Apply.PageOfAnswers
                                            {
                                                Id = Guid.NewGuid(),
                                                Answers = new List<Domain.Apply.Answer>
                                                {
                                                    new Domain.Apply.Answer
                                                    {
                                                        QuestionId = "FIN-1",
                                                        Value = answer1
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Domain.Apply.Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        PageId = "FIN-2",
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>
                                        {
                                            new Domain.Apply.PageOfAnswers
                                            {
                                                Id = Guid.NewGuid(),
                                                Answers = new List<Domain.Apply.Answer>
                                                {
                                                    new Domain.Apply.Answer
                                                    {
                                                        QuestionId = "FIN-2",
                                                        Value = answer2
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Domain.Apply.Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        PageId = "FIN-3",
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>
                                        {
                                            new Domain.Apply.PageOfAnswers
                                            {
                                                Id = Guid.NewGuid(),
                                                Answers = new List<Domain.Apply.Answer>
                                                {
                                                    new Domain.Apply.Answer
                                                    {
                                                        QuestionId = "FIN-3",
                                                        Value = answer3
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()))
                         .ReturnsAsync(applicationSequences[0].Sections[0]);

            var status = _service.FinishSectionStatus(Guid.NewGuid(), RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks, applicationSequences, true);

            status.Should().Be(TaskListSectionStatus.InProgress);
        }

        [Test]
        public void Finish_application_checks_shows_complete_if_all_conditions_agreed_to()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    SequenceId = RoatpWorkflowSequenceIds.Finish,
                    Sequential = true,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks,
                            QnAData = new QnAData
                            {
                                Pages = new List<Domain.Apply.Page>
                                {
                                    new Domain.Apply.Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        PageId = "FIN-1",
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>
                                        {
                                            new Domain.Apply.PageOfAnswers
                                            {
                                                Id = Guid.NewGuid(),
                                                Answers = new List<Domain.Apply.Answer>
                                                {
                                                    new Domain.Apply.Answer
                                                    {
                                                        QuestionId = "FIN-1",
                                                        Value = "Yes"
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Domain.Apply.Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        PageId = "FIN-2",
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>
                                        {
                                            new Domain.Apply.PageOfAnswers
                                            {
                                                Id = Guid.NewGuid(),
                                                Answers = new List<Domain.Apply.Answer>
                                                {
                                                    new Domain.Apply.Answer
                                                    {
                                                        QuestionId = "FIN-2",
                                                        Value = "Yes"
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Domain.Apply.Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        PageId = "FIN-3",
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>
                                        {
                                            new Domain.Apply.PageOfAnswers
                                            {
                                                Id = Guid.NewGuid(),
                                                Answers = new List<Domain.Apply.Answer>
                                                {
                                                    new Domain.Apply.Answer
                                                    {
                                                        QuestionId = "FIN-3",
                                                        Value = "Yes"
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()))
                         .ReturnsAsync(applicationSequences[0].Sections[0]);

            var status = _service.FinishSectionStatus(Guid.NewGuid(), RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks, applicationSequences, true);

            status.Should().Be(TaskListSectionStatus.Completed);
        }

        private List<ApplicationSequence> GenerateSectionsCompletedUpToWhosInControl()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    SequenceId = 1,
                    Sequential = true,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = 1,
                            QnAData = new QnAData
                            {
                                Pages = new List<Domain.Apply.Page>
                                {
                                    new Domain.Apply.Page
                                    {
                                        PageId = "YO-100",
                                        Questions = new List<Domain.Apply.Question>
                                        {
                                            new Domain.Apply.Question
                                            {
                                                QuestionId = "YO-10"
                                            }
                                        },
                                        Active = true,
                                        Complete = true,
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>
                                        {
                                            new Domain.Apply.PageOfAnswers
                                            {
                                                Id = Guid.NewGuid(),
                                                Answers = new List<Domain.Apply.Answer>
                                                {
                                                    new Domain.Apply.Answer
                                                    {
                                                        QuestionId = "YO-10",
                                                        Value = "Yes"
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = 2,
                            QnAData = new QnAData
                            {
                                Pages = new List<Domain.Apply.Page>
                                {
                                    new Domain.Apply.Page
                                    {
                                        PageId = "YO-110",
                                        Questions = new List<Domain.Apply.Question>
                                        {
                                            new Domain.Apply.Question
                                            {
                                                QuestionId = "YO-20"
                                            }
                                        },
                                        Active = true,
                                        Complete = true,
                                        PageOfAnswers = new List<Domain.Apply.PageOfAnswers>
                                        {
                                            new Domain.Apply.PageOfAnswers
                                            {
                                                Id = Guid.NewGuid(),
                                                Answers = new List<Domain.Apply.Answer>
                                                {
                                                    new Domain.Apply.Answer
                                                    {
                                                        QuestionId = "YO-20",
                                                        Value = "No"
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };


            return applicationSequences;
        }
    }
}
