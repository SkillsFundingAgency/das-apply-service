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

        [SetUp]
        public void Before_each_test()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
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

            _controller = new RoatpApplicationController(_apiClient.Object, _logger.Object, _sessionService.Object, _configService.Object,
                                                         _userService.Object, _usersApiClient.Object, _qnaApiClient.Object, 
                                                          _pagesWithSectionsFlowService.Object,
                                                         _questionPropertyTokeniser.Object, _pageOverrideConfiguration.Object,
                                                         _pageNavigationTrackingService.Object, _qnaLinks.Object, _customValidatorFactory.Object,
                                                         _roatpApiClient.Object,
                                                         _submitApplicationEmailService.Object, _tabularDataRepository.Object,
                                                         _roatpTaskListWorkflowService.Object, _roatpOrganisationVerificationService.Object)
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
        public void Applications_starts_a_new_application_if_no_applications_for_that_user()
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

            var result = _controller.Applications().GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("Applications");

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
        public void Applications_shows_enter_ukprn_page_if_application_cancelled()
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
        public void Submit_application_presents_confirmation_page_with_legal_name()
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
        }

        [Test]
        public void Confirm_submit_application_updates_application_status_and_sends_confirmation_email_if_they_have_confirmed_details_are_correct()
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

            var providerRouteAnswer = new Answer
            {
                QuestionId = "YO-1",
                Value = ApplicationRoute.MainProviderApplicationRoute.ToString()
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.ProviderRoute, It.IsAny<string>())).ReturnsAsync(providerRouteAnswer);

            _apiClient.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(application);

            _apiClient.Setup(x => x.SubmitApplication(It.IsAny<Application.Apply.Submit.SubmitApplicationRequest>())).ReturnsAsync(true);

            _submitApplicationEmailService.Setup(x => x.SendGetHelpWithQuestionEmail(It.IsAny<ApplicationSubmitConfirmation>())).Returns(Task.FromResult(true));

            var result = _controller.ConfirmSubmitApplication(model).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ApplicationSubmitted");

            _submitApplicationEmailService.VerifyAll();
        }
    }
}
