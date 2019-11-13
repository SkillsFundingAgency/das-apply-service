
using SFA.DAS.ApplyService.Web.Services;

namespace SFA.DAS.ApplyService.Web.UnitTests
{
    using System;
    using Domain.Entities;
    using NUnit.Framework;
    using System.Collections.Generic;
    using Application.Apply.Roatp;
    using Domain.Apply;
    using FluentAssertions;
    using ViewModels.Roatp;

    [TestFixture]
    public class TaskListViewModelTests
    {
        private Guid _applicationId;
        private List<ApplicationSequence> _applicationSequences;
        private ApplicationSequence _yourApplicationSequence;
        private ApplicationSection _providerRouteSection;
        private ApplicationSection _whatYouNeedSection;
        private RoatpTaskListWorkflowService _roatpTaskListWorkflowService;
        
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
                Sections = new List<ApplicationSection>()
            };
            _roatpTaskListWorkflowService = new RoatpTaskListWorkflowService();
            _providerRouteSection = new ApplicationSection
            {
                SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.ProviderRoute
            };
            _whatYouNeedSection = new ApplicationSection
            {
                SequenceId = RoatpWorkflowSequenceIds.YourOrganisation,
                SectionId = RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed
            };

            _yourApplicationSequence.Sections.Add(_providerRouteSection);
            _yourApplicationSequence.Sections.Add(_whatYouNeedSection);
            _applicationSequences.Add(_yourApplicationSequence);
        }

        [Test]
        public void Task_list_shows_next_for_provider_route_if_not_complete()
        {
            // This should be prefilled in the pre-amble but test in case the sequencing changes

            _providerRouteSection.QnAData = new QnAData
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

            _whatYouNeedSection.QnAData = new QnAData
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

            var model = new TaskListViewModel(_roatpTaskListWorkflowService)
            {
                ApplicationId = _applicationId,
                ApplicationSequences = _applicationSequences,
                UKPRN = "10001234",
                OrganisationName = "Org Name"
            };

            model.SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ProviderRoute, true).Should().Be("Next");
            model.SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed, true).ToLower().Should().Be("");
        }

        [Test]
        public void Task_list_shows_next_for_your_organisation_what_you_need_if_not_complete()
        {
            _providerRouteSection.QnAData = new QnAData
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
                        }
                    }
                }
            };
            _providerRouteSection.SectionCompleted = true;

            _whatYouNeedSection.QnAData = new QnAData
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

            var model = new TaskListViewModel(_roatpTaskListWorkflowService)
            {
                ApplicationId = _applicationId,
                ApplicationSequences = _applicationSequences,
                UKPRN = "10001234",
                OrganisationName = "Org Name"
            };

            model.SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ProviderRoute, true).Should().Be("Completed");
            model.SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed, true).Should().Be("Next");
        }

        [Test]
        public void Task_list_shows_completed_your_organisation_section()
        {
            _providerRouteSection.QnAData = new QnAData
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
                        }
                    }
                }
            };
            _providerRouteSection.SectionCompleted = true;

            _whatYouNeedSection.QnAData = new QnAData
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
                        }
                    }
                }
            };
            _whatYouNeedSection.SectionCompleted = true;

            var model = new TaskListViewModel(_roatpTaskListWorkflowService)
            {
                ApplicationId = _applicationId,
                ApplicationSequences = _applicationSequences,
                UKPRN = "10001234",
                OrganisationName = "Org Name"
            };

            model.SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ProviderRoute, true).Should().Be("Completed");
            model.SectionStatus(RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed, true).Should().Be("Completed");
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

            var model = new TaskListViewModel(_roatpTaskListWorkflowService)
            {
                ApplicationId = _applicationId,
                ApplicationSequences = _applicationSequences,
                UKPRN = "10001234",
                OrganisationName = "Org Name"
            };

            model.SectionStatus(RoatpWorkflowSequenceIds.CriminalComplianceChecks,
                RoatpWorkflowSectionIds.CriminalComplianceChecks.WhatYouWillNeed, false).Should().Be("Completed");
            model.SectionStatus(RoatpWorkflowSequenceIds.CriminalComplianceChecks,
                RoatpWorkflowSectionIds.CriminalComplianceChecks.ChecksOnYourOrganisation, false).Should().Be("In Progress");
            model.SectionStatus(RoatpWorkflowSequenceIds.CriminalComplianceChecks,
                RoatpWorkflowSectionIds.CriminalComplianceChecks.CheckOnWhosInControl, false).Should().Be("");
        }
    }
}
