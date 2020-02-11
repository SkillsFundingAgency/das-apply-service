using AutoMapper;

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
    using System.Security.Claims;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Roatp;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using SFA.DAS.ApplyService.Domain.Ukrlp;
    using System.Threading.Tasks;
    using Domain.CharityCommission;
    using Domain.Entities;
    using InternalApi.Types;
    using InternalApi.Types.CharityCommission;
    using SFA.DAS.ApplyService.Domain.CompaniesHouse;
    using SFA.DAS.ApplyService.Web.AutoMapper;
    using Trustee = InternalApi.Types.CharityCommission.Trustee;
    using SFA.DAS.ApplyService.Application.Apply.Roatp;
    using SFA.DAS.ApplyService.Domain.Apply;

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
        private Mock<IApplicationApiClient> _applicationApiClient;
        private Mock<IQnaApiClient> _qnaApiClient;

        private RoatpApplicationPreambleController _controller;

        private CompaniesHouseSummary _activeCompany;
        private ApiResponse<Charity> _activeCharity;

        private Contact _user;
        private ApplicationDetails _applicationDetails;
        private CreateOrganisationRequest _expectedRequest;

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
            _applicationApiClient = new Mock<IApplicationApiClient>();
            _qnaApiClient = new Mock<IQnaApiClient>();

            _controller = new RoatpApplicationPreambleController(_logger.Object, _roatpApiClient.Object,
                _ukrlpApiClient.Object,
                _sessionService.Object, _companiesHouseApiClient.Object,
                _charityCommissionApiClient.Object,
                _organisationApiClient.Object,
                _usersApiClient.Object,
                _applicationApiClient.Object,
                _qnaApiClient.Object);

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

            _activeCharity = new ApiResponse<Charity>
            {
                Success = true,
                Response = new Charity
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
                }
            };
            
            _applicationDetails = new ApplicationDetails
            {
                CompanySummary = null,
                CharitySummary = null,
                UKPRN = 10001234,
                UkrlpLookupDetails = new ProviderDetails
                {
                    ProviderName = "Provider Name",
                    UKPRN = "10001234",
                    ProviderAliases = new List<ProviderAlias>
                    {
                        new ProviderAlias
                        {
                            Alias = "Alias",
                            LastUpdated = DateTime.Now
                        }
                    },
                    ProviderStatus = "Active",
                    VerificationDate = new DateTime(2019, 01, 01),
                    ContactDetails = new List<ProviderContact>
                    {
                        new ProviderContact
                        {
                            ContactType = "L",
                            ContactEmail = "test@test.com",
                            LastUpdated = new DateTime(2019, 01, 01),
                            ContactTelephone1 = "01234 567890",
                            ContactWebsiteAddress = "www.test.com",
                            ContactAddress = new ContactAddress
                            {
                                Address1 = "Address 1",
                                Address2 = "Address 2",
                                Address3 = "Address 3",
                                Address4 = "Address 4",
                                Town = "Town",
                                PostCode = "TS1 1ST"
                            }
                        }
                    }
                },
                RoatpRegisterStatus = new OrganisationRegisterStatus
                {
                    UkprnOnRegister = false
                }
            };

            _applicationApiClient.Setup(x => x.GetExistingApplicationStatus(It.IsAny<string>())).ReturnsAsync(new List<RoatpApplicationStatus>());

            _roatpApiClient.Setup(x => x.GetOrganisationRegisterStatus(It.IsAny<long>())).ReturnsAsync(_applicationDetails.RoatpRegisterStatus);

            var applicationRoutes = new List<ApplicationRoute>
            {
                new ApplicationRoute { Id = ApplicationRoute.MainProviderApplicationRoute, RouteName = "Main provider" },
                new ApplicationRoute { Id = ApplicationRoute.EmployerProviderApplicationRoute, RouteName = "Employer provider" },
                new ApplicationRoute { Id = ApplicationRoute.SupportingProviderApplicationRoute, RouteName = "Supporting provider" }
            };

            _roatpApiClient.Setup(x => x.GetApplicationRoutes()).ReturnsAsync(applicationRoutes);
            
            _user = new Contact {Id = Guid.NewGuid()};

            var webUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "test name"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "test claim value"),
            }, "mock"));
            _controller.ControllerContext.HttpContext = new DefaultHttpContext {User = webUser};

            _expectedRequest = new CreateOrganisationRequest
            {
                CreatedBy = _user.Id,
                Name = _applicationDetails.UkrlpLookupDetails.ProviderName,
                OrganisationType = "TrainingProvider",
                OrganisationUkprn = Convert.ToInt32(_applicationDetails.UkrlpLookupDetails.UKPRN),
                RoATPApproved = false,
                RoEPAOApproved = false,
                PrimaryContactEmail = _applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactEmail,
                OrganisationDetails = new InternalApi.Types.OrganisationDetails
                {
                    ProviderName = _applicationDetails.UkrlpLookupDetails.ProviderName,
                    CompanyNumber = null,
                    Address1 = _applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactAddress.Address1,
                    Address2 = _applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactAddress.Address2,
                    Address3 = _applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactAddress.Address3,
                    City = _applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactAddress.Town,
                    Postcode = _applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactAddress.PostCode,
                    CharityCommissionDetails = null,
                    CompaniesHouseDetails = null,
                    CharityNumber = null,
                    FHADetails = new InternalApi.Types.FHADetails(
                    ),
                    LegalName = _applicationDetails.UkrlpLookupDetails.ProviderName,
                    TradingName = _applicationDetails.UkrlpLookupDetails.ProviderAliases.FirstOrDefault()?.Alias,
                    OrganisationReferenceId = _applicationDetails.UkrlpLookupDetails.UKPRN,
                    OrganisationReferenceType = "UKRLP",
                    UKRLPDetails = new InternalApi.Types.UKRLPDetails
                    {
                        UKPRN = _applicationDetails.UkrlpLookupDetails.UKPRN,
                        Alias = _applicationDetails.UkrlpLookupDetails.ProviderAliases.FirstOrDefault()?.Alias,
                        OrganisationName = _applicationDetails.UkrlpLookupDetails.ProviderName,
                        ContactNumber = _applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactTelephone1,
                        Website = _applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactWebsiteAddress,
                        PrimaryContactAddress = new ContactAddress
                        {
                            Address1 = _applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactAddress
                                .Address1,
                            Address2 = _applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactAddress
                                .Address2,
                            Address3 = _applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactAddress
                                .Address3,
                            Address4 = _applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactAddress
                                .Address4,
                            Town = _applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactAddress.Town,
                            PostCode = _applicationDetails.UkrlpLookupDetails.PrimaryContactDetails.ContactAddress
                                .PostCode
                        },
                        VerificationDetails = new List<VerificationDetails>()
                    }
                }
            };

            Mapper.Reset();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<CompaniesHouseSummaryProfile>();
                cfg.AddProfile<DirectorInformationProfile>();
                cfg.AddProfile<PersonSignificantControlInformationProfile>();
                cfg.AddProfile<CharityCommissionProfile>();
                cfg.AddProfile<CharityTrusteeProfile>();
                cfg.AddProfile<RoatpCreateOrganisationRequestProfile>();
                cfg.AddProfile<RoatpContactAddressProfile>();
            });

            Mapper.AssertConfigurationIsValid();
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
            var viewModel = viewResult.Model as SearchByUkprnViewModel;
            viewModel.ErrorMessages.Count.Should().Be(1);

        }

        [Test]
        public void UKPRN_is_not_found_on_UKRLP()
        {
            var noResults = new UkrlpLookupResults {Success = true, Results = new List<ProviderDetails>()};

            _ukrlpApiClient.Setup(x => x.GetTrainingProviderByUkprn(It.IsAny<long>())).ReturnsAsync(noResults);

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
        public void UKPRN_is_found_on_UKRLP()
        {
            var matchingResult = new UkrlpLookupResults
            {
                Success = true,
                Results = new List<ProviderDetails>
                {
                    new ProviderDetails
                    {
                        UKPRN = "10001000",
                        ProviderName = "Test Provider"
                    }
                }
            };
            _ukrlpApiClient.Setup(x => x.GetTrainingProviderByUkprn(It.IsAny<long>())).ReturnsAsync(matchingResult);

            _sessionService.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<ApplicationDetails>()));

            var model = new SearchByUkprnViewModel
            {
                UKPRN = "10001000"
            };

            var result = _controller.SearchByUkprn(model).GetAwaiter().GetResult();

            result.Should().BeOfType<RedirectToActionResult>();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("ConfirmOrganisation");
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
                UKPRN = 10001000,
                UkrlpLookupDetails = providerDetails
            };

            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);
            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>()))
                .Returns(Task.FromResult(_activeCompany)).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>())).Verifiable();

            var result = _controller.VerifyOrganisationDetails().GetAwaiter().GetResult();

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
                UKPRN = 10001000,
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>()))
                .Returns(Task.FromResult(_activeCompany)).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>()))
                .Returns(Task.FromResult(_activeCharity)).Verifiable();

            var result = _controller.VerifyOrganisationDetails().GetAwaiter().GetResult();

            _companiesHouseApiClient.Verify(x => x.GetCompanyDetails(It.IsAny<string>()), Times.Never);
            _charityCommissionApiClient.Verify(x => x.GetCharityDetails(It.IsAny<int>()), Times.Once);
        }

        [TestCase("SC123456")]
        [TestCase("NI123123")]
        public void
            UKPRN_is_verified_against_charity_commission_but_charity_number_not_for_england_and_wales_commission(
                string charityNumber)
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
                        VerificationId = charityNumber
                    }
                }
            };

            var applicationDetails = new ApplicationDetails
            {
                UKPRN = 10001000,
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            _sessionService.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<ApplicationDetails>())).Verifiable();

            _activeCharity.Response.CharityNumber = charityNumber;

            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>()))
                .Returns(Task.FromResult(_activeCompany)).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>()))
                .Returns(Task.FromResult(_activeCharity)).Verifiable();

            var result = _controller.VerifyOrganisationDetails().GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("SelectApplicationRoute");

            _companiesHouseApiClient.Verify(x => x.GetCompanyDetails(It.IsAny<string>()), Times.Never);
            _charityCommissionApiClient.Verify(x => x.GetCharityDetails(It.IsAny<int>()), Times.Never);
            _sessionService.Verify(
                x => x.Set(It.IsAny<string>(),
                    It.Is<ApplicationDetails>(y => y.CharitySummary.TrusteeManualEntryRequired == true)), Times.Once);
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
                UKPRN = 10001000,
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>()))
                .Returns(Task.FromResult(_activeCompany)).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>()))
                .Returns(Task.FromResult(_activeCharity)).Verifiable();

            var result = _controller.VerifyOrganisationDetails().GetAwaiter().GetResult();

            _companiesHouseApiClient.Verify(x => x.GetCompanyDetails(It.IsAny<string>()), Times.Once);
            _charityCommissionApiClient.Verify(x => x.GetCharityDetails(It.IsAny<int>()), Times.Once);
        }

        [TestCase("liquidation")]
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
                UKPRN = 10001000,
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            var inactiveCompany = new CompaniesHouseSummary
            {
                Status = status,
                CompanyNumber = "12345678"
            };
            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>()))
                .Returns(Task.FromResult(inactiveCompany)).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>())).Verifiable();

            var result = _controller.VerifyOrganisationDetails().GetAwaiter().GetResult();
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("CompanyNotFound");

            _companiesHouseApiClient.Verify(x => x.GetCompanyDetails(It.IsAny<string>()), Times.Once);
            _charityCommissionApiClient.Verify(x => x.GetCharityDetails(It.IsAny<int>()), Times.Never);
        }

        [TestCase(null)]
        [TestCase("")]
        public void UKPRN_is_verified_against_companies_house_but_company_status_not_supplied(string status)
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
                UKPRN = 10001000,
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            var inactiveCompany = new CompaniesHouseSummary
            {
                Status = status,
                CompanyNumber = "12345678"
            };
            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>()))
                .Returns(Task.FromResult(inactiveCompany)).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>())).Verifiable();

            var result = _controller.VerifyOrganisationDetails().GetAwaiter().GetResult();
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("SelectApplicationRoute");

            _companiesHouseApiClient.Verify(x => x.GetCompanyDetails(It.IsAny<string>()), Times.Once);
            _charityCommissionApiClient.Verify(x => x.GetCharityDetails(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void UKPRN_is_verified_against_companies_house_but_company_not_found()
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
                UKPRN = 10001000,
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            var companyNotFound = new CompaniesHouseSummary
            {
                Status = CompaniesHouseSummary.CompanyStatusNotFound,
                CompanyNumber = "12345678"
            };
            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>()))
                .Returns(Task.FromResult(companyNotFound)).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>())).Verifiable();

            var result = _controller.VerifyOrganisationDetails().GetAwaiter().GetResult();
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("CompanyNotFound");

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
                UKPRN = 10001000,
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            var inactiveCharity = new ApiResponse<Charity>()
            {
                Success = true,
                Response = new Charity
                {
                    Status = "removed",
                    DissolvedOn = new DateTime(2010, 1, 1)
                }
            };

            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>())).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>()))
                .Returns(Task.FromResult(inactiveCharity)).Verifiable();

            var result = _controller.VerifyOrganisationDetails().GetAwaiter().GetResult();
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("CharityNotFound");

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
                UKPRN = 10001000,
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>())).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>())).Verifiable();

            var result = _controller.VerifyOrganisationDetails().GetAwaiter().GetResult();
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
                UKPRN = 10001000,
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);
            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>()))
                .Returns(Task.FromResult(new CompaniesHouseSummary())).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>())).Verifiable();

            var result = _controller.VerifyOrganisationDetails().GetAwaiter().GetResult();

            _companiesHouseApiClient.Verify(x => x.GetCompanyDetails(It.IsAny<string>()), Times.Never);
            _charityCommissionApiClient.Verify(x => x.GetCharityDetails(It.IsAny<int>()), Times.Never);

            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("SelectApplicationRoute");
        }

        [Test]
        public void UKPRN_is_verified_against_companies_house_but_no_directors_or_pscs_returned()
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
            _activeCompany = new CompaniesHouseSummary
            {
                Status = "active",
                CompanyNumber = "12345678",
                Directors = new List<DirectorInformation>(),
                PersonsSignificationControl = new List<PersonSignificantControlInformation>(),
                IncorporationDate = new DateTime(2012, 1, 10),
                CompanyType = "ltd"
            };

            var applicationDetails = new ApplicationDetails
            {
                UKPRN = 10001000,
                UkrlpLookupDetails = providerDetails
            };

            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);
            _sessionService.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<ApplicationDetails>())).Verifiable();

            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>()))
                .Returns(Task.FromResult(_activeCompany)).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>())).Verifiable();

            var result = _controller.VerifyOrganisationDetails().GetAwaiter().GetResult();

            _companiesHouseApiClient.Verify(x => x.GetCompanyDetails(It.IsAny<string>()), Times.Once);
            _charityCommissionApiClient.Verify(x => x.GetCharityDetails(It.IsAny<int>()), Times.Never);

            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("SelectApplicationRoute");

            _sessionService.Verify(x => x.Set(It.IsAny<string>(),
                It.Is<ApplicationDetails>(y => y.CompanySummary.ManualEntryRequired == true)));
        }
  
        
        [Test]
        public void Application_details_confirmed_with_match_on_companies_house()
        {
            _applicationDetails.UkrlpLookupDetails.VerificationDetails = new List<VerificationDetails>
            {
                new VerificationDetails
                {
                    VerificationAuthority = "Companies House",
                    VerificationId = "12345678"
                }
            };
            _applicationDetails.CompanySummary = new CompaniesHouseSummary
            {
                CompanyNumber = "12345678",
                IncorporationDate = new DateTime(2012, 01, 01),
                CompanyType = "company_type",
                CompanyTypeDescriptions = new Dictionary<string, string>
                {
                    {"company_type", "Company Type Description"}
                },
                Status = "Active",
                Directors = new List<DirectorInformation>
                {
                    new DirectorInformation
                    {
                        AppointedDate = new DateTime(2012, 01, 01),
                        DateOfBirth = new DateTime(1970, 01, 01),
                        Id = "1",
                        Name = "Mr A Director",
                        ResignedDate = null
                    },
                    new DirectorInformation
                    {
                        AppointedDate = new DateTime(2014, 01, 01),
                        DateOfBirth = new DateTime(1968, 02, 02),
                        Id = "2",
                        Name = "Mr A Resigned",
                        ResignedDate = new DateTime(2018, 03, 03)
                    }
                },
                PersonsSignificationControl = new List<PersonSignificantControlInformation>
                {
                    new PersonSignificantControlInformation
                    {
                        CeasedDate = null,
                        DateOfBirth = new DateTime(1970, 01, 01),
                        Id = "1",
                        Name = "Mr A Director",
                        NotifiedDate = new DateTime(2019, 01, 01)
                    }
                }
            };

            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(_applicationDetails);

            _usersApiClient.Setup(x => x.GetUserBySignInId(It.IsAny<string>())).ReturnsAsync(_user);
            _usersApiClient.Setup(x => x.ApproveUser(It.IsAny<Guid>())).ReturnsAsync(true);

            _organisationApiClient.Setup(x => x.Create(It.IsAny<CreateOrganisationRequest>(), It.IsAny<Guid>()))
                .ReturnsAsync(new Organisation()).Verifiable();

            _expectedRequest.OrganisationDetails.CompanyNumber = _applicationDetails.CompanySummary.CompanyNumber;

            _expectedRequest.OrganisationDetails.UKRLPDetails.VerificationDetails.Add(new VerificationDetails
            {
                VerificationAuthority = "Companies House",
                VerificationId = "12345678"
            });
            _expectedRequest.OrganisationDetails.CompaniesHouseDetails = new InternalApi.Types.CompaniesHouseDetails
            {
                IncorporationDate = _applicationDetails.CompanySummary.IncorporationDate,
                CompanyType = _applicationDetails.CompanySummary.CompanyType,
                Directors = new List<DirectorInformation>
                {
                    new DirectorInformation
                    {
                        AppointedDate = new DateTime(2012, 01, 01),
                        DateOfBirth = new DateTime(1970, 01, 01),
                        Id = "1",
                        Name = "Mr A Director",
                        ResignedDate = null
                    },
                    new DirectorInformation
                    {
                        AppointedDate = new DateTime(2014, 01, 01),
                        DateOfBirth = new DateTime(1968, 02, 02),
                        Id = "2",
                        Name = "Mr A Resigned",
                        ResignedDate = new DateTime(2018, 03, 03)
                    }
                },
                PersonsSignificationControl = new List<PersonSignificantControlInformation>
                {
                    new PersonSignificantControlInformation
                    {
                        CeasedDate = null,
                        DateOfBirth = new DateTime(1970, 01, 01),
                        Id = "1",
                        Name = "Mr A Director",
                        NotifiedDate = new DateTime(2019, 01, 01)
                    }
                }
            };

            var applicationRouteModel = new SelectApplicationRouteViewModel
            {
                ApplicationRouteId = ApplicationRoute.MainProviderApplicationRoute
            };

            var result = _controller.StartApplication(applicationRouteModel).GetAwaiter().GetResult();

            _organisationApiClient.Verify(x =>
                x.Create(It.Is<CreateOrganisationRequest>(y => y.OrganisationUkprn == _expectedRequest.OrganisationUkprn
                                                               && y.OrganisationDetails.CompaniesHouseDetails
                                                                   .IncorporationDate
                                                               == _expectedRequest.OrganisationDetails
                                                                   .CompaniesHouseDetails.IncorporationDate),
                    It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void Application_details_confirmed_with_match_on_charity_commission()
        {
            _applicationDetails.UkrlpLookupDetails.VerificationDetails = new List<VerificationDetails>
            {
                new VerificationDetails
                {
                    VerificationAuthority = "Charity Commission",
                    VerificationId = "12345678"
                }
            };
            _applicationDetails.CharitySummary = new CharityCommissionSummary
            {
                CharityNumber = "12345678",
                IncorporatedOn = new DateTime(2006, 01, 02),
                Trustees = new List<Domain.CharityCommission.Trustee>
                {
                    new Domain.CharityCommission.Trustee
                    {
                        Id = "1",
                        Name = "Mr A Trustee"
                    },
                    new Domain.CharityCommission.Trustee
                    {
                        Id = "2",
                        Name = "Mrs B Trustworthy"
                    }
                }
            };

            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(_applicationDetails);

            _usersApiClient.Setup(x => x.GetUserBySignInId(It.IsAny<string>())).ReturnsAsync(_user);
            _usersApiClient.Setup(x => x.ApproveUser(It.IsAny<Guid>())).ReturnsAsync(true);

            _organisationApiClient.Setup(x => x.Create(It.IsAny<CreateOrganisationRequest>(), It.IsAny<Guid>()))
                .ReturnsAsync(new Organisation()).Verifiable();

            _expectedRequest.OrganisationDetails.CharityNumber = _applicationDetails.CharitySummary.CharityNumber;

            _expectedRequest.OrganisationDetails.UKRLPDetails.VerificationDetails.Add(new VerificationDetails
            {
                VerificationAuthority = "Charity Commission",
                VerificationId = "12345678"
            });
            _expectedRequest.OrganisationDetails.CharityCommissionDetails =
                new InternalApi.Types.CharityCommissionDetails
                {
                    RegistrationDate = new DateTime(2006, 01, 02),
                    Trustees = new List<Domain.CharityCommission.Trustee>
                    {
                        new Domain.CharityCommission.Trustee
                        {
                            Id = "1",
                            Name = "Mr A Trustee"
                        },
                        new Domain.CharityCommission.Trustee
                        {
                            Id = "2",
                            Name = "Mrs B Trustworthy"
                        }
                    }
                };

            var applicationRouteModel = new SelectApplicationRouteViewModel
            {
                ApplicationRouteId = ApplicationRoute.SupportingProviderApplicationRoute
            };

            var result = _controller.StartApplication(applicationRouteModel).GetAwaiter().GetResult();

            _organisationApiClient.Verify(x =>
                x.Create(It.Is<CreateOrganisationRequest>(y => y.OrganisationUkprn == _expectedRequest.OrganisationUkprn
                                                               && y.OrganisationDetails.CharityCommissionDetails
                                                                   .RegistrationDate
                                                               == _expectedRequest.OrganisationDetails
                                                                   .CharityCommissionDetails.RegistrationDate),
                    It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void Application_details_confirmed_with_no_recognised_verification_source()
        {
            _applicationDetails.UkrlpLookupDetails.VerificationDetails = new List<VerificationDetails>
            {
                new VerificationDetails
                {
                    VerificationAuthority = "National Audit Office",
                    VerificationId = "12345678"
                }
            };

            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(_applicationDetails);

            _usersApiClient.Setup(x => x.GetUserBySignInId(It.IsAny<string>())).ReturnsAsync(_user);
            _usersApiClient.Setup(x => x.ApproveUser(It.IsAny<Guid>())).ReturnsAsync(true);

            _organisationApiClient.Setup(x => x.Create(It.IsAny<CreateOrganisationRequest>(), It.IsAny<Guid>()))
                .ReturnsAsync(new Organisation()).Verifiable();

            _expectedRequest.OrganisationDetails.UKRLPDetails.VerificationDetails.Add(new VerificationDetails
            {
                VerificationAuthority = "National Audit Office",
                VerificationId = "12345678"
            });

            var applicationRouteModel = new SelectApplicationRouteViewModel
            {
                ApplicationRouteId = ApplicationRoute.MainProviderApplicationRoute
            };

            var result = _controller.StartApplication(applicationRouteModel).GetAwaiter().GetResult();

            _organisationApiClient.Verify(x =>
                x.Create(It.Is<CreateOrganisationRequest>(y => y.OrganisationUkprn == _expectedRequest.OrganisationUkprn
                                                               && y.OrganisationDetails.CompaniesHouseDetails == null
                                                               && y.OrganisationDetails.CharityCommissionDetails ==
                                                               null), It.IsAny<Guid>()), Times.Once);
        }
        
        [Test]
        public void Provider_asked_to_confirm_levy_status_if_choose_employer_application_route()
        {
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(_applicationDetails);

            var model = new SelectApplicationRouteViewModel
            {
                ApplicationRouteId = ApplicationRoute.EmployerProviderApplicationRoute
            };

            var result = _controller.StartApplication(model).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ConfirmLevyStatus");
        }

        [Test]
        public void Provider_routed_to_confirmation_page_if_non_levy_employer()
        {
            var model = new EmployerLevyStatusViewModel
            {
                ApplicationRouteId = ApplicationRoute.EmployerProviderApplicationRoute,
                LevyPayingEmployer = "N",
                UKPRN = "10001234"
            };

            var applicationDetails = new ApplicationDetails
            {
                UKPRN = 10001000,
                ApplicationRoute = new ApplicationRoute { Id = ApplicationRoute.EmployerProviderApplicationRoute }
            };

            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);
            var result = _controller.SubmitLevyStatus(model).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("IneligibleNonLevy");
        }

        [Test]
        public void Provider_routed_to_task_list_if_levy_paying_employer()
        {
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(_applicationDetails);

            _usersApiClient.Setup(x => x.GetUserBySignInId(It.IsAny<string>())).ReturnsAsync(_user);
            _usersApiClient.Setup(x => x.ApproveUser(It.IsAny<Guid>())).ReturnsAsync(true);

            _organisationApiClient.Setup(x => x.Create(It.IsAny<CreateOrganisationRequest>(), It.IsAny<Guid>()))
                .ReturnsAsync(new Organisation()).Verifiable();

            _expectedRequest.OrganisationDetails.UKRLPDetails.VerificationDetails.Add(new VerificationDetails
            {
                VerificationAuthority = "National Audit Office",
                VerificationId = "12345678"
            });

            var model = new EmployerLevyStatusViewModel
            {
                ApplicationRouteId = ApplicationRoute.EmployerProviderApplicationRoute,
                LevyPayingEmployer = "Y",
                UKPRN = "10001234"
            };

            _applicationDetails.ApplicationRoute = new ApplicationRoute { Id = ApplicationRoute.EmployerProviderApplicationRoute };
            
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(_applicationDetails);

            var result = _controller.SubmitLevyStatus(model).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("Applications");
            redirectResult.ControllerName.Should().Be("RoatpApplication");
        }

        [Test]
        public void Provider_asked_to_choose_application_route_again_if_non_levy_and_want_to_continue_with_application()
        {
            var model = new EmployerProviderContinueApplicationViewModel
            {
                ContinueWithApplication = "Y"
            };

            var result = _controller.ConfirmNonLevyContinue(model);

            var redirectResult = result as RedirectToActionResult;

			redirectResult.ActionName.Should().Be("SelectApplicationRoute");
        }

        [TestCase(OrganisationStatus.Active)]
        [TestCase(OrganisationStatus.ActiveNotTakingOnApprentices)]
        [TestCase(OrganisationStatus.Onboarding)]
        public void UKPRN_is_already_active_on_register(int statusId)
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
                UKPRN = 10001000,
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>()))
                .Returns(Task.FromResult(_activeCompany)).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>()))
                .Returns(Task.FromResult(_activeCharity)).Verifiable();

            var registerStatus = new OrganisationRegisterStatus
            {
                OrganisationId = Guid.NewGuid(),
                ProviderTypeId = ApplicationRoute.MainProviderApplicationRoute,
                RemovedReasonId = null,
                StatusDate = new DateTime(2018, 1, 2),
                StatusId = statusId,
                UkprnOnRegister = true
            };

            _roatpApiClient.Setup(x => x.GetOrganisationRegisterStatus(It.IsAny<long>())).ReturnsAsync(registerStatus);

            var result = _controller.VerifyOrganisationDetails().GetAwaiter().GetResult();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ProviderAlreadyOnRegister");

        }

        [TestCase(OrganisationStatus.Removed)]
        public void UKPRN_is_already_on_register_but_was_removed(int statusId)
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
                UKPRN = 10001000,
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>()))
                .Returns(Task.FromResult(_activeCompany)).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>()))
                .Returns(Task.FromResult(_activeCharity)).Verifiable();

            var registerStatus = new OrganisationRegisterStatus
            {
                OrganisationId = Guid.NewGuid(),
                ProviderTypeId = ApplicationRoute.MainProviderApplicationRoute,
                RemovedReasonId = RemovedReason.MinimumStandardsNotMet,
                StatusDate = new DateTime(2018, 1, 2),
                StatusId = statusId,
                UkprnOnRegister = true
            };

            _roatpApiClient.Setup(x => x.GetOrganisationRegisterStatus(It.IsAny<long>())).ReturnsAsync(registerStatus);

            var result = _controller.VerifyOrganisationDetails().GetAwaiter().GetResult();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("SelectApplicationRoute");

        }

        [Test]
        public void Application_in_progress_for_UKPRN()
        {
            var providerDetails = new ProviderDetails
            {
                UKPRN = "10001000"
            };

            var applicationDetails = new ApplicationDetails
            {
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            var existingApplicationStatuses = new List<RoatpApplicationStatus>
            {
                new RoatpApplicationStatus
                {
                    ApplicationId = Guid.NewGuid(),
                    Status = ApplicationStatus.InProgress
                }
            };
            _applicationApiClient.Setup(x => x.GetExistingApplicationStatus(It.IsAny<string>())).ReturnsAsync(existingApplicationStatuses);

            var result = _controller.VerifyOrganisationDetails().GetAwaiter().GetResult();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ApplicationInProgress");
        }

        [Test]
        public void Application_already_submitted_for_UKPRN()
        {
            var providerDetails = new ProviderDetails
            {
                UKPRN = "10001000"
            };

            var applicationDetails = new ApplicationDetails
            {
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            var existingApplicationStatuses = new List<RoatpApplicationStatus>
            {
                new RoatpApplicationStatus
                {
                    ApplicationId = Guid.NewGuid(),
                    Status = ApplicationStatus.Submitted
                }
            };
            _applicationApiClient.Setup(x => x.GetExistingApplicationStatus(It.IsAny<string>())).ReturnsAsync(existingApplicationStatuses);

            var result = _controller.VerifyOrganisationDetails().GetAwaiter().GetResult();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ApplicationPreviouslySubmitted");
        }

        [Test]
        public void Application_previously_submitted_and_a_new_application_in_progress_for_UKPRN()
        {
            var providerDetails = new ProviderDetails
            {
                UKPRN = "10001000"
            };

            var applicationDetails = new ApplicationDetails
            {
                UkrlpLookupDetails = providerDetails
            };
            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            var existingApplicationStatuses = new List<RoatpApplicationStatus>
            {
                new RoatpApplicationStatus
                {
                    ApplicationId = Guid.NewGuid(),
                    Status = ApplicationStatus.Submitted
                },
                new RoatpApplicationStatus
                {
                    ApplicationId = Guid.NewGuid(),
                    Status = ApplicationStatus.InProgress
                }
            };
            _applicationApiClient.Setup(x => x.GetExistingApplicationStatus(It.IsAny<string>())).ReturnsAsync(existingApplicationStatuses);

            var result = _controller.VerifyOrganisationDetails().GetAwaiter().GetResult();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ApplicationInProgress");
        }


        [Test]
        public void Confirm_change_provider_route_selects_yes_and_redirected_to_change_route_page()
        {
            var model = new ConfirmChangeRouteViewModel { ApplicationId = Guid.NewGuid(), ConfirmChangeRoute = "Y" };

            var result = _controller.SubmitConfirmChangeRoute(model).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ChangeApplicationProviderRoute");
        }

        [Test]
        public void Confirm_change_provider_route_selects_no_and_redirected_to_task_list()
        {
            var model = new ConfirmChangeRouteViewModel { ApplicationId = Guid.NewGuid(), ConfirmChangeRoute = "N" };

            var result = _controller.SubmitConfirmChangeRoute(model).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("TaskList");
        }

        [TestCase(OrganisationStatus.Active)]
        [TestCase(OrganisationStatus.ActiveNotTakingOnApprentices)]
        [TestCase(OrganisationStatus.Onboarding)]
        public void Provider_already_on_register_changes_route_and_cannot_select_current_route(int statusId)
        {
            var registerStatus = new OrganisationRegisterStatus
            {
                OrganisationId = Guid.NewGuid(),
                ProviderTypeId = ApplicationRoute.MainProviderApplicationRoute,
                RemovedReasonId = null,
                StatusDate = new DateTime(2018, 1, 2),
                StatusId = statusId,
                UkprnOnRegister = true
            };

            _roatpApiClient.Setup(x => x.GetOrganisationRegisterStatus(It.IsAny<long>())).ReturnsAsync(registerStatus);

            var applicationDetails = new ApplicationDetails
            {
                UKPRN = 10001000,
                RoatpRegisterStatus = registerStatus            
            };

            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            var result = _controller.SelectApplicationRoute().GetAwaiter().GetResult();
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as SelectApplicationRouteViewModel;

            var currentApplicationRoute = model.ApplicationRoutes.FirstOrDefault(x => x.Id == registerStatus.ProviderTypeId);

            currentApplicationRoute.Should().BeNull();
        }

        [Test]
        public void Provider_already_on_register_but_previously_removed_can_select_any_route()
        {
            var registerStatus = new OrganisationRegisterStatus
            {
                OrganisationId = Guid.NewGuid(),
                ProviderTypeId = ApplicationRoute.MainProviderApplicationRoute,
                RemovedReasonId = RemovedReason.MinimumStandardsNotMet,
                StatusDate = new DateTime(2018, 1, 2),
                StatusId = OrganisationStatus.Removed,
                UkprnOnRegister = true
            };

            _roatpApiClient.Setup(x => x.GetOrganisationRegisterStatus(It.IsAny<long>())).ReturnsAsync(registerStatus);

            var applicationDetails = new ApplicationDetails
            {
                UKPRN = 10001000,
                RoatpRegisterStatus = registerStatus,
                ApplicationRoute = new ApplicationRoute {  Id = ApplicationRoute.MainProviderApplicationRoute }
            };

            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            var result = _controller.SelectApplicationRoute().GetAwaiter().GetResult();
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as SelectApplicationRouteViewModel;

            var currentApplicationRoute = model.ApplicationRoutes.FirstOrDefault(x => x.Id == registerStatus.ProviderTypeId);

            currentApplicationRoute.Should().NotBeNull();
        }

        [Test]
        public void Provider_already_on_register_opts_to_change_route()
        {
            var model = new ChangeProviderRouteViewModel { ChangeApplicationRoute = "Y" };

            var result = _controller.ChangeProviderRoute(model).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;

            redirectResult.Should().NotBeNull();

            redirectResult.ActionName.Should().Be("SelectApplicationRoute");
        }

        [Test]
        public void Provider_shown_shutter_page_if_non_levy_and_choose_not_to_continue_with_application()
        {
            var model = new EmployerProviderContinueApplicationViewModel
            {
                ContinueWithApplication = "N"
            };

            var result = _controller.ConfirmNonLevyContinue(model);

            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("NonLevyAbandonedApplication");
        }

		[Test]
        public void Provider_already_on_register_opts_to_stay_on_register_in_same_route()
        {
            var model = new ChangeProviderRouteViewModel { ChangeApplicationRoute = "N" };

            var result = _controller.ChangeProviderRoute(model).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;

            redirectResult.Should().NotBeNull();

            redirectResult.ActionName.Should().Be("ChosenToRemainOnRegister");
        }

        [Test]

        public void Change_UKPRN_clears_application_data_and_redirects_to_enter_ukprn()
        {
            var model = new ChangeUkprnViewModel { ApplicationId = Guid.NewGuid() };

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
                         .ReturnsAsync(new Answer { Value = "10001234" })
                         .Verifiable();

            _applicationApiClient.Setup(x => x.UpdateApplicationStatus(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(true).Verifiable();

            _sessionService.Setup(x => x.Remove(It.IsAny<string>())).Verifiable();

            var result = _controller.ConfirmChangeUkprn(model).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("EnterApplicationUkprn");

            _applicationApiClient.Verify(x => x.UpdateApplicationStatus(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once);
            _qnaApiClient.VerifyAll();
            _sessionService.VerifyAll();
        }

        [Test]
        public void Provider_changing_route_mid_application_not_currently_on_the_register_is_offered_all_available_routes()
        {
            var applicationId = Guid.NewGuid();
            var applyApplicationRouteId = ApplicationRoute.MainProviderApplicationRoute;
            var ukprn = 10001234;
            var ukprnAnswer = new Answer
            {
                Value = ukprn.ToString()
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UKPRN, It.IsAny<string>())).ReturnsAsync(ukprnAnswer);

            var applicationRouteAnswer = new Answer
            {
                Value = applyApplicationRouteId.ToString()
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.ProviderRoute, It.IsAny<string>())).ReturnsAsync(applicationRouteAnswer);
            
            var notOnRegisterStatus = new OrganisationRegisterStatus
            {
                UkprnOnRegister = false
            };
            _roatpApiClient.Setup(x => x.GetOrganisationRegisterStatus(It.IsAny<long>())).ReturnsAsync(notOnRegisterStatus);

            var result =_controller.ChangeApplicationProviderRoute(applicationId).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as SelectApplicationRouteViewModel;
            model.Should().NotBeNull();

            model.ApplicationId.Should().Be(applicationId);
            model.ApplicationRouteId.Should().Be(applyApplicationRouteId);
            model.ApplicationRoutes.Count().Should().Be(3);
        }

        [Test]
        public void Provider_changing_route_mid_application_currently_active_on_the_register_is_only_offered_other_provider_routes()
        {
            var applicationId = Guid.NewGuid();
            var applyApplicationRouteId = ApplicationRoute.MainProviderApplicationRoute;
            var ukprn = 10001234;
            var ukprnAnswer = new Answer
            {
                Value = ukprn.ToString()
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UKPRN, It.IsAny<string>())).ReturnsAsync(ukprnAnswer);

            var applicationRouteAnswer = new Answer
            {
                Value = applyApplicationRouteId.ToString()
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.ProviderRoute, It.IsAny<string>())).ReturnsAsync(applicationRouteAnswer);

            var existingProviderStatus = new OrganisationRegisterStatus
            {
                UkprnOnRegister = true,
                ProviderTypeId = ApplicationRoute.EmployerProviderApplicationRoute
            };
            _roatpApiClient.Setup(x => x.GetOrganisationRegisterStatus(It.IsAny<long>())).ReturnsAsync(existingProviderStatus);

            var result = _controller.ChangeApplicationProviderRoute(applicationId).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as SelectApplicationRouteViewModel;
            model.Should().NotBeNull();

            model.ApplicationId.Should().Be(applicationId);
            model.ApplicationRouteId.Should().Be(applyApplicationRouteId);
            model.ApplicationRoutes.Should().NotContain(route => route.Id == existingProviderStatus.ProviderTypeId);
        }

        [Test]
        public void Provider_changing_route_mid_application_currently_removed_on_the_register_is_offered_all_provider_routes()
        {
            var applicationId = Guid.NewGuid();
            var applyApplicationRouteId = ApplicationRoute.MainProviderApplicationRoute;
            var ukprn = 10001234;
            var ukprnAnswer = new Answer
            {
                Value = ukprn.ToString()
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UKPRN, It.IsAny<string>())).ReturnsAsync(ukprnAnswer);

            var applicationRouteAnswer = new Answer
            {
                Value = applyApplicationRouteId.ToString()
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.ProviderRoute, It.IsAny<string>())).ReturnsAsync(applicationRouteAnswer);

            var notOnRegisterStatus = new OrganisationRegisterStatus
            {
                UkprnOnRegister = true,
                StatusId = OrganisationStatus.Removed,
                RemovedReasonId = RemovedReason.Merger
            };
            _roatpApiClient.Setup(x => x.GetOrganisationRegisterStatus(It.IsAny<long>())).ReturnsAsync(notOnRegisterStatus);

            var result = _controller.ChangeApplicationProviderRoute(applicationId).GetAwaiter().GetResult();

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            var model = viewResult.Model as SelectApplicationRouteViewModel;
            model.Should().NotBeNull();

            model.ApplicationId.Should().Be(applicationId);
            model.ApplicationRouteId.Should().Be(applyApplicationRouteId);
            model.ApplicationRoutes.Count().Should().Be(3);
        }

        [TestCase(ApplicationRoute.MainProviderApplicationRoute)]
        [TestCase(ApplicationRoute.SupportingProviderApplicationRoute)]
        public void Provider_changing_route_to_main_or_supporting_is_directed_to_task_list_with_route_changed(int chosenApplicationRouteId)
        {
            var model = new SelectApplicationRouteViewModel
            {
                ApplicationId = Guid.NewGuid(),
                ApplicationRouteId = chosenApplicationRouteId
            };

            var preambleSection = new ApplicationSection { Id = Guid.NewGuid() };

            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.Preamble,
                                RoatpWorkflowSectionIds.Preamble)).ReturnsAsync(preambleSection).Verifiable();

            _qnaApiClient.Setup(x => x.UpdatePageAnswers(It.IsAny<Guid>(), preambleSection.Id, It.IsAny<string>(), It.IsAny<List<Answer>>()))
                               .ReturnsAsync(new ApplyService.Application.Apply.SetPageAnswersResponse
                               {
                                   ValidationPassed = true
                               }).Verifiable();

            var result = _controller.UpdateApplicationProviderRoute(model).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("TaskList");
            redirectResult.ControllerName.Should().Be("RoatpApplication");

            _qnaApiClient.VerifyAll();
        }
        
        [Test]
        public void Provider_changing_route_to_employer_is_asked_if_they_are_a_levy_paying_employer()
        {
            var model = new SelectApplicationRouteViewModel
            {
                ApplicationId = Guid.NewGuid(),
                ApplicationRouteId = ApplicationRoute.EmployerProviderApplicationRoute
            };

            var ukprnAnswer = new Answer
            {
                Value = "10001234"
            };

            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.UKPRN, It.IsAny<string>()))
                .ReturnsAsync(ukprnAnswer).Verifiable();
            
            var result = _controller.UpdateApplicationProviderRoute(model).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ConfirmLevyStatus");

            _qnaApiClient.VerifyAll();
        }

        [Test]
        public void Provider_changing_route_to_employer_is_directed_to_confirm_continue_page_if_not_levy_paying()
        {
            var model = new EmployerLevyStatusViewModel
            {
                ApplicationId = Guid.NewGuid(),
                UKPRN = "10001234",
                ApplicationRouteId = ApplicationRoute.EmployerProviderApplicationRoute,
                LevyPayingEmployer = "N"
            };

            var result = _controller.SubmitLevyStatus(model).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("IneligibleNonLevy");
        }

        [Test]
        public void Provider_changing_route_to_employer_is_directed_to_task_list_with_route_changed_if_levy_paying()
        {
            var model = new EmployerLevyStatusViewModel
            {
                ApplicationId = Guid.NewGuid(),
                UKPRN = "10001234",
                ApplicationRouteId = ApplicationRoute.EmployerProviderApplicationRoute,
                LevyPayingEmployer = "Y"
            };

            var preambleSection = new ApplicationSection { Id = Guid.NewGuid() };

            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(It.IsAny<Guid>(), RoatpWorkflowSequenceIds.Preamble,
                                RoatpWorkflowSectionIds.Preamble)).ReturnsAsync(preambleSection).Verifiable();

            _qnaApiClient.Setup(x => x.UpdatePageAnswers(It.IsAny<Guid>(), preambleSection.Id, It.IsAny<string>(), It.IsAny<List<Answer>>()))
                               .ReturnsAsync(new ApplyService.Application.Apply.SetPageAnswersResponse
                               {
                                   ValidationPassed = true
                               }).Verifiable();
            
            var result = _controller.SubmitLevyStatus(model).GetAwaiter().GetResult();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("TaskList");
            redirectResult.ControllerName.Should().Be("RoatpApplication");

            _qnaApiClient.VerifyAll();
        }

        [Test]
        public void Provider_ineligible_to_change_route_to_employer_but_wants_to_continue_with_application_is_presented_with_change_provider_route()
        {
            var model = new EmployerProviderContinueApplicationViewModel
            {
                ContinueWithApplication = "Y",
                ApplicationId = Guid.NewGuid()
            };

            var result = _controller.ConfirmNonLevyContinue(model);

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("ChangeApplicationProviderRoute");
        }

        [Test]
        public void Provider_ineligible_to_change_route_to_employer_is_directed_to_shutter_page()
        {
            var model = new EmployerProviderContinueApplicationViewModel
            {
                ContinueWithApplication = "N",
                ApplicationId = Guid.NewGuid()
            };

            var result = _controller.ConfirmNonLevyContinue(model);

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult.ActionName.Should().Be("NonLevyAbandonedApplication");
            redirectResult.ControllerName.Should().Be("RoatpShutterPages");
        }

    }

}