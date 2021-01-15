using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Application.Apply.Start;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Domain.Ukrlp;
using SFA.DAS.ApplyService.EmailService;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Controllers;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Infrastructure.Interfaces;
using SFA.DAS.ApplyService.Web.Infrastructure.Validations;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Web.Orchestrators;

namespace SFA.DAS.ApplyService.Web.UnitTests.Controllers
{
    [TestFixture]
    public class RoatpApplicationControllerTests
    {
        private RoatpApplicationController _controller;
        private Mock<IApplicationApiClient> _apiClient;
        private Mock<ILogger<RoatpApplicationController>> _logger;
        private Mock<IUsersApiClient> _usersApiClient;
        private Mock<ISessionService> _sessionService;
        private Mock<IConfigurationService> _configService;
        private Mock<IUserService> _userService;
        private Mock<IQnaApiClient> _qnaApiClient;
        private Mock<IQuestionPropertyTokeniser> _questionPropertyTokeniser;
        private Mock<IPageNavigationTrackingService> _pageNavigationTrackingService;
        private Mock<IOptions<List<QnaPageOverrideConfiguration>>> _pageOverrideConfiguration;
        private Mock<IOptions<List<QnaLinksConfiguration>>> _qnaLinks;
        private Mock<ICustomValidatorFactory> _customValidatorFactory;
        private Mock<IRoatpApiClient> _roatpApiClient;
        private Mock<ISubmitApplicationConfirmationEmailService> _submitApplicationEmailService;
        private Mock<ITabularDataRepository> _tabularDataRepository;
        private Mock<IPagesWithSectionsFlowService> _pagesWithSectionsFlowService;
        private Mock<IRoatpTaskListWorkflowService> _roatpTaskListWorkflowService;
        private Mock<IRoatpOrganisationVerificationService> _roatpOrganisationVerificationService;
        private Mock<ITaskListOrchestrator> _taskListOrchestrator;
        private Mock<IUkrlpApiClient> _ukrlpApiClient;
        private Mock<IApplicationApiClient> _applicationApiClient;

        [SetUp]
        public void Before_each_test()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("Email", "test@test.com"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            _apiClient = new Mock<IApplicationApiClient>();
            _logger = new Mock<ILogger<RoatpApplicationController>>();
            _usersApiClient = new Mock<IUsersApiClient>();
            _sessionService = new Mock<ISessionService>();
            _configService = new Mock<IConfigurationService>();
            _userService = new Mock<IUserService>();
            _qnaApiClient = new Mock<IQnaApiClient>();
            _pagesWithSectionsFlowService = new Mock<IPagesWithSectionsFlowService>();

            _questionPropertyTokeniser = new Mock<IQuestionPropertyTokeniser>();
            _pageNavigationTrackingService = new Mock<IPageNavigationTrackingService>();
            _pageOverrideConfiguration = new Mock<IOptions<List<QnaPageOverrideConfiguration>>>();
            _qnaLinks = new Mock<IOptions<List<QnaLinksConfiguration>>>();
            _customValidatorFactory = new Mock<ICustomValidatorFactory>();
            _roatpApiClient = new Mock<IRoatpApiClient>();
            _submitApplicationEmailService = new Mock<ISubmitApplicationConfirmationEmailService>();
            _tabularDataRepository = new Mock<ITabularDataRepository>();
            _roatpTaskListWorkflowService = new Mock<IRoatpTaskListWorkflowService>();
            _roatpOrganisationVerificationService = new Mock<IRoatpOrganisationVerificationService>();
            _taskListOrchestrator = new Mock<ITaskListOrchestrator>();
            _ukrlpApiClient = new Mock<IUkrlpApiClient>();
            _applicationApiClient = new Mock<IApplicationApiClient>();

            _controller = new RoatpApplicationController(_apiClient.Object, _logger.Object, _sessionService.Object, _configService.Object,
                                                         _userService.Object, _usersApiClient.Object, _qnaApiClient.Object, 
                                                          _pagesWithSectionsFlowService.Object,
                                                         _questionPropertyTokeniser.Object, _pageOverrideConfiguration.Object,
                                                         _pageNavigationTrackingService.Object, _qnaLinks.Object, _customValidatorFactory.Object,
                                                         _roatpApiClient.Object,
                                                         _submitApplicationEmailService.Object, _tabularDataRepository.Object,
                                                         _roatpTaskListWorkflowService.Object, _roatpOrganisationVerificationService.Object, _taskListOrchestrator.Object,
                                                         _ukrlpApiClient.Object, _applicationApiClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = user }
                }
            };

