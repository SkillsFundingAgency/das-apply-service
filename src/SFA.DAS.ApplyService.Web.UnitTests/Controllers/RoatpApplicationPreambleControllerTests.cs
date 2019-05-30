namespace SFA.DAS.ApplyService.Web.UnitTests.Controllers
{
    using System;
    using Domain.Roatp;
    using Microsoft.Extensions.Logging;
    using Moq;
    using NUnit.Framework;
    using SFA.DAS.ApplyService.Session;
    using SFA.DAS.ApplyService.Web.Controllers.Roatp;
    using SFA.DAS.ApplyService.Web.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Roatp;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using SFA.DAS.ApplyService.Domain.Ukrlp;
    using System.Threading.Tasks;
    using InternalApi.Types.CharityCommission;
    using SFA.DAS.ApplyService.Domain.CompaniesHouse;

    [TestFixture]
    public class RoatpApplicationPreambleControllerTests
    {
        private Mock<ILogger<RoatpApplicationPreambleController>> _logger;
        private Mock<IRoatpApiClient> _roatpApiClient;
        private Mock<IUkrlpApiClient> _ukrlpApiClient;
        private Mock<ISessionService> _sessionService;
        private Mock<ICompaniesHouseApiClient> _companiesHouseApiClient;
        private Mock<ICharityCommissionApiClient> _charityCommissionApiClient;
        private Mock<IOrganisationApiClient> _organisationApiClient;
        private Mock<IUsersApiClient> _usersApiClient;

        private RoatpApplicationPreambleController _controller;

        private CompaniesHouseSummary _activeCompany;
        private Charity _activeCharity;

        [SetUp]
        public void Before_each_test()
        {
            _logger = new Mock<ILogger<RoatpApplicationPreambleController>>();
            _roatpApiClient = new Mock<IRoatpApiClient>();
            _ukrlpApiClient = new Mock<IUkrlpApiClient>();
            _sessionService = new Mock<ISessionService>();
            _companiesHouseApiClient = new Mock<ICompaniesHouseApiClient>();
            _charityCommissionApiClient = new Mock<ICharityCommissionApiClient>();
            _organisationApiClient = new Mock<IOrganisationApiClient>();
            _usersApiClient = new Mock<IUsersApiClient>();

            _controller = new RoatpApplicationPreambleController(_logger.Object, _roatpApiClient.Object, _ukrlpApiClient.Object, 
                                                                 _sessionService.Object, _companiesHouseApiClient.Object,
                                                                 _charityCommissionApiClient.Object,
                                                                 _organisationApiClient.Object,
                                                                 _usersApiClient.Object);
            _activeCompany = new CompaniesHouseSummary
            {
                CompanyNumber = "12345678",
                CompanyType = "ltd",
                Directors = new List<DirectorInformation>
                {
                    new DirectorInformation
                    {
                        Id = "1234",
                        DateOfBirth = new DateTime(1948, 11, 1),
                        AppointedDate = new DateTime(1960, 12, 12),
                        ResignedDate = null,
                        Name = "Mr A Director"
                    }
                },
                PersonsSignificationControl = new List<PersonSignificantControlInformation>
                {
                    new PersonSignificantControlInformation
                    {
                        Id = "1234",
                        DateOfBirth = new DateTime(1948, 11, 1),
                        Name = "Mr A Director"
                    }
                },
                IncorporationDate = new DateTime(1960, 12, 12),
                Status = "active"
            };

            _activeCharity = new Charity
            {
                Status = "registered",
                CharityNumber = "12345678",
                Trustees = new List<Trustee>
                {
                    new Trustee
                    {
                        Id = 1,
                        Name = "MR A TRUSTEE"
                    }
                },
                IncorporatedOn = new DateTime(2019, 1, 1),
                DissolvedOn = null
            };
        }

        [Test]
        public void Select_application_routes_presents_choice_of_routes()
        {
            var applicationRoutes = new List<ApplicationRoute>
            {
                new ApplicationRoute { Id = ApplicationRoute.MainProviderApplicationRoute, RouteName = "Main provider" },
                new ApplicationRoute { Id = ApplicationRoute.EmployerProviderApplicationRoute, RouteName = "Employer provider" },
                new ApplicationRoute { Id = ApplicationRoute.SupportingProviderApplicationRoute, RouteName = "Supporting provider" }
            };

            _roatpApiClient.Setup(x => x.GetApplicationRoutes()).ReturnsAsync(applicationRoutes);

            var result = _controller.SelectApplicationRoute().GetAwaiter().GetResult();

            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult.Model.Should().BeOfType<SelectApplicationRouteViewModel>();

            var model = viewResult.Model as SelectApplicationRouteViewModel;
            model.ApplicationRouteId.Should().Be(0);    // unselected
            var applicationRouteList = model.ApplicationRoutes.ToList();
            applicationRouteList.Count.Should().Be(3);
            applicationRouteList[0].Id.Should().Be(applicationRoutes[0].Id);
            applicationRouteList[0].RouteName.Should().Be(applicationRoutes[0].RouteName);
            applicationRouteList[1].Id.Should().Be(applicationRoutes[1].Id);
            applicationRouteList[1].RouteName.Should().Be(applicationRoutes[1].RouteName);
            applicationRouteList[2].Id.Should().Be(applicationRoutes[2].Id);
            applicationRouteList[2].RouteName.Should().Be(applicationRoutes[2].RouteName);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(4)]
        public void Validation_error_is_triggered_if_application_route_not_selected_or_invalid(int applicationRouteId)
        {
            var applicationRoutes = new List<ApplicationRoute>
            {
                new ApplicationRoute { Id = ApplicationRoute.MainProviderApplicationRoute, RouteName = "Main provider" },
                new ApplicationRoute { Id = ApplicationRoute.EmployerProviderApplicationRoute, RouteName = "Employer provider" },
                new ApplicationRoute { Id = ApplicationRoute.SupportingProviderApplicationRoute, RouteName = "Supporting provider" }
            };

            _roatpApiClient.Setup(x => x.GetApplicationRoutes()).ReturnsAsync(applicationRoutes);

            var model = new SelectApplicationRouteViewModel {ApplicationRouteId = applicationRouteId};
            _controller.ModelState.AddModelError("Application Route", "Select an application route");

            var result = _controller.EnterApplicationUkprn(model).GetAwaiter().GetResult();

            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult.ViewName.Should().Contain("SelectApplicationRoute");
        }

        [TestCase(ApplicationRoute.MainProviderApplicationRoute)]
        [TestCase(ApplicationRoute.EmployerProviderApplicationRoute)]
        [TestCase(ApplicationRoute.SupportingProviderApplicationRoute)]
        public void User_is_prompted_to_enter_UKPRN_if_selected_application_route_is_valid(int applicationRouteId)
        {
            _sessionService.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<ApplicationDetails>()));

            var model = new SelectApplicationRouteViewModel { ApplicationRouteId = applicationRouteId };

            var result = _controller.EnterApplicationUkprn(model).GetAwaiter().GetResult();

            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult.ViewName.Should().Contain("EnterApplicationUkprn");
            var viewModel = viewResult.Model as SearchByUkprnViewModel;
            viewModel.Should().NotBeNull();
            viewModel.ApplicationRouteId.Should().Be(model.ApplicationRouteId);
            viewModel.UKPRN.Should().BeNullOrEmpty();
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("1234567")]
        [TestCase("123456789")]
        [TestCase("1234567A")]
        public void Validation_error_is_triggered_if_UKPRN_not_in_correct_format(string ukprn)
        {
            var model = new SearchByUkprnViewModel
            {
                UKPRN = ukprn
            };

            var httpContext = new DefaultHttpContext();
            var tempDataProvider = new Mock<ITempDataProvider>();
            _controller.TempData = new TempDataDictionary(httpContext, tempDataProvider.Object);
            var result = _controller.SearchByUkprn(model).GetAwaiter().GetResult();

            result.Should().BeOfType<ViewResult>();

            var viewResult = result as ViewResult;
            viewResult.ViewName.Should().Contain("EnterApplicationUkprn");
            _controller.ModelState.ErrorCount.Should().Be(1);

        }

        [Test]
        public void UKPRN_is_not_found_on_UKRLP()
        {
            var noResults = new List<ProviderDetails>();
            _ukrlpApiClient.Setup(x => x.GetTrainingProviderByUkprn(It.IsAny<long>())).ReturnsAsync(noResults);

            var applicationDetails = new ApplicationDetails
            {
                ApplicationRoute = new ApplicationRoute
                {
                    Id = ApplicationRoute.MainProviderApplicationRoute, RouteName = "Main provider"
                }
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);
            _sessionService.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<ApplicationDetails>()));

            var model = new SearchByUkprnViewModel
            {
                UKPRN = "10001000"
            };

            var result = _controller.SearchByUkprn(model).GetAwaiter().GetResult();

            result.Should().BeOfType<RedirectToActionResult>();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("UkprnNotFound");
        }

        [Test]
        public void UKPRN_is_found_on_UKRLP_but_existing_active_provider_on_register_with_same_application_route()
        {
            var matchingResult = new List<ProviderDetails>
            {
                new ProviderDetails
                {
                    UKPRN = "10001000",
                    ProviderName = "Test Provider"
                }
            };
            _ukrlpApiClient.Setup(x => x.GetTrainingProviderByUkprn(It.IsAny<long>())).ReturnsAsync(matchingResult);

            var applicationDetails = new ApplicationDetails
            {
                ApplicationRoute = new ApplicationRoute
                {
                    Id = ApplicationRoute.MainProviderApplicationRoute,
                    RouteName = "Main provider"
                }
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);
            _sessionService.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<ApplicationDetails>()));

            var registerStatus = new OrganisationRegisterStatus
            {
                ProviderTypeId = ApplicationRoute.MainProviderApplicationRoute,
                StatusId = OrganisationRegisterStatus.ActiveStatus,
                ExistingUKPRN = true
            };
            _roatpApiClient.Setup(x => x.UkprnOnRegister(It.IsAny<long>())).ReturnsAsync(registerStatus);

            var model = new SearchByUkprnViewModel
            {
                UKPRN = "10001000"
            };

            var result = _controller.SearchByUkprn(model).GetAwaiter().GetResult();

            result.Should().BeOfType<RedirectToActionResult>();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("UkprnActive");
        }

        [Test]
        public void UKPRN_is_found_on_UKRLP_but_existing_active_provider_on_register_with_different_application_route()
        {
            var matchingResult = new List<ProviderDetails>
            {
                new ProviderDetails
                {
                    UKPRN = "10001000",
                    ProviderName = "Test Provider"
                }
            };
            _ukrlpApiClient.Setup(x => x.GetTrainingProviderByUkprn(It.IsAny<long>())).ReturnsAsync(matchingResult);

            var applicationDetails = new ApplicationDetails
            {
                ApplicationRoute = new ApplicationRoute
                {
                    Id = ApplicationRoute.MainProviderApplicationRoute,
                    RouteName = "Main provider"
                }
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);
            _sessionService.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<ApplicationDetails>()));

            var registerStatus = new OrganisationRegisterStatus
            {
                ProviderTypeId = ApplicationRoute.SupportingProviderApplicationRoute,
                StatusId = OrganisationRegisterStatus.ActiveStatus,
                ExistingUKPRN = true
            };
            _roatpApiClient.Setup(x => x.UkprnOnRegister(It.IsAny<long>())).ReturnsAsync(registerStatus);

            var model = new SearchByUkprnViewModel
            {
                UKPRN = "10001000"
            };

            var result = _controller.SearchByUkprn(model).GetAwaiter().GetResult();

            result.Should().BeOfType<RedirectToActionResult>();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("UkprnFound");
        }

        [Test]
        public void UKPRN_is_found_on_UKRLP_but_existing_provider_on_register_with_status_of_removed()
        {
            var matchingResult = new List<ProviderDetails>
            {
                new ProviderDetails
                {
                    UKPRN = "10001000",
                    ProviderName = "Test Provider"
                }
            };
            _ukrlpApiClient.Setup(x => x.GetTrainingProviderByUkprn(It.IsAny<long>())).ReturnsAsync(matchingResult);

            var applicationDetails = new ApplicationDetails
            {
                ApplicationRoute = new ApplicationRoute
                {
                    Id = ApplicationRoute.MainProviderApplicationRoute,
                    RouteName = "Main provider"
                }
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);
            _sessionService.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<ApplicationDetails>()));

            var registerStatus = new OrganisationRegisterStatus
            {
                ProviderTypeId = ApplicationRoute.MainProviderApplicationRoute,
                StatusId = OrganisationRegisterStatus.RemovedStatus,
                ExistingUKPRN = true
            };
            _roatpApiClient.Setup(x => x.UkprnOnRegister(It.IsAny<long>())).ReturnsAsync(registerStatus);

            var model = new SearchByUkprnViewModel
            {
                UKPRN = "10001000"
            };

            var result = _controller.SearchByUkprn(model).GetAwaiter().GetResult();

            result.Should().BeOfType<RedirectToActionResult>();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("UkprnFound");
        }

        [Test]
        public void UKPRN_is_found_on_UKRLP_and_not_already_on_register()
        {
            var matchingResult = new List<ProviderDetails>
            {
                new ProviderDetails
                {
                    UKPRN = "10001000",
                    ProviderName = "Test Provider"
                }
            };
            _ukrlpApiClient.Setup(x => x.GetTrainingProviderByUkprn(It.IsAny<long>())).ReturnsAsync(matchingResult);

            var applicationDetails = new ApplicationDetails
            {
                ApplicationRoute = new ApplicationRoute
                {
                    Id = ApplicationRoute.MainProviderApplicationRoute,
                    RouteName = "Main provider"
                }
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);
            _sessionService.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<ApplicationDetails>()));

            var registerStatus = new OrganisationRegisterStatus
            {
                ExistingUKPRN = false
            };
            _roatpApiClient.Setup(x => x.UkprnOnRegister(It.IsAny<long>())).ReturnsAsync(registerStatus);

            var model = new SearchByUkprnViewModel
            {
                UKPRN = "10001000"
            };

            var result = _controller.SearchByUkprn(model).GetAwaiter().GetResult();

            result.Should().BeOfType<RedirectToActionResult>();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("UkprnFound");
        }

        [Test]
        public void UKPRN_is_verified_against_companies_house()
        {
            var providerDetails = new ProviderDetails
            {
                UKPRN = "10001000",
                ProviderName = "Test Provider",
                VerificationDetails = new List<VerificationDetails>
                {
                    new VerificationDetails
                    {
                        VerificationAuthority = VerificationAuthorities.CompaniesHouseAuthority,
                        VerificationId = "12345678"
                    }
                }
            };
            
            var applicationDetails = new ApplicationDetails
            {
                ApplicationRoute = new ApplicationRoute
                {
                    Id = ApplicationRoute.MainProviderApplicationRoute,
                    RouteName = "Main provider"
                },
                UkrlpLookupDetails = providerDetails
            };
       
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);
            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>())).Returns(Task.FromResult(_activeCompany)).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>())).Verifiable();

            var result = _controller.UkprnFound().GetAwaiter().GetResult();
            
            _companiesHouseApiClient.Verify(x => x.GetCompanyDetails(It.IsAny<string>()), Times.Once);
            _charityCommissionApiClient.Verify(x => x.GetCharityDetails(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void UKPRN_is_verified_against_charity_commission()
        {
            var providerDetails = new ProviderDetails
            {
                UKPRN = "10001000",
                ProviderName = "Test Provider",
                VerificationDetails = new List<VerificationDetails>
                {
                    new VerificationDetails
                    {
                        VerificationAuthority = VerificationAuthorities.CharityCommissionAuthority,
                        VerificationId = "12345678"
                    }
                }
            };

            var applicationDetails = new ApplicationDetails
            {
                ApplicationRoute = new ApplicationRoute
                {
                    Id = ApplicationRoute.MainProviderApplicationRoute,
                    RouteName = "Main provider"
                },
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);
            
            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>())).Returns(Task.FromResult(_activeCompany)).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>())).Returns(Task.FromResult(_activeCharity)).Verifiable();

            var result = _controller.UkprnFound().GetAwaiter().GetResult();

            _companiesHouseApiClient.Verify(x => x.GetCompanyDetails(It.IsAny<string>()), Times.Never);
            _charityCommissionApiClient.Verify(x => x.GetCharityDetails(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void UKPRN_is_verified_against_companies_house_and_charity_commission()
        {
            var providerDetails = new ProviderDetails
            {
                UKPRN = "10001000",
                ProviderName = "Test Provider",
                VerificationDetails = new List<VerificationDetails>
                {
                    new VerificationDetails
                    {
                        VerificationAuthority = VerificationAuthorities.CompaniesHouseAuthority,
                        VerificationId = "12345678"
                    },
                    new VerificationDetails
                    {
                        VerificationAuthority = VerificationAuthorities.CharityCommissionAuthority,
                        VerificationId = "0123456"
                    }
                }
            };

            var applicationDetails = new ApplicationDetails
            {
                ApplicationRoute = new ApplicationRoute
                {
                    Id = ApplicationRoute.MainProviderApplicationRoute,
                    RouteName = "Main provider"
                },
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>())).Returns(Task.FromResult(_activeCompany)).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>())).Returns(Task.FromResult(_activeCharity)).Verifiable();

            var result = _controller.UkprnFound().GetAwaiter().GetResult();

            _companiesHouseApiClient.Verify(x => x.GetCompanyDetails(It.IsAny<string>()), Times.Once);
            _charityCommissionApiClient.Verify(x => x.GetCharityDetails(It.IsAny<int>()), Times.Once);
        }

        [TestCase("liquidation")]
        [TestCase(null)]
        [TestCase("")]
        public void UKPRN_is_verified_against_companies_house_but_company_not_active(string status)
        {
            var providerDetails = new ProviderDetails
            {
                UKPRN = "10001000",
                ProviderName = "Test Provider",
                VerificationDetails = new List<VerificationDetails>
                {
                    new VerificationDetails
                    {
                        VerificationAuthority = VerificationAuthorities.CompaniesHouseAuthority,
                        VerificationId = "12345678"
                    }
                }
            };

            var applicationDetails = new ApplicationDetails
            {
                ApplicationRoute = new ApplicationRoute
                {
                    Id = ApplicationRoute.MainProviderApplicationRoute,
                    RouteName = "Main provider"
                },
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            var inactiveCompany = new CompaniesHouseSummary
            {
                Status = status
            };
            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>())).Returns(Task.FromResult(inactiveCompany)).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>())).Verifiable();

            var result = _controller.UkprnFound().GetAwaiter().GetResult();
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("CompanyNotActive");

            _companiesHouseApiClient.Verify(x => x.GetCompanyDetails(It.IsAny<string>()), Times.Once);
            _charityCommissionApiClient.Verify(x => x.GetCharityDetails(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void UKPRN_is_verified_against_charity_commission_but_charity_not_active()
        {
            var providerDetails = new ProviderDetails
            {
                UKPRN = "10001000",
                ProviderName = "Test Provider",
                VerificationDetails = new List<VerificationDetails>
                {
                    new VerificationDetails
                    {
                        VerificationAuthority = VerificationAuthorities.CharityCommissionAuthority,
                        VerificationId = "12345678"
                    }
                }
            };

            var applicationDetails = new ApplicationDetails
            {
                ApplicationRoute = new ApplicationRoute
                {
                    Id = ApplicationRoute.MainProviderApplicationRoute,
                    RouteName = "Main provider"
                },
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            var inactiveCharity = new Charity
            {
                Status = "removed",
                DissolvedOn = new DateTime(2010, 1, 1)
            };
            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>())).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>())).Returns(Task.FromResult(inactiveCharity)).Verifiable();

            var result = _controller.UkprnFound().GetAwaiter().GetResult();
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("CharityNotActive");

            _companiesHouseApiClient.Verify(x => x.GetCompanyDetails(It.IsAny<string>()), Times.Never);
            _charityCommissionApiClient.Verify(x => x.GetCharityDetails(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void UKPRN_is_verified_against_charity_commission_but_charity_number_not_valid()
        {
            var providerDetails = new ProviderDetails
            {
                UKPRN = "10001000",
                ProviderName = "Test Provider",
                VerificationDetails = new List<VerificationDetails>
                {
                    new VerificationDetails
                    {
                        VerificationAuthority = VerificationAuthorities.CharityCommissionAuthority,
                        VerificationId = "AB123456"
                    }
                }
            };

            var applicationDetails = new ApplicationDetails
            {
                ApplicationRoute = new ApplicationRoute
                {
                    Id = ApplicationRoute.MainProviderApplicationRoute,
                    RouteName = "Main provider"
                },
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>())).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>())).Verifiable();

            var result = _controller.UkprnFound().GetAwaiter().GetResult();
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("CharityNotFound");

            _companiesHouseApiClient.Verify(x => x.GetCompanyDetails(It.IsAny<string>()), Times.Never);
            _charityCommissionApiClient.Verify(x => x.GetCharityDetails(It.IsAny<int>()), Times.Never);
        }
        
        [Test]
        public void UKPRN_is_not_verified_against_a_recognised_source()
        {
            var providerDetails = new ProviderDetails
            {
                UKPRN = "10001000",
                ProviderName = "Test Provider",
                VerificationDetails = new List<VerificationDetails>
                {
                    new VerificationDetails
                    {
                        VerificationAuthority = "Word of mouth",
                        VerificationId = "Y"
                    }
                }
            };

            var applicationDetails = new ApplicationDetails
            {
                ApplicationRoute = new ApplicationRoute
                {
                    Id = ApplicationRoute.MainProviderApplicationRoute,
                    RouteName = "Main provider"
                },
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);
            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>())).Returns(Task.FromResult(new CompaniesHouseSummary())).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>())).Verifiable();

            var result = _controller.UkprnFound().GetAwaiter().GetResult();

            _companiesHouseApiClient.Verify(x => x.GetCompanyDetails(It.IsAny<string>()), Times.Never);
            _charityCommissionApiClient.Verify(x => x.GetCharityDetails(It.IsAny<int>()), Times.Never);
        }

    }
}
