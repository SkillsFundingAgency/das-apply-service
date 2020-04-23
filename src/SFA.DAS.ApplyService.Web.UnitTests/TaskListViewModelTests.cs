namespace SFA.DAS.ApplyService.Web.UnitTests
{
    using System;
    using Domain.Entities;
    using NUnit.Framework;
    using System.Collections.Generic;
    using Application.Apply.Roatp;
    using SFA.DAS.ApplyService.Web.Services;
    using Domain.Apply;
    using FluentAssertions;
    using ViewModels.Roatp;
    using Moq;
    using SFA.DAS.ApplyService.Web.Infrastructure;

    [TestFixture]
    public class TaskListViewModelTests
    {
        private Guid _applicationId;
        private List<ApplicationSequence> _applicationSequences;
        private ApplicationSequence _yourApplicationSequence;
        private ApplicationSection _orgDetailsSection1;
        private ApplicationSection _orgDetailsSection2;
        private Mock<IQnaApiClient> _qnaApiClient;

        private const string MainProviderRoute = "1";
        private const string EmployerProviderRoute = "2";
        private const string SupportingProviderRoute = "3";

        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();
            _applicationSequences = new List<ApplicationSequence>();
            _yourApplicationSequence = new ApplicationSequence
            {
                Id = Guid.NewGuid(),
                ApplicationId = _applicationId,
                SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                Sections = new List<ApplicationSection>(),
                Sequential = true
            };
            _orgDetailsSection1 = new ApplicationSection
            {
                SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed
            };
            _orgDetailsSection2 = new ApplicationSection
            {
                SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails
            };

            _yourApplicationSequence.Sections.Add(_orgDetailsSection1);
            _yourApplicationSequence.Sections.Add(_orgDetailsSection2);
            _applicationSequences.Add(_yourApplicationSequence);
            _qnaApiClient = new Mock<IQnaApiClient>();
        }

        [Test]
        public void Task_list_shows_next_for_first_your_organisation_section_if_not_complete()
        {
            // This should be prefilled in the pre-amble but test in case the sequencing changes

            _orgDetailsSection1.QnAData = new QnAData
            {
                Pages = new List<Page>
                {
                    new Page
                    {
                        PageId = "1",
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                QuestionId = "YO-1"
                            }
                        },
                        PageOfAnswers = new List<PageOfAnswers>()
                    }
                }
            };

            _orgDetailsSection2.QnAData = new QnAData
            {
                Pages = new List<Page>
                {
                    new Page
                    {
                        PageId = "2",
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                QuestionId = "YO-2"
                            }
                        },
                        PageOfAnswers = new List<PageOfAnswers>()
                    }
                }
            };

            var model = new TaskListViewModel(_qnaApiClient.Object)
            {
                ApplicationId = _applicationId,
                ApplicationSequences = _applicationSequences,
                UKPRN = "10001234",
                OrganisationName = "Org Name"
            };

            model.SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed).Should().Be(TaskListSectionStatus.Next);
            model.SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails).ToLower().Should().Be(TaskListSectionStatus.Blank);
        }

        [Test]
        public void Task_list_shows_next_for_second_your_organisation_section_if_not_complete()
        {
            _orgDetailsSection1.QnAData = new QnAData
            {
                Pages = new List<Page>
                {
                    new Page
                    {
                        PageId = "1",
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                QuestionId = "YO-1"
                            }
                        },
                        PageOfAnswers = new List<PageOfAnswers>
                        {
                            new PageOfAnswers
                            {
                                Id = Guid.NewGuid(),
                                Answers = new List<Answer>
                                {
                                    new Answer {QuestionId = "YO-1", Value = "1"}
                                }
                            }
                        },
                        Active = true,
                        Complete = true
                    }
                    
                }
            };

            _orgDetailsSection2.QnAData = new QnAData
            {
                Pages = new List<Page>
                {
                    new Page
                    {
                        PageId = "2",
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                QuestionId = "YO-2"
                            }
                        },
                        PageOfAnswers = new List<PageOfAnswers>()
                    }
                }
            };

            var model = new TaskListViewModel(_qnaApiClient.Object)
            {
                ApplicationId = _applicationId,
                ApplicationSequences = _applicationSequences,
                UKPRN = "10001234",
                OrganisationName = "Org Name"
            };

            model.SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed).Should().Be(TaskListSectionStatus.Completed);
            model.SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails).Should().Be(TaskListSectionStatus.Next);
        }

        [Test]
        public void Task_list_shows_completed_your_organisation_section()
        {
            _orgDetailsSection1.QnAData = new QnAData
            {
                Pages = new List<Page>
                {
                    new Page
                    {
                        PageId = "1",
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                QuestionId = "YO-1"
                            }
                        },
                        PageOfAnswers = new List<PageOfAnswers>
                        {
                            new PageOfAnswers
                            {
                                Id = Guid.NewGuid(),
                                Answers = new List<Answer>
                                {
                                    new Answer {QuestionId = "YO-1", Value = "1"}
                                }
                            }
                        },
                        Active = true,
                        Complete = true
                    }
                }
            };

            _orgDetailsSection2.QnAData = new QnAData
            {
                Pages = new List<Page>
                {
                    new Page
                    {
                        PageId = "2",
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                QuestionId = "YO-2"
                            }
                        },
                        PageOfAnswers = new List<PageOfAnswers>
                        {
                            new PageOfAnswers
                            {
                                Id = Guid.NewGuid(),
                                Answers = new List<Answer>
                                {
                                    new Answer {QuestionId = "YO-2", Value = "1"}
                                }
                            }
                        },
                        Active = true,
                        Complete = true
                    }
                }
            };

            var model = new TaskListViewModel(_qnaApiClient.Object)
            {
                ApplicationId = _applicationId,
                ApplicationSequences = _applicationSequences,
                UKPRN = "10001234",
                OrganisationName = "Org Name"
            };

            model.SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed).Should().Be(TaskListSectionStatus.Completed);
            model.SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails).Should().Be(TaskListSectionStatus.Completed);
        }

        [Test]
        public void Task_list_shows_blank_for_sequential_sections_if_previous_sections_are_blank()
        {
            _orgDetailsSection1.QnAData = new QnAData
            {
                Pages = new List<Page>
                {
                    new Page
                    {
                        PageId = "1",
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                QuestionId = "YO-1"
                            }
                        },
                        PageOfAnswers = new List<PageOfAnswers>
                        {
                            new PageOfAnswers
                            {
                                Id = Guid.NewGuid(),
                                Answers = new List<Answer>()
                            }
                        },
                        Active = true,
                        Complete = false
                    }
                }
            };

            _orgDetailsSection2.QnAData = new QnAData
            {
                Pages = new List<Page>
                {
                    new Page
                    {
                        PageId = "2",
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                QuestionId = "YO-2"
                            }
                        },
                        PageOfAnswers = new List<PageOfAnswers>
                        {
                            new PageOfAnswers
                            {
                                Id = Guid.NewGuid(),
                                Answers = new List<Answer>()
                            }
                        },
                        Active = true,
                        Complete = false
                    }
                }
            };

            var orgDetailsSection3 = new ApplicationSection
            {
                SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation,
                QnAData = new QnAData
                {
                    Pages = new List<Page>
                    {
                        new Page
                        {
                            PageId = "3",
                            Questions = new List<Question>
                            {
                                new Question
                                {
                                    QuestionId = "YO-3"
                                }
                            },
                            PageOfAnswers = new List<PageOfAnswers>
                            {
                                new PageOfAnswers
                                {
                                    Id = Guid.NewGuid(),
                                    Answers = new List<Answer> 
                                    {
                                        new Answer
                                        {
                                            QuestionId = "YO-3",
                                            Value = "Y"
                                        }
                                    }
                                }
                            },
                            Active = true,
                            Complete = true
                        }
                    }
                }
            };
            _yourApplicationSequence.Sections.Add(orgDetailsSection3);

            var model = new TaskListViewModel(_qnaApiClient.Object)
            {
                ApplicationId = _applicationId,
                ApplicationSequences = _applicationSequences,
                UKPRN = "10001234",
                OrganisationName = "Org Name"
            };

            model.SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed).Should().Be(TaskListSectionStatus.Next);
            model.SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails).Should().Be(TaskListSectionStatus.Blank);
            model.SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation).Should().Be(TaskListSectionStatus.Blank);
        }

        [Test]
        public void Task_list_shows_correct_status_for_incomplete_non_sequential_section()
        {
            var criminalComplianceSequence = new ApplicationSequence
            {
                Id = Guid.NewGuid(),
                ApplicationId = _applicationId,
                SequenceId = RoatpWorkflowSequenceIds.CriminalComplianceChecks,
                Sections = new List<ApplicationSection>()
            };
            var criminalWhatYouNeedSection = new ApplicationSection
            {
                SequenceId = RoatpWorkflowSequenceIds.CriminalComplianceChecks,
                SectionId = RoatpWorkflowSectionIds.CriminalComplianceChecks.WhatYouWillNeed,
                QnAData = new QnAData
                {
                    Pages = new List<Page>
                    {
                        new Page
                        {
                            PageId = "1",
                            Questions = new List<Question>
                            {
                                new Question
                                {
                                    QuestionId = "CC-1"
                                }
                            },
                            PageOfAnswers = new List<PageOfAnswers>
                            {
                                new PageOfAnswers
                                {
                                    Id = Guid.NewGuid(),
                                    Answers = new List<Answer>
                                    {
                                        new Answer {QuestionId = "CC-1", Value = "1"}
                                    }
                                }
                            },
                            Active = true,
                            Complete = true
                        }
                    }
                }
            };

            var criminalOrganisationChecksSection = new ApplicationSection
            {
                SequenceId = RoatpWorkflowSequenceIds.CriminalComplianceChecks,
                SectionId = RoatpWorkflowSectionIds.CriminalComplianceChecks.ChecksOnYourOrganisation,
                QnAData = new QnAData
                {
                    Pages = new List<Page>
                    {
                        new Page
                        {
                            PageId = "1",
                            Questions = new List<Question>
                            {
                                new Question
                                {
                                    QuestionId = "CC-1"
                                },
                                new Question
                                {
                                    QuestionId = "CC-2"
                                },
                                new Question
                                {
                                    QuestionId = "CC-3"
                                }
                            },
                            PageOfAnswers = new List<PageOfAnswers>
                            {
                                new PageOfAnswers
                                {
                                    Id = Guid.NewGuid(),
                                    Answers = new List<Answer>
                                    {
                                        new Answer {QuestionId = "CC-1", Value = "1"},
                                        new Answer {QuestionId = "CC-2", Value = "1"}
                                    }
                                }
                            }
                        }
                    }
                }
            };
            var criminalIndividualChecksSection = new ApplicationSection
            {
                SequenceId = RoatpWorkflowSequenceIds.CriminalComplianceChecks,
                SectionId = RoatpWorkflowSectionIds.CriminalComplianceChecks.CheckOnWhosInControl,
                QnAData = new QnAData
                {
                    Pages = new List<Page>
                    {
                        new Page
                        {
                            PageId = "1",
                            Questions = new List<Question>
                            {
                                new Question
                                {
                                    QuestionId = "CC-10"
                                },
                                new Question
                                {
                                    QuestionId = "CC-20"
                                },
                                new Question
                                {
                                    QuestionId = "CC-30"
                                }
                            },
                            PageOfAnswers = new List<PageOfAnswers>()
                        }
                    }
                }
            };
           criminalComplianceSequence.Sections.Add(criminalWhatYouNeedSection);
           criminalComplianceSequence.Sections.Add(criminalOrganisationChecksSection);
            criminalComplianceSequence.Sections.Add(criminalIndividualChecksSection);
            _applicationSequences.Add(criminalComplianceSequence);

            var model = new TaskListViewModel(_qnaApiClient.Object)
            {
                ApplicationId = _applicationId,
                ApplicationSequences = _applicationSequences,
                UKPRN = "10001234",
                OrganisationName = "Org Name"
            };

            model.SectionStatus(RoatpWorkflowSequenceIds.CriminalComplianceChecks,
                RoatpWorkflowSectionIds.CriminalComplianceChecks.WhatYouWillNeed).Should().Be(TaskListSectionStatus.Completed);
            model.SectionStatus(RoatpWorkflowSequenceIds.CriminalComplianceChecks,
                RoatpWorkflowSectionIds.CriminalComplianceChecks.ChecksOnYourOrganisation).Should().Be(TaskListSectionStatus.InProgress);
            model.SectionStatus(RoatpWorkflowSequenceIds.CriminalComplianceChecks,
                RoatpWorkflowSectionIds.CriminalComplianceChecks.CheckOnWhosInControl).Should().Be(TaskListSectionStatus.Blank);
        }

        [Test]
        public void Whos_in_control_section_status_shows_as_next_if_companies_house_verified_and_not_confirmed()
        {
            var model = GetTaskListViewModelWithSectionsUpToWhosInControlCompleted();

            model.VerifiedCompaniesHouse = true;
            model.VerifiedCharityCommission = false;
            model.CompaniesHouseDataConfirmed = false;
            model.CharityCommissionDataConfirmed = false;
            model.WhosInControlConfirmed = false;

            model.WhosInControlSectionStatus.Should().Be(TaskListSectionStatus.Next);
        }

        [Test]
        public void Whos_in_control_section_status_shows_as_next_if_charity_commission_verified_and_not_confirmed()
        {
            var model = GetTaskListViewModelWithSectionsUpToWhosInControlCompleted();

            model.VerifiedCompaniesHouse = false;
            model.VerifiedCharityCommission = true;
            model.CompaniesHouseDataConfirmed = false;
            model.CharityCommissionDataConfirmed = false;
            model.WhosInControlConfirmed = false;
            
            model.WhosInControlSectionStatus.Should().Be(TaskListSectionStatus.Next);
        }
        
        [Test]
        public void Whos_in_control_section_status_shows_as_complete_if_companies_house_verified_and_confirmed()
        {
            var model = GetTaskListViewModelWithSectionsUpToWhosInControlCompleted();
            
            model.VerifiedCompaniesHouse = true;
            model.VerifiedCharityCommission = false;
            model.CompaniesHouseDataConfirmed = true;
            model.CharityCommissionDataConfirmed = false;
            model.WhosInControlConfirmed = false;
            
            model.WhosInControlSectionStatus.Should().Be(TaskListSectionStatus.Completed);
        }


        [Test]
        public void Whos_in_control_section_status_shows_as_complete_if_companies_house_verified_and_not_confirmed_and_manual_entry_confirmed()
        {
            var model = GetTaskListViewModelWithSectionsUpToWhosInControlCompleted();

            model.VerifiedCompaniesHouse = true;
            model.VerifiedCharityCommission = false;
            model.CompaniesHouseDataConfirmed = false;
            model.CompaniesHouseManualEntry = true;
            model.CharityCommissionDataConfirmed = false;
            model.WhosInControlConfirmed = false;

            model.WhosInControlSectionStatus.Should().Be(TaskListSectionStatus.Completed);
        }

        [Test]
        public void Whos_in_control_section_status_shows_as_in_progress_if_companies_house_and_charity_commission_verified_and_only_confirmed_company()
        {
            var model = GetTaskListViewModelWithSectionsUpToWhosInControlCompleted();

            model.VerifiedCompaniesHouse = true;
            model.VerifiedCharityCommission = true;
            model.CompaniesHouseDataConfirmed = true;
            model.CharityCommissionDataConfirmed = false;
            model.WhosInControlConfirmed = false;
            
            model.WhosInControlSectionStatus.Should().Be(TaskListSectionStatus.InProgress);
        }

        [Test]
        public void Whos_in_control_section_status_shows_as_in_progress_if_companies_house_and_charity_commission_verified_and_only_confirmed_charity()
        {
            var model = GetTaskListViewModelWithSectionsUpToWhosInControlCompleted();

            model.VerifiedCompaniesHouse = true;
            model.VerifiedCharityCommission = true;
            model.CompaniesHouseDataConfirmed = false;
            model.CharityCommissionDataConfirmed = true;
            model.WhosInControlConfirmed = false;
            
            model.WhosInControlSectionStatus.Should().Be(TaskListSectionStatus.InProgress);
        }

        [Test]
        public void Whos_in_control_section_status_shows_as_completed_if_companies_house_and_charity_commission_verified_and_both_confirmed()
        {
            var model = GetTaskListViewModelWithSectionsUpToWhosInControlCompleted();

            model.VerifiedCompaniesHouse = true;
            model.VerifiedCharityCommission = true;
            model.CompaniesHouseDataConfirmed = true;
            model.CharityCommissionDataConfirmed = true;
            model.WhosInControlConfirmed = false;
            
            model.WhosInControlSectionStatus.Should().Be(TaskListSectionStatus.Completed);
        }

        [Test]
        public void Whos_in_control_section_status_shows_as_completed_if_companies_house_and_charity_commission_verified_and_neither_confirmed_but_manual_entry_confirmed()
        {
            var model = GetTaskListViewModelWithSectionsUpToWhosInControlCompleted();

            model.VerifiedCompaniesHouse = true;
            model.VerifiedCharityCommission = true;
            model.CompaniesHouseDataConfirmed = false;
            model.CompaniesHouseManualEntry = true;
            model.CharityCommissionDataConfirmed = false;
            model.CharityCommissionManualEntry = true;
            model.WhosInControlConfirmed = false;
            model.WhosInControlStarted = false;

            model.WhosInControlSectionStatus.Should().Be(TaskListSectionStatus.Completed);
        }

        [Test]
        public void Whos_in_control_section_status_shows_as_pending_if_not_verified_by_companies_house_or_charity_commission_and_whos_in_control_has_started_but_not_confirmed()
        {
            var model = GetTaskListViewModelWithSectionsUpToWhosInControlCompleted();

            model.VerifiedCompaniesHouse = false;
            model.VerifiedCharityCommission = false;
            model.CompaniesHouseDataConfirmed = false;
            model.CharityCommissionDataConfirmed = false;
            model.WhosInControlConfirmed = false;
            model.WhosInControlStarted = true;

            model.WhosInControlSectionStatus.Should().Be(TaskListSectionStatus.InProgress);
        }


        [Test]
        public void Whos_in_control_section_status_shows_as_next_if_not_verified_by_companies_house_or_charity_commission_and_whos_in_control_not_confirmed()
        {
            var model = GetTaskListViewModelWithSectionsUpToWhosInControlCompleted();

            model.VerifiedCompaniesHouse = false;
            model.VerifiedCharityCommission = false;
            model.CompaniesHouseDataConfirmed = false;
            model.CharityCommissionDataConfirmed = false;
            model.WhosInControlConfirmed = false;
            
            model.WhosInControlSectionStatus.Should().Be(TaskListSectionStatus.Next);
        }

        [Test]
        public void Whos_in_control_section_status_shows_as_completed_if_not_verified_by_companies_house_or_charity_commission_and_whos_in_control_confirmed()
        {
            var model = GetTaskListViewModelWithSectionsUpToWhosInControlCompleted();

            model.VerifiedCompaniesHouse = false;
            model.VerifiedCharityCommission = false;
            model.CompaniesHouseDataConfirmed = false;
            model.CharityCommissionDataConfirmed = false;
            model.WhosInControlConfirmed = true;
            
            model.WhosInControlSectionStatus.Should().Be(TaskListSectionStatus.Completed);
        }

        [Test]
        public void Finish_application_checks_shows_blank_if_not_all_sequences_completed()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {                                        
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                },
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.Finish,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.TermsAndConditions,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var model = new TaskListViewModel(_qnaApiClient.Object)
            {
                ApplicationSequences = applicationSequences
            };

            var status = model.FinishSectionStatus(RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks);
            status.Should().Be(TaskListSectionStatus.Blank);
            var css = model.FinishCss(RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks);
            css.Should().Be("hidden");
        }

        [Test]
        public void Finish_application_checks_shows_next_if_all_sequences_completed_and_section_not_started()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = 1,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                },
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.Finish,
                    Sequential = true,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionPersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = null });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishAccuratePersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = null });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionSubmitApplication, It.IsAny<string>())).ReturnsAsync(new Answer { Value = null });

            var model = new TaskListViewModel(_qnaApiClient.Object)
            {
                ApplicationSequences = applicationSequences
            };

            var status = model.FinishSectionStatus(RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks);
            status.Should().Be(TaskListSectionStatus.Next);
            var css = model.FinishCss(RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks);
            css.Should().Be(TaskListSectionStatus.Next.ToLower());
        }

        [Test]
        public void Finish_application_checks_shows_in_progress_if_all_not_questions_answered()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = 1,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                },
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.Finish,
                    Sequential = true,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionPersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishAccuratePersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = null });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionSubmitApplication, It.IsAny<string>())).ReturnsAsync(new Answer { Value = null });
            
            var model = new TaskListViewModel(_qnaApiClient.Object)
            {
                ApplicationSequences = applicationSequences
            };

            var status = model.FinishSectionStatus(RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks);
            status.Should().Be(TaskListSectionStatus.InProgress);
            var css = model.FinishCss(RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks);
            css.Should().Be("inprogress");
        }

        [Test]
        public void Finish_application_checks_shows_in_progress_if_questions_answered_and_shutter_page_shown_for_conditions_not_met()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                },
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.Finish,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.TermsAndConditions,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }

                                }
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionPersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishAccuratePersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "No" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionSubmitApplication, It.IsAny<string>())).ReturnsAsync(new Answer { Value = null });

            var model = new TaskListViewModel(_qnaApiClient.Object)
            {
                ApplicationSequences = applicationSequences
            };

            var status = model.FinishSectionStatus(RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks);
            status.Should().Be(TaskListSectionStatus.InProgress);
            var css = model.FinishCss(RoatpWorkflowSectionIds.Finish.ApplicationPermissionsAndChecks);
            css.Should().Be("inprogress");
        }

        [Test]
        public void Finish_commercial_in_confidence_shows_blank_if_previous_section_not_complete()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                },
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.Finish,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.TermsAndConditions,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }

                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.CommercialInConfidenceInformation,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                }
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionPersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishAccuratePersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "No" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionSubmitApplication, It.IsAny<string>())).ReturnsAsync(new Answer { Value = null });

            var model = new TaskListViewModel(_qnaApiClient.Object)
            {
                ApplicationSequences = applicationSequences
            };

            var status = model.FinishSectionStatus(RoatpWorkflowSectionIds.Finish.CommercialInConfidenceInformation);
            status.Should().Be(TaskListSectionStatus.Next);
            var css = model.FinishCss(RoatpWorkflowSectionIds.Finish.CommercialInConfidenceInformation);
            css.Should().Be("next");
        }

        [Test]
        public void Finish_commercial_in_confidence_shows_next_if_no_questions_answered_and_previous_section_complete()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                },
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.Finish,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.TermsAndConditions,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }

                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.CommercialInConfidenceInformation,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                }
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionPersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishAccuratePersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionSubmitApplication, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishCommercialInConfidence, It.IsAny<string>())).ReturnsAsync(new Answer { Value = null });

            var model = new TaskListViewModel(_qnaApiClient.Object)
            {
                ApplicationSequences = applicationSequences
            };

            var status = model.FinishSectionStatus(RoatpWorkflowSectionIds.Finish.CommercialInConfidenceInformation);
            status.Should().Be(TaskListSectionStatus.Next);
            var css = model.FinishCss(RoatpWorkflowSectionIds.Finish.CommercialInConfidenceInformation);
            css.Should().Be(TaskListSectionStatus.Next.ToLower());
        }

        [TestCase("Yes")]
        [TestCase("No")]
        public void Finish_commercial_in_confidence_shows_completed_if_answer_supplied(string answerValue)
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                },
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.Finish,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.TermsAndConditions,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }

                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.CommercialInConfidenceInformation,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                }
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionPersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishAccuratePersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionSubmitApplication, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishCommercialInConfidence, It.IsAny<string>())).ReturnsAsync(new Answer { Value = answerValue });

            var model = new TaskListViewModel(_qnaApiClient.Object)
            {
                ApplicationSequences = applicationSequences
            };


            var status = model.FinishSectionStatus(RoatpWorkflowSectionIds.Finish.CommercialInConfidenceInformation);
            
            var css = model.FinishCss(RoatpWorkflowSectionIds.Finish.CommercialInConfidenceInformation);
           
            if (answerValue.Equals("yes", StringComparison.InvariantCultureIgnoreCase))
            {
                status.Should().Be(TaskListSectionStatus.Completed);
                css.Should().Be(TaskListSectionStatus.Completed.ToLower());
            }
            else
            {
                status.Should().Be(TaskListSectionStatus.InProgress);
                css.Should().Be(TaskListSectionStatus.InProgress.Replace(" ", string.Empty).ToLower());
            }
        }

        [Test]
        public void Finish_terms_and_conditions_shows_blank_if_previous_section_not_complete()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                },
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.Finish,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.TermsAndConditions,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }

                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.CommercialInConfidenceInformation,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                }
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionPersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishAccuratePersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionSubmitApplication, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishCommercialInConfidence, It.IsAny<string>())).ReturnsAsync(new Answer { Value = null });

            var model = new TaskListViewModel(_qnaApiClient.Object)
            {
                ApplicationSequences = applicationSequences
            };

            var status = model.FinishSectionStatus(RoatpWorkflowSectionIds.Finish.TermsAndConditions);
            status.Should().Be(TaskListSectionStatus.Next);
            var css = model.FinishCss(RoatpWorkflowSectionIds.Finish.TermsAndConditions);
            css.Should().Be("next");
        }

        [Test]
        public void Finish_terms_and_conditions_shows_next_if_no_questions_answered_and_previous_section_complete()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                },
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.Finish,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.TermsAndConditions,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }

                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.CommercialInConfidenceInformation,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.TermsAndConditions,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                }
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionPersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishAccuratePersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionSubmitApplication, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishCommercialInConfidence, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "No" });

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishCOA2MainEmployer, It.IsAny<string>())).ReturnsAsync(new Answer { Value = null });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishCOA3MainEmployer, It.IsAny<string>())).ReturnsAsync(new Answer { Value = null });

            var model = new TaskListViewModel(_qnaApiClient.Object)
            {
                ApplicationSequences = applicationSequences,
                ApplicationRouteId = MainProviderRoute
            };

            var status = model.FinishSectionStatus(RoatpWorkflowSectionIds.Finish.TermsAndConditions);
            status.Should().Be(TaskListSectionStatus.Next);
            var css = model.FinishCss(RoatpWorkflowSectionIds.Finish.TermsAndConditions);
            css.Should().Be("next");
        }

        [Test]
        public void Finish_terms_and_conditions_shows_in_progress_if_not_all_questions_answered()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                },
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.Finish,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.TermsAndConditions,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }

                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.CommercialInConfidenceInformation,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.TermsAndConditions,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                }
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionPersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishAccuratePersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionSubmitApplication, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishCommercialInConfidence, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "No" });

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishCOA2Supporting, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishCOA3Supporting, It.IsAny<string>())).ReturnsAsync(new Answer { Value = null });

            var model = new TaskListViewModel(_qnaApiClient.Object)
            {
                ApplicationSequences = applicationSequences,
                ApplicationRouteId = SupportingProviderRoute
            };

            var status = model.FinishSectionStatus(RoatpWorkflowSectionIds.Finish.TermsAndConditions);
            status.Should().Be(TaskListSectionStatus.NotRequired);
            var css = model.FinishCss(RoatpWorkflowSectionIds.Finish.TermsAndConditions);
            css.Should().Be("notrequired");
        }

        [Test]
        public void Finish_terms_and_conditions_shows_in_progress_if_questions_answered_and_shutter_page_shown_for_conditions_not_met()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                },
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.Finish,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.TermsAndConditions,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }

                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.CommercialInConfidenceInformation,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.TermsAndConditions,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                }
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionPersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishAccuratePersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionSubmitApplication, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishCommercialInConfidence, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "No" });

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishCOA2Supporting, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishCOA3Supporting, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "No" });

            var model = new TaskListViewModel(_qnaApiClient.Object)
            {
                ApplicationSequences = applicationSequences,
                ApplicationRouteId = SupportingProviderRoute
            };

            var status = model.FinishSectionStatus(RoatpWorkflowSectionIds.Finish.TermsAndConditions);
            status.Should().Be(TaskListSectionStatus.NotRequired);
            var css = model.FinishCss(RoatpWorkflowSectionIds.Finish.TermsAndConditions);
            css.Should().Be("notrequired");
        }

        [Test]
        public void Finish_terms_and_conditions_shows_completed_if_all_questions_answered()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                },
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.Finish,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.TermsAndConditions,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }

                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.CommercialInConfidenceInformation,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.TermsAndConditions,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                }
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionPersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishAccuratePersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionSubmitApplication, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishCommercialInConfidence, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "No" });

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishCOA2MainEmployer, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishCOA3MainEmployer, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });

            var model = new TaskListViewModel(_qnaApiClient.Object)
            {
                ApplicationSequences = applicationSequences,
                ApplicationRouteId = EmployerProviderRoute
            };

            var status = model.FinishSectionStatus(RoatpWorkflowSectionIds.Finish.TermsAndConditions);
            status.Should().Be(TaskListSectionStatus.Completed);
            var css = model.FinishCss(RoatpWorkflowSectionIds.Finish.TermsAndConditions);
            css.Should().Be(TaskListSectionStatus.Completed.ToLower());
        }

        [Test]
        public void Finish_submit_application_shows_next_if_previous_section_completed()
        {
            var applicationSequences = new List<ApplicationSequence>
            {
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                },
                new ApplicationSequence
                {
                    ApplicationId = Guid.NewGuid(),
                    SequenceId = RoatpWorkflowSequenceIds.Finish,
                    Sections = new List<ApplicationSection>
                    {
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.TermsAndConditions,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    },
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }

                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.CommercialInConfidenceInformation,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = true,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        },
                        new ApplicationSection
                        {
                            SectionId = RoatpWorkflowSectionIds.Finish.TermsAndConditions,
                            QnAData = new QnAData
                            {
                                Pages = new List<Page>
                                {
                                    new Page
                                    {
                                        Active = true,
                                        Complete = false,
                                        Questions = new List<Question>()
                                    }
                                }
                            }
                        }
                    }
                }
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionPersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishAccuratePersonalDetails, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishPermissionSubmitApplication, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishCommercialInConfidence, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "No" });

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishCOA2Supporting, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.FinishCOA3Supporting, It.IsAny<string>())).ReturnsAsync(new Answer { Value = "Yes" });

            var model = new TaskListViewModel(_qnaApiClient.Object)
            {
                ApplicationSequences = applicationSequences,
                ApplicationRouteId = SupportingProviderRoute
            };

            var status = model.FinishSectionStatus(RoatpWorkflowSectionIds.Finish.SubmitApplication);
            status.Should().Be(TaskListSectionStatus.Next);
            var css = model.FinishCss(RoatpWorkflowSectionIds.Finish.SubmitApplication);
            css.Should().Be(TaskListSectionStatus.Next.ToLower());
        }

        private TaskListViewModel GetTaskListViewModelWithSectionsUpToWhosInControlCompleted()
        {
            var model = new TaskListViewModel(_qnaApiClient.Object);

            var applicationSequences = new List<ApplicationSequence>();

            var yourOrganisationSequence = new ApplicationSequence
            {
                ApplicationId = Guid.NewGuid(),
                SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                Sequential = true,
                Sections = new List<ApplicationSection>
                {
                    new ApplicationSection
                    {
                        ApplicationId = Guid.NewGuid(),
                        SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                        SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed,
                        QnAData = GetQnaDataWithCompletedSection()
                    },
                    new ApplicationSection
                    {
                        ApplicationId = Guid.NewGuid(),
                        SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                        SectionId = RoatpWorkflowSectionIds.YourOrganisation.OrganisationDetails,
                        QnAData = GetQnaDataWithCompletedSection()
                    },
                    new ApplicationSection
                    {
                        ApplicationId = Guid.NewGuid(),
                        SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                        SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhosInControl,
                        QnAData = new QnAData
                        {
                            Pages = new List<Page>
                            {
                                new Page
                                {
                                    Active = true,
                                    Complete = false
                                }
                            }
                        }
                    }
                }
            };
            applicationSequences.Add(yourOrganisationSequence);

            model.ApplicationSequences = applicationSequences;

            return model;
        }

        private QnAData GetQnaDataWithCompletedSection()
        {
            return new QnAData
            {
                Pages = new List<Page>
                {
                    new Page
                    {
                        Active = true,
                        Complete = true,
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                QuestionId = "TEST1"
                            }
                        },
                        PageOfAnswers = new List<PageOfAnswers>
                        {
                            new PageOfAnswers
                            {
                                Answers = new List<Answer>
                                {
                                    new Answer
                                    {
                                        QuestionId = "TEST1",
                                        Value = "Y"
                                    }
                                }
                            }
                        }                                 
                    }
                }
            };
        }
    }
}