            _userService.Setup(x => x.ValidateUser(It.IsAny<string>())).ReturnsAsync(true);
            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                GivenNames = "Test",
                FamilyName = "User"
            };
            _usersApiClient.Setup(x => x.GetUserBySignInId(It.IsAny<string>())).ReturnsAsync(contact);
        }

        [Test]
        public async Task Applications_starts_a_new_application_if_no_applications_for_that_user()
        {
            _apiClient.Setup(x => x.GetApplications(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(new List<Domain.Entities.Apply>());

            var applicationDetails = new ApplicationDetails
            {
                UKPRN = 10002000,
                UkrlpLookupDetails = new ProviderDetails
                {
                    ProviderName = "Provider name",
                    ContactDetails = new List<ProviderContact>
                    {
                        new ProviderContact
                        {
                            ContactType = "L",
                            ContactAddress = new ContactAddress
                            {
                                Address1 = "Address line 1",
                                PostCode = "PS1 1ST"
                            }
                        }
                    },
                    VerificationDetails = new List<VerificationDetails>
                    {
                        new VerificationDetails
                        {
                            VerificationAuthority = VerificationAuthorities.SoleTraderPartnershipAuthority,
                            PrimaryVerificationSource = true
                        }
                    }
                },
                ApplicationRoute = new ApplicationRoute
                {
                    Id = 1
                },
                RoatpRegisterStatus = new OrganisationRegisterStatus
                {
                    ProviderTypeId = 1
                }
            };

            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            var applicationId = Guid.NewGuid();
            var qnaResponse = new StartQnaApplicationResponse
            {
                ApplicationId = applicationId
            };

            _qnaApiClient.Setup(x => x.StartApplication(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(qnaResponse).Verifiable();

            _apiClient.Setup(x => x.StartApplication(It.IsAny<StartApplicationRequest>())).ReturnsAsync(applicationId).Verifiable();

            await _controller.Applications();

            _qnaApiClient.VerifyAll();
            _apiClient.VerifyAll();
        }

        [Test]
        public async Task Applications_starts_a_new_application_if_no_active_applications_for_that_user()
        {
            var applications = new List<Domain.Entities.Apply>
            {
                new Apply { ApplicationStatus = ApplicationStatus.Cancelled },
                new Apply { ApplicationStatus = ApplicationStatus.Withdrawn },
                new Apply { ApplicationStatus = ApplicationStatus.Removed }
            };

            _apiClient.Setup(x => x.GetApplications(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(applications);

            var applicationDetails = new ApplicationDetails
            {
                UKPRN = 10002000,
                UkrlpLookupDetails = new ProviderDetails
                {
                    ProviderName = "Provider name",
                    ContactDetails = new List<ProviderContact>
                    {
                        new ProviderContact
                        {
                            ContactType = "L",
                            ContactAddress = new ContactAddress
                            {
                                Address1 = "Address line 1",
                                PostCode = "PS1 1ST"
                            }
                        }
                    },
                    VerificationDetails = new List<VerificationDetails>
                    {
                        new VerificationDetails
                        {
                            VerificationAuthority = VerificationAuthorities.SoleTraderPartnershipAuthority,
                            PrimaryVerificationSource = true
                        }
                    }
                },
                ApplicationRoute = new ApplicationRoute
                {
                    Id = 1
                }
            };

            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            var applicationId = Guid.NewGuid();
            var qnaResponse = new StartQnaApplicationResponse
            {
                ApplicationId = applicationId
            };

            _qnaApiClient.Setup(x => x.StartApplication(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(qnaResponse).Verifiable();

            _apiClient.Setup(x => x.StartApplication(It.IsAny<StartApplicationRequest>())).ReturnsAsync(applicationId).Verifiable();

            await _controller.Applications();

            _qnaApiClient.VerifyAll();
            _apiClient.VerifyAll();
        }

        [Test]
        public void Applications_shows_task_list_if_an_application_in_progress()
        {
            var inProgressApp = new Domain.Entities.Apply
            {
                ApplicationStatus = ApplicationStatus.InProgress
            };
            var applications = new List<Domain.Entities.Apply>
            {
                inProgressApp
            };

            _apiClient.Setup(x => x.GetApplications(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(applications);

            var result = _controller.Applications().GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("TaskList");
        }

        [Test]
        public void Applications_shows_confirmation_page_if_application_submitted()
        {
            var submittedApp = new Domain.Entities.Apply
            {
                ApplicationStatus = ApplicationStatus.Submitted
            };
            var applications = new List<Domain.Entities.Apply>
            {
                submittedApp
            };

            _apiClient.Setup(x => x.GetApplications(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(applications);

            var result = _controller.Applications().GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ApplicationSubmitted");
        }

        [Test]
        public void Applications_shows_confirmation_page_if_application_Gateway_Assessed()
        {
            var submittedApp = new Domain.Entities.Apply
            {
                ApplicationStatus = ApplicationStatus.GatewayAssessed
            };
            var applications = new List<Domain.Entities.Apply>
            {
                submittedApp
            };

            _apiClient.Setup(x => x.GetApplications(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(applications);

            var result = _controller.Applications().GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ApplicationSubmitted");
        }


        [Test]
        public void Applications_shows_confirmation_page_if_application_new()
        {
            var submittedApp = new Domain.Entities.Apply
            {
                ApplicationStatus = ApplicationStatus.New
            };
            var applications = new List<Domain.Entities.Apply>
            {
                submittedApp
            };

            _apiClient.Setup(x => x.GetApplications(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(applications);

            var result = _controller.Applications().GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("TaskList");
        }

        [Test]
        public void Applications_shows_confirmation_page_if_application_resubmitted()
        {
            var submittedApp = new Domain.Entities.Apply
            {
                ApplicationStatus = ApplicationStatus.Resubmitted
            };
            var applications = new List<Domain.Entities.Apply>
            {
                submittedApp
            };

            _apiClient.Setup(x => x.GetApplications(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(applications);

            var result = _controller.Applications().GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ApplicationSubmitted");
        }


        [Test]
        public void Applications_shows_two_in_twelve_months_page_if_application_cancelled()
        {
            var submittedApp = new Domain.Entities.Apply
            {
                ApplicationStatus = ApplicationStatus.Cancelled
            };
            var applications = new List<Domain.Entities.Apply>
            {
                submittedApp
            };

            _apiClient.Setup(x => x.GetApplications(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(applications);

            var result = _controller.Applications().GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("TwoInTwelveMonths");
            redirectResult.ControllerName.Should().Be("RoatpApplicationPreamble");
        }

        [Test]
        public async Task Applications_shows_two_in_twelve_months_page_if_no_active_applications()
        {
            _apiClient.Setup(x => x.GetApplications(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(new List<Apply>());

            var result = await _controller.Applications();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("TwoInTwelveMonths");
            redirectResult.ControllerName.Should().Be("RoatpApplicationPreamble");
        }

        [Test]
        public void Submit_application_presents_confirmation_page_with_legal_name_and_emailaddress()
        {
            var organisationNameAnswer = new Answer
            {
                QuestionId = "ORG-1",
                Value = "My organisation"
            };

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpLegalName, It.IsAny<string>())).ReturnsAsync(organisationNameAnswer);

            var result = _controller.SubmitApplication(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as SubmitApplicationViewModel;
            model.Should().NotBeNull();
            model.OrganisationName.Should().Be(organisationNameAnswer.Value);
            model.EmailAddress.Should().NotBeNull();
        }

        [Test]
        public async Task Confirm_submit_application_updates_application_status_and_sends_confirmation_email_if_they_have_confirmed_details_are_correct()
        {
            var application = new Apply
            {
                ApplicationId = Guid.NewGuid(),
                ApplicationStatus = ApplicationStatus.InProgress,
                ApplyData = new ApplyData
                {
                    ApplyDetails = new ApplyDetails(),
                    Sequences = new List<ApplySequence>()
                }
            };

            var model = new SubmitApplicationViewModel
            {
                ApplicationId = application.ApplicationId,
                ConfirmSubmitApplication = true,
                ConfirmFurtherInfoSubmitApplication = true,
                ConfirmFurtherCommunicationSubmitApplication = true,
                ErrorMessages = new List<ValidationErrorDetail>()
            };

            var organisationDetails = new Organisation
            {
                OrganisationUkprn = 10003000,
                Name = "Organisation Name",
                OrganisationDetails = new OrganisationDetails
                {
                    TradingName = "Trading name"
                }
            };

            _apiClient.Setup(x => x.GetOrganisationByUserId(It.IsAny<Guid>())).ReturnsAsync(organisationDetails);

            _apiClient.Setup(x => x.GetApplicationByUserId(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(application);

            _qnaApiClient.Setup(x => x.GetSections(It.IsAny<Guid>())).ReturnsAsync(() => new List<ApplicationSection>
            {
                new ApplicationSection {SectionId = 2, SequenceId = 2,}
            });

            _qnaApiClient.Setup(x => x.GetApplicationData(It.IsAny<Guid>()))
                .ReturnsAsync(() => new JObject());

            _qnaApiClient.Setup(x => x.GetPage(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(() => new Page
                {
                    PageOfAnswers = new List<PageOfAnswers>
                    {
                        new PageOfAnswers{Answers = new List<Answer>()}
                    }
                });

            var providerRouteAnswer = new Answer
            {
                QuestionId = "YO-1",
                Value = ApplicationRoute.MainProviderApplicationRoute.ToString()
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.ProviderRoute, It.IsAny<string>())).ReturnsAsync(providerRouteAnswer);

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(application);

            _apiClient.Setup(x => x.SubmitApplication(It.IsAny<Application.Apply.Submit.SubmitApplicationRequest>())).ReturnsAsync(true);

            _submitApplicationEmailService.Setup(x => x.SendGetHelpWithQuestionEmail(It.IsAny<ApplicationSubmitConfirmation>())).Returns(Task.FromResult(true));

            var result = await _controller.ConfirmSubmitApplication(model);

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ApplicationSubmitted");

            _submitApplicationEmailService.VerifyAll();
        }

        [Test]
        public async Task Confirm_submit_application_submit_application_request_includes_financial_data()
        {
            var application = new Apply
            {
                ApplicationId = Guid.NewGuid(),
                ApplicationStatus = ApplicationStatus.InProgress,
                ApplyData = new ApplyData
                {
                    ApplyDetails = new ApplyDetails(),
                    Sequences = new List<ApplySequence>()
                }
            };

            var model = new SubmitApplicationViewModel
            {
                ApplicationId = application.ApplicationId,
                ConfirmSubmitApplication = true,
                ConfirmFurtherInfoSubmitApplication = true,
                ConfirmFurtherCommunicationSubmitApplication = true,
                ErrorMessages = new List<ValidationErrorDetail>()
            };

            var organisationDetails = new Organisation
            {
                OrganisationUkprn = 10003000,
                Name = "Organisation Name",
                OrganisationDetails = new OrganisationDetails
                {
                    TradingName = "Trading name"
                }
            };

            _apiClient.Setup(x => x.GetOrganisationByUserId(It.IsAny<Guid>())).ReturnsAsync(organisationDetails);

            _apiClient.Setup(x => x.GetApplicationByUserId(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(application);

            _qnaApiClient.Setup(x => x.GetSections(It.IsAny<Guid>())).ReturnsAsync(() => new List<ApplicationSection>
            {
                new ApplicationSection {SectionId = 2, SequenceId = 2}
            });

            _qnaApiClient.Setup(x => x.GetApplicationData(It.IsAny<Guid>()))
                .ReturnsAsync(() => new JObject
                {
                    new JProperty(RoatpWorkflowQuestionTags.Turnover, "1"),
                    new JProperty(RoatpWorkflowQuestionTags.Depreciation, "2"),
                    new JProperty(RoatpWorkflowQuestionTags.ProfitLoss, "3"),
                    new JProperty(RoatpWorkflowQuestionTags.Dividends, "4"),
                    new JProperty(RoatpWorkflowQuestionTags.Assets, "5"),
                    new JProperty(RoatpWorkflowQuestionTags.Liabilities, "6"),
                    new JProperty(RoatpWorkflowQuestionTags.Borrowings, "7"),
                    new JProperty(RoatpWorkflowQuestionTags.ShareholderFunds, "8"),
                    new JProperty(RoatpWorkflowQuestionTags.IntangibleAssets, "9")
                });

            var providerRouteAnswer = new Answer
            {
                QuestionId = "YO-1",
                Value = ApplicationRoute.MainProviderApplicationRoute.ToString()
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.ProviderRoute, It.IsAny<string>())).ReturnsAsync(providerRouteAnswer);

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(application);

            _apiClient.Setup(x => x.SubmitApplication(It.IsAny<Application.Apply.Submit.SubmitApplicationRequest>())).ReturnsAsync(true);

            _submitApplicationEmailService.Setup(x => x.SendGetHelpWithQuestionEmail(It.IsAny<ApplicationSubmitConfirmation>())).Returns(Task.FromResult(true));

            await _controller.ConfirmSubmitApplication(model);

            _apiClient.Verify(x =>
                x.SubmitApplication(It.Is<SubmitApplicationRequest>(r =>
                    r.FinancialData.TurnOver == 1
                    && r.FinancialData.Depreciation == 2
                    && r.FinancialData.ProfitLoss == 3
                    && r.FinancialData.Dividends == 4
                    && r.FinancialData.Assets == 5
                    && r.FinancialData.Liabilities == 6
                    && r.FinancialData.Borrowings == 7
                    && r.FinancialData.ShareholderFunds == 8
                    && r.FinancialData.IntangibleAssets == 9
                    )));
        }


        [Test]
        public async Task Confirm_submit_application_submit_application_request_includes_null_financial_data_if_financial_page_is_not_active()
        {
            var application = new Apply
            {
                ApplicationId = Guid.NewGuid(),
                ApplicationStatus = ApplicationStatus.InProgress,
                ApplyData = new ApplyData
                {
                    ApplyDetails = new ApplyDetails(),
                    Sequences = new List<ApplySequence>()
                }
            };

            var model = new SubmitApplicationViewModel
            {
                ApplicationId = application.ApplicationId,
                ConfirmSubmitApplication = true,
                ConfirmFurtherInfoSubmitApplication = true,
                ConfirmFurtherCommunicationSubmitApplication = true,
                ErrorMessages = new List<ValidationErrorDetail>()
            };

            var organisationDetails = new Organisation
            {
                OrganisationUkprn = 10003000,
                Name = "Organisation Name",
                OrganisationDetails = new OrganisationDetails
                {
                    TradingName = "Trading name"
                }
            };

            _apiClient.Setup(x => x.GetOrganisationByUserId(It.IsAny<Guid>())).ReturnsAsync(organisationDetails);

            _apiClient.Setup(x => x.GetApplicationByUserId(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(application);

            _qnaApiClient.Setup(x => x.GetSections(It.IsAny<Guid>())).ReturnsAsync(() => new List<ApplicationSection>
            {
                new ApplicationSection {SectionId = 2, SequenceId = 2}
            });

            var applicationData = new JObject();
            _qnaApiClient.Setup(x => x.GetApplicationData(It.IsAny<Guid>()))
                .ReturnsAsync(() => applicationData);

            _qnaApiClient.Setup(x => x.GetPage(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(() => new Page
                {
                    Active = false,
                    PageOfAnswers = new List<PageOfAnswers>
                    {
                        new PageOfAnswers{Answers = new List<Answer>
                        {
                            new Answer{ QuestionId = "FH-140", Value = "1"},
                            new Answer{ QuestionId = "FH-150", Value = "2"},
                            new Answer{ QuestionId = "FH-160", Value = "3"},
                            new Answer{ QuestionId = "FH-170", Value = "4"},
                            new Answer{ QuestionId = "FH-180", Value = "5"},
                            new Answer{ QuestionId = "FH-190", Value = "6"},
                            new Answer{ QuestionId = "FH-200", Value = "7"},
                            new Answer{ QuestionId = "FH-210", Value = "8"},
                            new Answer{ QuestionId = "FH-220", Value = "9"}
                        }}
                    }
                });

            var providerRouteAnswer = new Answer
            {
                QuestionId = "YO-1",
                Value = ApplicationRoute.MainProviderApplicationRoute.ToString()
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.ProviderRoute, It.IsAny<string>())).ReturnsAsync(providerRouteAnswer);

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(application);

            _apiClient.Setup(x => x.SubmitApplication(It.IsAny<Application.Apply.Submit.SubmitApplicationRequest>())).ReturnsAsync(true);

            _submitApplicationEmailService.Setup(x => x.SendGetHelpWithQuestionEmail(It.IsAny<ApplicationSubmitConfirmation>())).Returns(Task.FromResult(true));

            await _controller.ConfirmSubmitApplication(model);

            _apiClient.Verify(x =>
                x.SubmitApplication(It.Is<SubmitApplicationRequest>(r =>
                    r.FinancialData.TurnOver == null
                    && r.FinancialData.Depreciation == null
                    && r.FinancialData.ProfitLoss == null
                    && r.FinancialData.Dividends == null
                    && r.FinancialData.Assets == null
                    && r.FinancialData.Liabilities == null
                    && r.FinancialData.Borrowings == null
                    && r.FinancialData.ShareholderFunds == null
                    && r.FinancialData.IntangibleAssets == null
                    )));
        }


        [Test]
        public async Task Confirm_submit_application_submit_application_request_includes_organisation_type()
        {
            var application = new Apply
            {
                ApplicationId = Guid.NewGuid(),
                ApplicationStatus = ApplicationStatus.InProgress,
                ApplyData = new ApplyData
                {
                    ApplyDetails = new ApplyDetails(),
                    Sequences = new List<ApplySequence>()
                }
            };

            var model = new SubmitApplicationViewModel
            {
                ApplicationId = application.ApplicationId,
                ConfirmSubmitApplication = true,
                ConfirmFurtherInfoSubmitApplication = true,
                ConfirmFurtherCommunicationSubmitApplication = true,
                ErrorMessages = new List<ValidationErrorDetail>()
            };

            var organisationDetails = new Organisation
            {
                OrganisationUkprn = 10003000,
                Name = "Organisation Name",
                OrganisationDetails = new OrganisationDetails
                {
                    TradingName = "Trading name"
                }
            };

            _apiClient.Setup(x => x.GetOrganisationByUserId(It.IsAny<Guid>())).ReturnsAsync(organisationDetails);

            _apiClient.Setup(x => x.GetApplicationByUserId(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(application);

            _qnaApiClient.Setup(x => x.GetSections(It.IsAny<Guid>())).ReturnsAsync(() => new List<ApplicationSection>
            {
                new ApplicationSection {SectionId = 2, SequenceId = 2}
            });

            var applicationData = new JObject {{ "OrganisationPublicBody", "test"}};
            _qnaApiClient.Setup(x => x.GetApplicationData(It.IsAny<Guid>()))
                .ReturnsAsync(() => applicationData);

            _qnaApiClient.Setup(x => x.GetPage(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(() => new Page
                {
                    Active = false,
                    PageOfAnswers = new List<PageOfAnswers>
                    {
                        new PageOfAnswers{Answers = new List<Answer>
                        {
                            new Answer{ QuestionId = "FH-140", Value = "1"},
                            new Answer{ QuestionId = "FH-150", Value = "2"},
                            new Answer{ QuestionId = "FH-160", Value = "3"},
                            new Answer{ QuestionId = "FH-170", Value = "4"},
                            new Answer{ QuestionId = "FH-180", Value = "5"},
                            new Answer{ QuestionId = "FH-190", Value = "6"},
                            new Answer{ QuestionId = "FH-200", Value = "7"},
                            new Answer{ QuestionId = "FH-210", Value = "8"},
                            new Answer{ QuestionId = "FH-220", Value = "9"}
                        }}
                    }
                });

            var providerRouteAnswer = new Answer
            {
                QuestionId = "YO-1",
                Value = ApplicationRoute.MainProviderApplicationRoute.ToString()
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.ProviderRoute, It.IsAny<string>())).ReturnsAsync(providerRouteAnswer);

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(application);

            _apiClient.Setup(x => x.SubmitApplication(It.IsAny<Application.Apply.Submit.SubmitApplicationRequest>())).ReturnsAsync(true);

            _submitApplicationEmailService.Setup(x => x.SendGetHelpWithQuestionEmail(It.IsAny<ApplicationSubmitConfirmation>())).Returns(Task.FromResult(true));

            await _controller.ConfirmSubmitApplication(model);

            _apiClient.Verify(x =>
                x.SubmitApplication(It.Is<SubmitApplicationRequest>(r => r.OrganisationType == "test")));
        }


        [Test]
        public async Task TaskList_shows_tasklist_view_for_application()
        {
            var applicationId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            _userService.Setup(x => x.GetSignInId()).ReturnsAsync(() => userId);

            var inProgressApp = new Domain.Entities.Apply
            {
                ApplicationStatus = ApplicationStatus.InProgress,
                ApplyData = new ApplyData()
            };

            _apiClient.Setup(x => x.GetApplicationByUserId(applicationId, userId)).ReturnsAsync(() => inProgressApp);


            _taskListOrchestrator.Setup(x => x.GetTaskListViewModel(applicationId, userId))
                .ReturnsAsync(() => new TaskListViewModel());

            var result = await _controller.TaskList(applicationId);

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult) result;
            Assert.AreEqual("~/Views/Roatp/TaskList.cshtml", viewResult.ViewName);

        }
    }
}
