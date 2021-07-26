using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Models.Roatp;
using SFA.DAS.ApplyService.InternalApi.Services;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class RegistrationDetailsServiceTests
    {
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private Mock<IRoatpApiClient> _roatpApiClient;
        private Mock<ILogger<RegistrationDetailsService>> _logger;
        private List<ProviderType> _providerTypes;
        private List<OrganisationType> _organisationTypes;
        private RegistrationDetailsService _service;

        private const int ProviderTypeMain = 1;
        private const int ProviderTypeEmployer = 2;
        private const int ProviderTypeSupporting = 3;

        [SetUp]
        public void Before_each_test()
        {
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _roatpApiClient = new Mock<IRoatpApiClient>();

            _providerTypes = new List<ProviderType>
            {
                new ProviderType
                {
                    Id = ProviderTypeMain,
                    Type = "Main"
                },
                new ProviderType
                {
                    Id = ProviderTypeEmployer,
                    Type = "Employer"
                },
                new ProviderType
                {
                    Id = ProviderTypeSupporting,
                    Type = "Supporting"
                }
            };
            _roatpApiClient.Setup(x => x.GetProviderTypes()).ReturnsAsync(_providerTypes);

            _organisationTypes = new List<OrganisationType>
            {
                new OrganisationType
                {
                    Id = 1,
                    Type = "School"
                },
                new OrganisationType
                {
                    Id = 2,
                    Type = "General Further Education College"
                },
                new OrganisationType
                {
                    Id = 3,
                    Type = "National College"
                },
                new OrganisationType
                {
                    Id = 4,
                    Type = "Sixth Form College"
                },
                new OrganisationType
                {
                    Id = 5,
                    Type = "Further Education Institute"
                },
                new OrganisationType
                {
                    Id = 6,
                    Type = "Higher Education Institute"
                },
                new OrganisationType
                {
                    Id = 7,
                    Type = "Academy"
                },
                new OrganisationType
                {
                    Id = 8,
                    Type = "Multi-Academy Trust"
                },
                new OrganisationType
                {
                    Id = 9,
                    Type = "NHS Trust"
                },
                new OrganisationType
                {
                    Id = 10,
                    Type = "Police"
                },
                new OrganisationType
                {
                    Id = 11,
                    Type = "Fire authority"
                },
                new OrganisationType
                {
                    Id = 12,
                    Type = "Local authority"
                },
                new OrganisationType
                {
                    Id = 13,
                    Type = "Government department"
                },
                new OrganisationType
                {
                    Id = 14,
                    Type = "Non departmental public body (NDPB)"
                },
                new OrganisationType
                {
                    Id = 15,
                    Type = "Executive agency"
                },
                new OrganisationType
                {
                    Id = 16,
                    Type = "An Independent Training Provider"
                },
                new OrganisationType
                {
                    Id = 17,
                    Type = "An Apprenticeship Training Agency"
                },
                new OrganisationType
                {
                    Id = 18,
                    Type = "A Group Training Association"
                },
                new OrganisationType
                {
                    Id = 19,
                    Type = "An employer training apprentices in other organisations"
                },
                new OrganisationType
                {
                    Id = 20,
                    Type = "None of the above"
                }
            };
            _roatpApiClient.Setup(x => x.GetOrganisationTypes(It.IsAny<int>())).ReturnsAsync(_organisationTypes);

            _logger = new Mock<ILogger<RegistrationDetailsService>>();
            _service = new RegistrationDetailsService(_qnaApiClient.Object, _roatpApiClient.Object, _logger.Object);
        }

        [TestCase(ProviderTypeMain)]
        [TestCase(ProviderTypeEmployer)]
        [TestCase(ProviderTypeSupporting)]
        public void Registration_details_built_with_correct_provider_route_description(int providerTypeId)
        {
            var applicationId = Guid.NewGuid();

            var companyNumber = "12345678";
            var preamblePage = new Page
            {
                PageId = RoatpWorkflowPageIds.Preamble,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Id = Guid.NewGuid(),
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany,
                                Value = "TRUE"
                            },
                            new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompanyNumber,
                                Value = companyNumber
                            },
                            new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalName,
                                Value = "Legal name"
                            },
                            new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UKPRN,
                                Value = "10001234"
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.Preamble,
                                                          RoatpWorkflowSectionIds.Preamble, RoatpWorkflowPageIds.Preamble))
                                                         .ReturnsAsync(preamblePage);

            var organisationDetailsEmployerPage = new Page
            {
                PageId = RoatpYourOrganisationQuestionIdConstants.OrganisationTypeEmployer,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Id = Guid.NewGuid(),
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId = RoatpYourOrganisationQuestionIdConstants.OrganisationTypeEmployer,
                                Value = "An Independent Training Provider"
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.YourOrganisation,
                                                          RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation,
                                                          RoatpWorkflowPageIds.DescribeYourOrganisation.EmployerStartPage))
                                                         .ReturnsAsync(organisationDetailsEmployerPage);

            var organisationDetailsMainSupportingPage = new Page
            {
                PageId = RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Id = Guid.NewGuid(),
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId = RoatpYourOrganisationQuestionIdConstants.OrganisationTypeMainSupporting,
                                Value = "An Independent Training Provider"
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.YourOrganisation,
                                                          RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation,
                                                          RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage))
                                                         .ReturnsAsync(organisationDetailsMainSupportingPage);

            _qnaApiClient.Setup(x => x.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute, It.IsAny<string>()))
                         .ReturnsAsync(new Answer { Value = providerTypeId.ToString() });

            var details = _service.GetRegistrationDetails(applicationId).GetAwaiter().GetResult();

            details.ProviderTypeId.Should().Be(providerTypeId);
        }

        [Test]
        public void Registration_details_built_for_companies_house_verification()
        {
            var applicationId = Guid.NewGuid();

            var companyNumber = "12345678";
            var preamblePage = new Page
            {
                PageId = RoatpWorkflowPageIds.Preamble,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Id = Guid.NewGuid(),
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany,
                                Value = "TRUE"
                            },
                            new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompanyNumber,
                                Value = companyNumber
                            },
                            new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalName,
                                Value = "Legal name"
                            },
                            new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UKPRN,
                                Value = "10001234"
                            }
                        }
                    }
                }
            };


            _qnaApiClient.Setup(x => x.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.Preamble,
                                                          RoatpWorkflowSectionIds.Preamble, RoatpWorkflowPageIds.Preamble))
                                                         .ReturnsAsync(preamblePage);

            var organisationDetailsPage = new Page
            {
                PageId = RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Id = Guid.NewGuid(),
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId = RoatpYourOrganisationQuestionIdConstants.OrganisationTypeMainSupporting,
                                Value = "An Independent Training Provider"
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.YourOrganisation,
                                                          RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation,
                                                          RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage))
                                                         .ReturnsAsync(organisationDetailsPage);

            _qnaApiClient.Setup(x => x.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute, It.IsAny<string>()))
                         .ReturnsAsync(new Answer { Value = ProviderTypeMain.ToString() });

            var model = _service.GetRegistrationDetails(applicationId).GetAwaiter().GetResult();

            model.CompanyNumber.Should().Be(companyNumber);
        }

        [Test]
        public void Registration_details_built_for_charity_commission_verification()
        {
            var applicationId = Guid.NewGuid();

            var charityNumber = "12345678";

            var preamblePage = new Page
            {
                PageId = RoatpWorkflowPageIds.Preamble,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Id = Guid.NewGuid(),
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                                Value = "TRUE"
                            },
                            new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharityRegNumber,
                                Value = charityNumber
                            },
                            new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalName,
                                Value = "Legal name"
                            },
                            new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpTradingName,
                                Value = "Trading name"
                            },
                            new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UKPRN,
                                Value = "10001234"
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.Preamble,
                                                          RoatpWorkflowSectionIds.Preamble, RoatpWorkflowPageIds.Preamble))
                                                         .ReturnsAsync(preamblePage);
            var organisationDetailsPage = new Page
            {
                PageId = RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Id = Guid.NewGuid(),
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId = RoatpYourOrganisationQuestionIdConstants.OrganisationTypeMainSupporting,
                                Value = "An Independent Training Provider"
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.YourOrganisation,
                                                          RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation,
                                                          RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage))
                                                         .ReturnsAsync(organisationDetailsPage);

            _qnaApiClient.Setup(x => x.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute, It.IsAny<string>()))
                         .ReturnsAsync(new Answer { Value = ProviderTypeSupporting.ToString() });

            var model = _service.GetRegistrationDetails(applicationId).GetAwaiter().GetResult();

            model.CharityNumber.Should().Be(charityNumber);
        }

        [Test]
        public void Registration_details_built_for_non_company_or_charity()
        {
            var applicationId = Guid.NewGuid();

            var ukprn = "10002000";
            var legalName = "Legal Name";
            var tradingName = "Trading Name";

            var preamblePage = new Page
            {
                PageId = RoatpWorkflowPageIds.Preamble,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Id = Guid.NewGuid(),
                        Answers = new List<Answer>
                        {new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCompany,
                                Value = ""
                            },
                            new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpVerificationCharity,
                                Value = ""
                            },
                            new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UKPRN,
                                Value = ukprn
                            },
                            new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalName,
                                Value = legalName
                            },
                            new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpTradingName,
                                Value = tradingName
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.Preamble,
                                                          RoatpWorkflowSectionIds.Preamble, RoatpWorkflowPageIds.Preamble))
                                                         .ReturnsAsync(preamblePage);

            var organisationDetailsPage = new Page
            {
                PageId = RoatpWorkflowPageIds.DescribeYourOrganisation.EmployerStartPage,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Id = Guid.NewGuid(),
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId = RoatpYourOrganisationQuestionIdConstants.OrganisationTypeEmployer,
                                Value = "An Independent Training Provider"
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.YourOrganisation,
                                                          RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation,
                                                          RoatpWorkflowPageIds.DescribeYourOrganisation.EmployerStartPage))
                                                         .ReturnsAsync(organisationDetailsPage);
           
            _qnaApiClient.Setup(x => x.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute, It.IsAny<string>()))
                         .ReturnsAsync(new Answer { Value = ProviderTypeEmployer.ToString() });

            var model = _service.GetRegistrationDetails(applicationId).GetAwaiter().GetResult();

            model.UKPRN.Should().Be(ukprn);
            model.LegalName.Should().Be(legalName);
            model.TradingName.Should().Be(tradingName);
        }

        [TestCase("School", 1, ProviderTypeMain)]
        [TestCase("General Further Education College", 2, ProviderTypeMain)]
        [TestCase("National College", 3, ProviderTypeMain)]
        [TestCase("Sixth Form College", 4, ProviderTypeMain)]
        [TestCase("Further Education Institute", 5, ProviderTypeMain)]
        [TestCase("Higher Education Institute", 6, ProviderTypeMain)]
        [TestCase("Academy", 7, ProviderTypeMain)]
        [TestCase("Multi-Academy Trust", 8, ProviderTypeMain)]
        [TestCase("School", 1, ProviderTypeSupporting)]
        [TestCase("General Further Education College", 2, ProviderTypeSupporting)]
        [TestCase("National College", 3, ProviderTypeSupporting)]
        [TestCase("Sixth Form College", 4, ProviderTypeSupporting)]
        [TestCase("Further Education Institute", 5, ProviderTypeSupporting)]
        [TestCase("Higher Education Institute", 6, ProviderTypeSupporting)]
        [TestCase("Academy", 7, ProviderTypeSupporting)]
        [TestCase("Multi-Academy Trust", 8, ProviderTypeSupporting)]
        public void Registration_details_maps_organisation_type_for_educational_institute_main_supporting_provider(string organisationType, int organisationTypeId, int providerTypeId)
        {
            var applicationId = Guid.NewGuid();

            SetupPreamblePage();

            var organisationTypePage = new Page
            {
                PageId = RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Id = Guid.NewGuid(),
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId = RoatpYourOrganisationQuestionIdConstants.OrganisationTypeMainSupporting,
                                Value = "An educational institute"
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.YourOrganisation,
                                                          RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation,
                                                          RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage))
                                                         .ReturnsAsync(organisationTypePage);

            var educationalInstituteTypePage = new Page
            {
                PageId = RoatpWorkflowPageIds.DescribeYourOrganisation.EducationalInstituteType,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Id = Guid.NewGuid(),
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId = RoatpYourOrganisationQuestionIdConstants.EducationalInstituteType,
                                Value = organisationType
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.YourOrganisation,
                                                          RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation,
                                                          RoatpWorkflowPageIds.DescribeYourOrganisation.EducationalInstituteType))
                                                         .ReturnsAsync(educationalInstituteTypePage);

            _qnaApiClient.Setup(x => x.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute, It.IsAny<string>()))
                         .ReturnsAsync(new Answer { Value = providerTypeId.ToString() });

            var model = _service.GetRegistrationDetails(applicationId).GetAwaiter().GetResult();
            model.OrganisationTypeId.Should().Be(organisationTypeId);
        }

        [TestCase("School", 1)]
        [TestCase("General Further Education College", 2)]
        [TestCase("National College", 3)]
        [TestCase("Sixth Form College", 4)]
        [TestCase("Further Education Institute", 5)]
        [TestCase("Higher Education Institute", 6)]
        [TestCase("Academy", 7)]
        [TestCase("Multi-Academy Trust", 8)]
        public void Registration_details_maps_organisation_type_for_educational_institute_employer_provider(string organisationType, int organisationTypeId)
        {
            var applicationId = Guid.NewGuid();

            SetupPreamblePage();

            var organisationTypePage = new Page
            {
                PageId = RoatpWorkflowPageIds.DescribeYourOrganisation.EmployerStartPage,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Id = Guid.NewGuid(),
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId = RoatpYourOrganisationQuestionIdConstants.OrganisationTypeEmployer,
                                Value = "An educational institute"
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.YourOrganisation,
                                                          RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation,
                                                          RoatpWorkflowPageIds.DescribeYourOrganisation.EmployerStartPage))
                                                         .ReturnsAsync(organisationTypePage);

            var educationalInstituteTypePage = new Page
            {
                PageId = RoatpWorkflowPageIds.DescribeYourOrganisation.EducationalInstituteType,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Id = Guid.NewGuid(),
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId = RoatpYourOrganisationQuestionIdConstants.EducationalInstituteType,
                                Value = organisationType
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.YourOrganisation,
                                                          RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation,
                                                          RoatpWorkflowPageIds.DescribeYourOrganisation.EducationalInstituteType))
                                                         .ReturnsAsync(educationalInstituteTypePage);

            _qnaApiClient.Setup(x => x.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute, It.IsAny<string>()))
                         .ReturnsAsync(new Answer { Value = ProviderTypeEmployer.ToString() });

            var model = _service.GetRegistrationDetails(applicationId).GetAwaiter().GetResult();
            model.OrganisationTypeId.Should().Be(organisationTypeId);
        }

        [TestCase("NHS Trust", 9, ProviderTypeMain)]
        [TestCase("Police", 10, ProviderTypeMain)]
        [TestCase("Fire authority", 11, ProviderTypeMain)]
        [TestCase("Local authority", 12, ProviderTypeMain)]
        [TestCase("Government department", 13, ProviderTypeMain)]
        [TestCase("Non departmental public body (NDPB)", 14, ProviderTypeMain)]
        [TestCase("Executive agency", 15, ProviderTypeMain)]
        [TestCase("NHS Trust", 9, ProviderTypeSupporting)]
        [TestCase("Police", 10, ProviderTypeSupporting)]
        [TestCase("Fire authority", 11, ProviderTypeSupporting)]
        [TestCase("Local authority", 12, ProviderTypeSupporting)]
        [TestCase("Government department", 13, ProviderTypeSupporting)]
        [TestCase("Non departmental public body (NDPB)", 14, ProviderTypeSupporting)]
        [TestCase("Executive agency", 15, ProviderTypeSupporting)]
        public void Registration_details_maps_organisation_type_for_public_body_main_supporting_provider(string organisationType, int organisationTypeId, int providerTypeId)
        {
            var applicationId = Guid.NewGuid();

            SetupPreamblePage();

            var organisationTypePage = new Page
            {
                PageId = RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Id = Guid.NewGuid(),
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId = RoatpYourOrganisationQuestionIdConstants.OrganisationTypeMainSupporting,
                                Value = "A public body"
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.YourOrganisation,
                                                          RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation, 
                                                          RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage))
                                                         .ReturnsAsync(organisationTypePage);

            var publicBodyTypePage = new Page
            {
                PageId = RoatpWorkflowPageIds.DescribeYourOrganisation.PublicBodyType,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Id = Guid.NewGuid(),
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId = RoatpYourOrganisationQuestionIdConstants.PublicBodyType,
                                Value = organisationType
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.YourOrganisation,
                                                          RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation,
                                                          RoatpWorkflowPageIds.DescribeYourOrganisation.PublicBodyType))
                                                         .ReturnsAsync(publicBodyTypePage);

            _qnaApiClient.Setup(x => x.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute, It.IsAny<string>()))
                         .ReturnsAsync(new Answer { Value = providerTypeId.ToString() });

            var model = _service.GetRegistrationDetails(applicationId).GetAwaiter().GetResult();
            model.OrganisationTypeId.Should().Be(organisationTypeId);
        }

        [TestCase("NHS Trust", 9)]
        [TestCase("Police", 10)]
        [TestCase("Fire authority", 11)]
        [TestCase("Local authority", 12)]
        [TestCase("Government department", 13)]
        [TestCase("Non departmental public body (NDPB)", 14)]
        [TestCase("Executive agency", 15)]
        public void Registration_details_maps_organisation_type_for_public_body_employer_provider(string organisationType, int organisationTypeId)
        {
            var applicationId = Guid.NewGuid();

            SetupPreamblePage();

            var organisationTypePage = new Page
            {
                PageId = RoatpWorkflowPageIds.DescribeYourOrganisation.EmployerStartPage,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Id = Guid.NewGuid(),
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId = RoatpYourOrganisationQuestionIdConstants.OrganisationTypeEmployer,
                                Value = "A public body"
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(applicationId, RoatpWorkflowSequenceIds.YourOrganisation,
                                                          RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation, 
                                                          RoatpWorkflowPageIds.DescribeYourOrganisation.EmployerStartPage))
                                                         .ReturnsAsync(organisationTypePage);

            var publicBodyTypePage = new Page
            {
                PageId = RoatpWorkflowPageIds.DescribeYourOrganisation.PublicBodyType,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Id = Guid.NewGuid(),
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId = RoatpYourOrganisationQuestionIdConstants.PublicBodyType,
                                Value = organisationType
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.YourOrganisation,
                                                          RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation, 
                                                          RoatpWorkflowPageIds.DescribeYourOrganisation.PublicBodyType))
                                                         .ReturnsAsync(publicBodyTypePage);

            _qnaApiClient.Setup(x => x.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute, It.IsAny<string>()))
                         .ReturnsAsync(new Answer { Value = ProviderTypeEmployer.ToString() });

            var model = _service.GetRegistrationDetails(applicationId).GetAwaiter().GetResult();
            model.OrganisationTypeId.Should().Be(organisationTypeId);
        }

        [TestCase("An Independent Training Provider", 16, ProviderTypeMain)]
        [TestCase("An Apprenticeship Training Agency", 17, ProviderTypeMain)]
        [TestCase("A Group Training Association", 18, ProviderTypeMain)]
        [TestCase("An employer training apprentices in other organisations", 19, ProviderTypeMain)]
        [TestCase("An Independent Training Provider", 16, ProviderTypeSupporting)]
        [TestCase("An Apprenticeship Training Agency", 17, ProviderTypeSupporting)]
        [TestCase("A Group Training Association", 18, ProviderTypeSupporting)]
        [TestCase("An employer training apprentices in other organisations", 19, ProviderTypeSupporting)]
        public void Registration_details_maps_organisation_type_for_other_organisation_types(string organisationType, int organisationTypeId, int providerTypeId)
        {
            var applicationId = Guid.NewGuid();

            SetupPreamblePage();

            var organisationTypePage = new Page
            {
                PageId = RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Id = Guid.NewGuid(),
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId = RoatpYourOrganisationQuestionIdConstants.OrganisationTypeMainSupporting,
                                Value = organisationType
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x => x.GetPageBySectionNo(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.YourOrganisation,
                                                          RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation, 
                                                          RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage))
                                                         .ReturnsAsync(organisationTypePage);
            
            _qnaApiClient.Setup(x => x.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute, It.IsAny<string>()))
                        .ReturnsAsync(new Answer { Value = providerTypeId.ToString() });

            var model = _service.GetRegistrationDetails(applicationId).GetAwaiter().GetResult();
            model.OrganisationTypeId.Should().Be(organisationTypeId);
        }

        private void SetupPreamblePage()
        {
            var preamblePage = new Page
            {
                PageId = RoatpWorkflowPageIds.Preamble,
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Id = Guid.NewGuid(),
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UkrlpLegalName,
                                Value = "Legal name"
                            },
                            new Answer
                            {
                                QuestionId = RoatpPreambleQuestionIdConstants.UKPRN,
                                Value = "10001234"
                            }
                        }
                    }
                }
            };
                        
            _qnaApiClient.Setup(x => x.GetPageBySectionNo(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.Preamble,
                                                          RoatpWorkflowSectionIds.Preamble, RoatpWorkflowPageIds.Preamble))
                                                         .ReturnsAsync(preamblePage);
        }
    }
}
