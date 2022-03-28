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
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Domain.Ukrlp;
using SFA.DAS.ApplyService.EmailService.Interfaces;
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
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Domain.Apply.AllowedProviders;
using SFA.DAS.ApplyService.Web.Orchestrators;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.UnitTests.Controllers
{
    [TestFixture]
    public class RoatpApplicationControllerTests
    {
        private const string USER_GIVEN_NAME = "Test";
        private const string USER_FAMILY_NAME = "User";
        private const string USER_EMAIL_ADDRESS = "Test.User@test.com";
        private readonly Guid USER_USERID = Guid.NewGuid();
        private readonly Guid USER_SIGNINID = Guid.NewGuid();

        private RoatpApplicationController _controller;
        private Mock<IApplicationApiClient> _apiClient;
        private Mock<ILogger<RoatpApplicationController>> _logger;
        private Mock<IUsersApiClient> _usersApiClient;
        private Mock<ISessionService> _sessionService;
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
        private Mock<IReapplicationCheckService> _reapplicationCheckService;
        private Mock<IResetCompleteFlagService> _resetQnaDetailsService;

        [SetUp]
        public void Before_each_test()
        {
            var signInId = Guid.NewGuid();
            var givenNames = "Test";
            var familyName = "User";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, $"{USER_GIVEN_NAME} {USER_FAMILY_NAME}"),
                new Claim("sub", USER_SIGNINID.ToString()),
                new Claim("given_name", USER_GIVEN_NAME),
                new Claim("family_name", USER_FAMILY_NAME),
                new Claim("Email", USER_EMAIL_ADDRESS),
            }, "mock"));

            _apiClient = new Mock<IApplicationApiClient>();
            _logger = new Mock<ILogger<RoatpApplicationController>>();
            _usersApiClient = new Mock<IUsersApiClient>();
            _sessionService = new Mock<ISessionService>();
            _qnaApiClient = new Mock<IQnaApiClient>();
            _pagesWithSectionsFlowService = new Mock<IPagesWithSectionsFlowService>();

            _questionPropertyTokeniser = new Mock<IQuestionPropertyTokeniser>();
            _pageNavigationTrackingService = new Mock<IPageNavigationTrackingService>();
            _pageOverrideConfiguration = new Mock<IOptions<List<QnaPageOverrideConfiguration>>>();
            _pageOverrideConfiguration.Setup(x => x.Value).Returns(new List<QnaPageOverrideConfiguration>());
            _qnaLinks = new Mock<IOptions<List<QnaLinksConfiguration>>>();
            _qnaLinks.Setup(x => x.Value).Returns(new List<QnaLinksConfiguration>());
            _customValidatorFactory = new Mock<ICustomValidatorFactory>();
            _roatpApiClient = new Mock<IRoatpApiClient>();
            _submitApplicationEmailService = new Mock<ISubmitApplicationConfirmationEmailService>();
            _tabularDataRepository = new Mock<ITabularDataRepository>();
            _roatpTaskListWorkflowService = new Mock<IRoatpTaskListWorkflowService>();
            _roatpOrganisationVerificationService = new Mock<IRoatpOrganisationVerificationService>();
            _taskListOrchestrator = new Mock<ITaskListOrchestrator>();
            _ukrlpApiClient = new Mock<IUkrlpApiClient>();
            _reapplicationCheckService = new Mock<IReapplicationCheckService>();
            _resetQnaDetailsService = new Mock<IResetCompleteFlagService>();

            _controller = new RoatpApplicationController(_apiClient.Object, _logger.Object, _sessionService.Object,
                                                         _usersApiClient.Object, _qnaApiClient.Object, 
                                                          _pagesWithSectionsFlowService.Object,
                                                         _questionPropertyTokeniser.Object, _pageOverrideConfiguration.Object,
                                                         _pageNavigationTrackingService.Object, _qnaLinks.Object, _customValidatorFactory.Object,
                                                         _roatpApiClient.Object,
                                                         _submitApplicationEmailService.Object, _tabularDataRepository.Object,
                                                         _roatpTaskListWorkflowService.Object, _roatpOrganisationVerificationService.Object, _taskListOrchestrator.Object,
                                                         _ukrlpApiClient.Object, _reapplicationCheckService.Object, _resetQnaDetailsService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = user }
                }
            };

            var contact = new Contact
            {
                Id = USER_USERID,
                SigninId = USER_SIGNINID,
                GivenNames = USER_GIVEN_NAME,
                FamilyName = USER_FAMILY_NAME,
                Email = USER_EMAIL_ADDRESS
            };
            _usersApiClient.Setup(x => x.GetUserBySignInId(It.IsAny<Guid>())).ReturnsAsync(contact);
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
                new Apply { ApplicationStatus = ApplicationStatus.Cancelled }
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

        [TestCase(ApplicationStatus.Rejected, GatewayReviewStatus.Rejected)]
        [TestCase(ApplicationStatus.AppealSuccessful, GatewayReviewStatus.Fail)]

        public async Task Applications_shows_process_application_status_if_only_reapplication_requested_applications_and_reapplication_allowed_for_that_user(string applicationStatus, string gatewayReviewStatus)
        {
            var applications = new List<Domain.Entities.Apply>
            {
                new Apply
                {
                    ApplicationStatus = applicationStatus, GatewayReviewStatus = gatewayReviewStatus,
                    ApplyData = new ApplyData
                    {
                        ApplyDetails = new ApplyDetails
                        {
                            RequestToReapplyMade = true
                        }
                    }
                }
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
            _reapplicationCheckService.Setup(x => x.ReapplicationAllowed(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(true);
            var result = await _controller.Applications();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("ProcessApplicationStatus");
            redirectResult.ControllerName.Should().Be("RoatpOverallOutcome");
        }

        [TestCase(ApplicationStatus.Rejected, GatewayReviewStatus.Rejected)]
        [TestCase(ApplicationStatus.AppealSuccessful, GatewayReviewStatus.Fail)]

        public async Task Applications_shows_process_application_status_if_new_application_in_progress(string applicationStatus, string gatewayReviewStatus)
        {
            var applications = new List<Domain.Entities.Apply>
            {
                new Apply
                {
                    ApplicationStatus = applicationStatus, GatewayReviewStatus = gatewayReviewStatus,
                    ApplyData = new ApplyData
                    {
                        ApplyDetails = new ApplyDetails
                        {
                            RequestToReapplyMade = true
                        }
                    }
                }
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
            _reapplicationCheckService.Setup(x => x.ReapplicationAllowed(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(false);
            var result = await _controller.Applications();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("ProcessApplicationStatus");
            redirectResult.ControllerName.Should().Be("RoatpOverallOutcome");
        }

        [Test]
        public async Task
            Applications_shows_process_application_status_if_applications_called()
        {
            var submittedApp = new Domain.Entities.Apply {ApplyData = new ApplyData()};
            var applications = new List<Apply>
            {
                submittedApp
            };

            _apiClient.Setup(x => x.GetApplications(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(applications);

            var result = await _controller.Applications();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("ProcessApplicationStatus");
            redirectResult.ControllerName.Should().Be("RoatpOverallOutcome");
        }

        [Test]
        public async Task Applications_shows_enter_ukprn_page_if_application_cancelled()
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

            var result = await _controller.Applications();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("EnterApplicationUkprn");
            redirectResult.ControllerName.Should().Be("RoatpApplicationPreamble");
        }

        [Test]
        public async Task Applications_shows_enter_ukprn_page_if_no_active_applications()
        {
            _apiClient.Setup(x => x.GetApplications(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(new List<Apply>());

            var result = await _controller.Applications();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("EnterApplicationUkprn");
            redirectResult.ControllerName.Should().Be("RoatpApplicationPreamble");
        }

        [Test]
        public async Task Applications_shows_Applications_page_if_multiple_applications_in_progress()
        {
            var inProgressApp = new Domain.Entities.Apply
            {
                ApplicationStatus = ApplicationStatus.InProgress
            };

            var submittedApp = new Domain.Entities.Apply
            {
                ApplicationStatus = ApplicationStatus.Submitted
            };

            var gatewayAssessedApp = new Domain.Entities.Apply
            {
                ApplicationStatus = ApplicationStatus.GatewayAssessed
            };

            var applications = new List<Domain.Entities.Apply>
            {
                inProgressApp,
                submittedApp,
                gatewayAssessedApp
            };

            _apiClient.Setup(x => x.GetApplications(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(applications);

            var result = await _controller.Applications();

            var viewResult = result as ViewResult;
            viewResult.ViewName.Should().EndWith("Applications.cshtml");
        }

        [Test]
        public void Submit_application_presents_confirmation_page_with_legal_name_and_emailaddress()
        {
            var ukprn = "123456";
            var organisationNameAnswer = new Answer
            {
                QuestionId = "ORG-1",
                Value = "My organisation"
            };

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpLegalName, It.IsAny<string>())).ReturnsAsync(organisationNameAnswer);

            var application = new Apply
            {
                ApplicationId = Guid.NewGuid(),
                ApplicationStatus = ApplicationStatus.InProgress,
                ApplyData = new ApplyData
                {
                    ApplyDetails = new ApplyDetails {UKPRN = ukprn},
                    Sequences = new List<ApplySequence>()
                }
            };

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(application);
            _apiClient.Setup(x => x.GetAllowedProvider(ukprn))
                .ReturnsAsync(new AllowedProvider {EndDateTime = DateTime.Today});

            var result = _controller.SubmitApplication(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as SubmitApplicationViewModel;
            model.Should().NotBeNull();
            model.OrganisationName.Should().Be(organisationNameAnswer.Value);
            model.EmailAddress.Should().NotBeNull();
        }


        [Test]
        public void Submit_application_presents_post_invitation_window_closed_as_date_passed()
        {
            var ukprn = "123456";
            var organisationNameAnswer = new Answer
            {
                QuestionId = "ORG-1",
                Value = "My organisation"
            };

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpLegalName, It.IsAny<string>())).ReturnsAsync(organisationNameAnswer);

            var application = new Apply
            {
                ApplicationId = Guid.NewGuid(),
                ApplicationStatus = ApplicationStatus.InProgress,
                ApplyData = new ApplyData
                {
                    ApplyDetails = new ApplyDetails { UKPRN = ukprn },
                    Sequences = new List<ApplySequence>()
                }
            };

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(application);
            _apiClient.Setup(x => x.GetAllowedProvider(ukprn))
                .ReturnsAsync(new AllowedProvider { EndDateTime = DateTime.Today.AddDays(-1) });

            var result = _controller.SubmitApplication(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("InvitationWindowClosed.cshtml");
        }

        [Test]
        public void Submit_application_presents_post_invitation_window_closed_as_no_entry_in_allowed_providers()
        {
            var ukprn = "123456";
            var organisationNameAnswer = new Answer
            {
                QuestionId = "ORG-1",
                Value = "My organisation"
            };

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UkrlpLegalName, It.IsAny<string>())).ReturnsAsync(organisationNameAnswer);

            var application = new Apply
            {
                ApplicationId = Guid.NewGuid(),
                ApplicationStatus = ApplicationStatus.InProgress,
                ApplyData = new ApplyData
                {
                    ApplyDetails = new ApplyDetails { UKPRN = ukprn },
                    Sequences = new List<ApplySequence>()
                }
            };

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(application);
            _apiClient.Setup(x => x.GetAllowedProvider(ukprn))
                .ReturnsAsync((AllowedProvider)null);

            var result = _controller.SubmitApplication(Guid.NewGuid()).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("InvitationWindowClosed.cshtml");
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
                ConfirmChangeOfOwnershipSubmitApplication = true,
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

            _submitApplicationEmailService.Setup(x => x.SendSubmitConfirmationEmail(It.IsAny<ApplicationSubmitConfirmation>())).Returns(Task.FromResult(true));

            var result = await _controller.ConfirmSubmitApplication(model);

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ProcessApplicationStatus");

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
                ConfirmChangeOfOwnershipSubmitApplication = true,
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
                    new JProperty(RoatpWorkflowQuestionTags.IntangibleAssets, "9"),
                    new JProperty(RoatpWorkflowQuestionTags.AccountingReferenceDate, "01,01,2021"),
                    new JProperty(RoatpWorkflowQuestionTags.AccountingPeriod, "10"),
                    new JProperty(RoatpWorkflowQuestionTags.AverageNumberofFTEEmployees, "11")
                });

            var providerRouteAnswer = new Answer
            {
                QuestionId = "YO-1",
                Value = ApplicationRoute.MainProviderApplicationRoute.ToString()
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.ProviderRoute, It.IsAny<string>())).ReturnsAsync(providerRouteAnswer);

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(application);

            _apiClient.Setup(x => x.SubmitApplication(It.IsAny<Application.Apply.Submit.SubmitApplicationRequest>())).ReturnsAsync(true);

            _submitApplicationEmailService.Setup(x => x.SendSubmitConfirmationEmail(It.IsAny<ApplicationSubmitConfirmation>())).Returns(Task.FromResult(true));

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
                    && r.FinancialData.AccountingReferenceDate == new DateTime(2021, 1, 1)
                    && r.FinancialData.AccountingPeriod == 10
                    && r.FinancialData.AverageNumberofFTEEmployees == 11
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
                ConfirmChangeOfOwnershipSubmitApplication = true,
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
                            new Answer{ QuestionId = "FH-220", Value = "9"},
                            new Answer{ QuestionId = "FH-420", Value = "01,01,2021"},
                            new Answer{ QuestionId = "FH-430", Value = "10"}
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

            _submitApplicationEmailService.Setup(x => x.SendSubmitConfirmationEmail(It.IsAny<ApplicationSubmitConfirmation>())).Returns(Task.FromResult(true));

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
                    && r.FinancialData.AccountingReferenceDate == null
                    && r.FinancialData.AccountingPeriod == null
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
                ConfirmChangeOfOwnershipSubmitApplication = true,
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
                            new Answer{ QuestionId = "FH-220", Value = "9"},
                            new Answer{ QuestionId = "FH-420", Value = "01,01,2021"},
                            new Answer{ QuestionId = "FH-430", Value = "10"}
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

            _submitApplicationEmailService.Setup(x => x.SendSubmitConfirmationEmail(It.IsAny<ApplicationSubmitConfirmation>())).Returns(Task.FromResult(true));

            await _controller.ConfirmSubmitApplication(model);

            _apiClient.Verify(x =>
                x.SubmitApplication(It.Is<SubmitApplicationRequest>(r => r.OrganisationType == "test")));
        }

        [Test]
        public async Task TaskList_shows_tasklist_view_for_application()
        {
            var applicationId = Guid.NewGuid();

            var inProgressApp = new Apply
            {
                ApplicationStatus = ApplicationStatus.InProgress,
                ApplyData = new ApplyData()
            };

            _apiClient.Setup(x => x.GetApplicationByUserId(applicationId, USER_SIGNINID)).ReturnsAsync(() => inProgressApp);

            _taskListOrchestrator.Setup(x => x.GetTaskListViewModel(applicationId, USER_USERID))
                .ReturnsAsync(() => new TaskListViewModel());

            var result = await _controller.TaskList(applicationId);

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult) result;
            Assert.AreEqual("~/Views/Roatp/TaskList.cshtml", viewResult.ViewName);
        }

        [Test]
        public async Task SaveAnswers_OnSuccessfulUpdateAndRedirect_CallsServicesOnce()
        {
            var _applicationId = Guid.NewGuid();
            var _sectionId = Guid.NewGuid();
            var _sequenceNo = 1;
            var _sectionNo = 5;
            var _pageId = "235";
            var viewModel = new PageViewModel();
            viewModel.ApplicationId = _applicationId;
            viewModel.SequenceId = _sequenceNo;
            viewModel.SectionId = _sectionNo;
            viewModel.PageId = _pageId;
            viewModel.RedirectAction = "RedirectActionText";
            var _section = new ApplySection { SectionNo = _sectionNo, SectionId = _sectionId };
            var _page = new Page {PageId = _pageId,Questions = new List<Question>{new Question {Input = new Input {Type="Text"}}}};
            var _applicationSection = new ApplicationSection {QnAData = new QnAData {Pages = new List<Page>{_page}}, SequenceId = _sequenceNo, SectionId = _sectionNo};

            var formAction = "test";
            var application = new Apply
            {
                ApplicationId = _applicationId,
                ApplyData = new ApplyData { Sequences = new List<ApplySequence>(){new ApplySequence {SequenceNo = _sequenceNo, Sections = new List<ApplySection> {_section}}}},
                ApplicationStatus = ApplicationStatus.InProgress
            };

            _apiClient.Setup(x => x.GetApplicationByUserId(viewModel.ApplicationId, It.IsAny<Guid>())).ReturnsAsync(application); 
            _qnaApiClient.Setup(x => x.CanUpdatePage(_applicationId, _sectionId, _pageId)).ReturnsAsync(true);
            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(_applicationId, _sequenceNo, _sectionNo)).ReturnsAsync(_applicationSection);
            _qnaApiClient
                .Setup(x => x.UpdatePageAnswers(_applicationId, It.IsAny<Guid>(), _pageId, It.IsAny<List<Answer>>()))
                .ReturnsAsync(new SetPageAnswersResponse {ValidationPassed = true});
            
            _controller.HttpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>());

            var result = await  _controller.SaveAnswers(viewModel, formAction);
            var redirectToActionResult = result as RedirectToActionResult;

            _resetQnaDetailsService.Verify(x => x.ResetPagesComplete(_applicationId,  _pageId), Times.Once);
            _roatpTaskListWorkflowService.Verify(x=>x.RefreshNotRequiredOverrides(_applicationId),Times.Once);
            Assert.AreEqual("TaskList",redirectToActionResult.ActionName);
        }

        [Test]
        public async Task SaveAnswers_OnUnsuccessfulUpdate_CallsServicesNever()
        {
            var _applicationId = Guid.NewGuid();
            var _sectionId = Guid.NewGuid();
            var _sequenceNo = 1;
            var _sectionNo = 5;
            var _pageId = "235";
            var viewModel = new PageViewModel();
            viewModel.ApplicationId = _applicationId;
            viewModel.SequenceId = _sequenceNo;
            viewModel.SectionId = _sectionNo;
            viewModel.PageId = _pageId;
            viewModel.RedirectAction = "RedirectActionText";
            var section = new ApplySection { SectionNo = _sectionNo, SectionId = _sectionId };
       
            var formAction = "test";
            var application = new Apply
            {
                ApplicationId = _applicationId,
                ApplyData = new ApplyData { Sequences = new List<ApplySequence>() { new ApplySequence { SequenceNo = _sequenceNo, Sections = new List<ApplySection> { section } } } },
                ApplicationStatus = ApplicationStatus.InProgress
            };

            _apiClient.Setup(x => x.GetApplicationByUserId(viewModel.ApplicationId, It.IsAny<Guid>())).ReturnsAsync(application);
            _qnaApiClient.Setup(x => x.CanUpdatePage(_applicationId, _sectionId, _pageId)).ReturnsAsync(false);
            _qnaApiClient
                .Setup(x => x.UpdatePageAnswers(_applicationId, It.IsAny<Guid>(), _pageId, It.IsAny<List<Answer>>()))
                .ReturnsAsync(new SetPageAnswersResponse { ValidationPassed = true });

            _controller.HttpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>());

            var result = await _controller.SaveAnswers(viewModel, formAction);
            var redirectToActionResult = result as RedirectToActionResult;

            Assert.AreEqual("TaskList", redirectToActionResult.ActionName);
            _resetQnaDetailsService.Verify(x => x.ResetPagesComplete(_applicationId, _pageId), Times.Never);
            _roatpTaskListWorkflowService.Verify(x => x.RefreshNotRequiredOverrides(_applicationId), Times.Never);
        }

        [Test]
        public async Task SaveAnswers_OnModelStateInvalid_CallsServicesNever()
        {
            var _applicationId = Guid.NewGuid();
            var _sectionId = Guid.NewGuid();
            var _sequenceNo = 1;
            var _sectionNo = 5;
            var _pageId = "235";
            var viewModel = new PageViewModel();
            viewModel.ApplicationId = _applicationId;
            viewModel.SequenceId = _sequenceNo;
            viewModel.SectionId = _sectionNo;
            viewModel.PageId = _pageId;
            viewModel.RedirectAction = "RedirectActionText";
            var _section = new ApplySection { SectionNo = _sectionNo, SectionId = _sectionId };
            var _page = new Page { PageId = _pageId, Questions = new List<Question> { new Question { Input = new Input { Type = "Text" } } } };
            var _applicationSection = new ApplicationSection { QnAData = new QnAData { Pages = new List<Page> { _page } }, SequenceId = _sequenceNo, SectionId = _sectionNo };

            var formAction = "test";
            var application = new Apply
            {
                ApplicationId = _applicationId,
                ApplyData = new ApplyData { Sequences = new List<ApplySequence>() { new ApplySequence { SequenceNo = _sequenceNo, Sections = new List<ApplySection> { _section } } } },
                ApplicationStatus = ApplicationStatus.InProgress
            };

            _apiClient.Setup(x => x.GetApplicationByUserId(viewModel.ApplicationId, It.IsAny<Guid>())).ReturnsAsync(application);
            _qnaApiClient.Setup(x => x.CanUpdatePage(_applicationId, _sectionId, _pageId)).ReturnsAsync(true);
            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(_applicationId, _sequenceNo, _sectionNo)).ReturnsAsync(_applicationSection);
            _qnaApiClient
                .Setup(x => x.UpdatePageAnswers(_applicationId, It.IsAny<Guid>(), _pageId, It.IsAny<List<Answer>>()))
                .ReturnsAsync(new SetPageAnswersResponse { ValidationPassed = true });

            _controller.HttpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>());
            _controller.ModelState.AddModelError("key", "error message");
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
                 {
                     ["InvalidPageOfAnswers"] = JsonConvert.SerializeObject(new List<PageOfAnswers>())
                 };
            _controller.TempData =tempData;
            var result = await _controller.SaveAnswers(viewModel, formAction);
            var viewResult = result as ViewResult;

            _resetQnaDetailsService.Verify(x => x.ResetPagesComplete(_applicationId,  _pageId), Times.Never);
            _roatpTaskListWorkflowService.Verify(x => x.RefreshNotRequiredOverrides(_applicationId), Times.Never);
            Assert.IsTrue(viewResult.ViewName.Contains("Index.cshtml"));
        }

        [Test]
        public async Task SaveAnswers_PageUpateValidationFailed_CallsServicesNever()
        {
            var _applicationId = Guid.NewGuid();
            var _sectionId = Guid.NewGuid();
            var _sequenceNo = 1;
            var _sectionNo = 5;
            var _pageId = "235";
            var viewModel = new PageViewModel();
            viewModel.ApplicationId = _applicationId;
            viewModel.SequenceId = _sequenceNo;
            viewModel.SectionId = _sectionNo;
            viewModel.PageId = _pageId;
            viewModel.RedirectAction = "RedirectActionText";
            var _section = new ApplySection { SectionNo = _sectionNo, SectionId = _sectionId };
            var _page = new Page { PageId = _pageId, Questions = new List<Question> { new Question { Input = new Input { Type = "Text" } } } };
            var _applicationSection = new ApplicationSection { QnAData = new QnAData { Pages = new List<Page> { _page } }, SequenceId = _sequenceNo, SectionId = _sectionNo };

            var formAction = "test";
            var application = new Apply
            {
                ApplicationId = _applicationId,
                ApplyData = new ApplyData { Sequences = new List<ApplySequence>() { new ApplySequence { SequenceNo = _sequenceNo, Sections = new List<ApplySection> { _section } } } },
                ApplicationStatus = ApplicationStatus.InProgress
            };

            _apiClient.Setup(x => x.GetApplicationByUserId(viewModel.ApplicationId, It.IsAny<Guid>())).ReturnsAsync(application);
            _qnaApiClient.Setup(x => x.CanUpdatePage(_applicationId, _sectionId, _pageId)).ReturnsAsync(true);
            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(_applicationId, _sequenceNo, _sectionNo)).ReturnsAsync(_applicationSection);
            _qnaApiClient
                .Setup(x => x.UpdatePageAnswers(_applicationId, It.IsAny<Guid>(), _pageId, It.IsAny<List<Answer>>()))
                .ReturnsAsync(new SetPageAnswersResponse { ValidationPassed = false });
            _qnaApiClient.Setup(x => x.GetPage(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(() => new Page
                {
                    PageOfAnswers = new List<PageOfAnswers>
                    {
                        new PageOfAnswers{Answers = new List<Answer>()}
                    },
                    Questions = new List<Question>()
                });

            _controller.HttpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>());
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>())
            {
                ["InvalidPageOfAnswers"] = JsonConvert.SerializeObject(new List<PageOfAnswers>())
            };
            _controller.TempData = tempData;

            var result = await _controller.SaveAnswers(viewModel, formAction);
            var redirectToActionResult = result as RedirectToActionResult;

            _resetQnaDetailsService.Verify(x => x.ResetPagesComplete(_applicationId, _pageId), Times.Never);
            _roatpTaskListWorkflowService.Verify(x => x.RefreshNotRequiredOverrides(_applicationId), Times.Never);
            Assert.AreEqual("Page", redirectToActionResult.ActionName);
        }
    }
}
