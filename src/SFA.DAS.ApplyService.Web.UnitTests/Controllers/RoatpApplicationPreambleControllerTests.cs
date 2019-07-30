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
    using Validators;
    using Trustee = InternalApi.Types.CharityCommission.Trustee;

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
        private Mock<IRoatpStatusValidator> _roatpStatusValidator;

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
            _roatpStatusValidator = new Mock<IRoatpStatusValidator>();
            _roatpStatusValidator.Setup(x => x.ProviderEligibleToJoinRegister(It.IsAny<OrganisationRegisterStatus>()))
                .Returns(true);

            _controller = new RoatpApplicationPreambleController(_logger.Object, _roatpApiClient.Object,
                _ukrlpApiClient.Object,
                _sessionService.Object, _companiesHouseApiClient.Object,
                _charityCommissionApiClient.Object,
                _organisationApiClient.Object,
                _usersApiClient.Object,
                _roatpStatusValidator.Object);
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
                }
            };

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
            _controller.ModelState.ErrorCount.Should().Be(1);

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

        [TestCase(1, 12)]
        [TestCase(2, 12)]
        [TestCase(3, 3)]
        [Ignore("Checking for charity history moved to later in the process")]
        public void UKPRN_is_verified_against_charity_commission_but_does_not_have_sufficient_history(
            int applicationRouteId, int monthsRequired)
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

            _activeCharity.Response.IncorporatedOn = DateTime.Today.AddMonths(-1 * monthsRequired).AddDays(1);

            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);
            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>()))
                .Returns(Task.FromResult(_activeCompany)).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>()))
                .Returns(Task.FromResult(_activeCharity)).Verifiable();

            var result = _controller.VerifyOrganisationDetails().GetAwaiter().GetResult();

            _companiesHouseApiClient.Verify(x => x.GetCompanyDetails(It.IsAny<string>()), Times.Never);
            _charityCommissionApiClient.Verify(x => x.GetCharityDetails(It.IsAny<int>()), Times.Once);

            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("InvalidCharityFormationHistory");
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

        [TestCase("IP123456", true)]
        [TestCase("SP123456", true)]
        [TestCase("IC123456", true)]
        [TestCase("SI123456", true)]
        [TestCase("NP123456", true)]
        [TestCase("NV123456", true)]
        [TestCase("RC123456", true)]
        [TestCase("SR123456", true)]
        [TestCase("NR123456", true)]
        [TestCase("NO123456", true)]
        [TestCase("SC123456", false)]
        [TestCase("12345678", false)]
        public void UKPRN_is_verified_against_companies_house_but_limited_information_returned(string companyNumber,
            bool expectedRequired)
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
                        VerificationId = companyNumber
                    }
                }
            };
            _activeCompany.CompanyNumber = companyNumber;

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
                It.Is<ApplicationDetails>(y => y.CompanySummary.ManualEntryRequired == expectedRequired)));

        }

        [TestCase(1, 12)]
        [TestCase(2, 12)]
        [TestCase(3, 3)]
        [Ignore("Checking for company history moved to later in the application process")]
        public void UKPRN_is_verified_against_companies_house_but_does_not_have_sufficient_trading_history(
            int applicationRouteId, int monthsRequired)
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

            _activeCompany.IncorporationDate = DateTime.Today.AddMonths(-1 * monthsRequired).AddDays(1);

            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(applicationDetails);

            _companiesHouseApiClient.Setup(x => x.GetCompanyDetails(It.IsAny<string>()))
                .Returns(Task.FromResult(_activeCompany)).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharityDetails(It.IsAny<int>())).Verifiable();

            var result = _controller.VerifyOrganisationDetails().GetAwaiter().GetResult();

            _companiesHouseApiClient.Verify(x => x.GetCompanyDetails(It.IsAny<string>()), Times.Once);
            _charityCommissionApiClient.Verify(x => x.GetCharityDetails(It.IsAny<int>()), Times.Never);

            result.Should().BeOfType<RedirectToActionResult>();
            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("InvalidCompanyTradingHistory");
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

            var result = _controller.StartApplication().GetAwaiter().GetResult();

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

            var result = _controller.StartApplication().GetAwaiter().GetResult();

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

            var result = _controller.StartApplication().GetAwaiter().GetResult();

            _organisationApiClient.Verify(x =>
                x.Create(It.Is<CreateOrganisationRequest>(y => y.OrganisationUkprn == _expectedRequest.OrganisationUkprn
                                                               && y.OrganisationDetails.CompaniesHouseDetails == null
                                                               && y.OrganisationDetails.CharityCommissionDetails ==
                                                               null), It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void UKPRN_is_found_on_UKRLP_but_not_eligible_to_join_register()
        {
            _applicationDetails.UkrlpLookupDetails.VerificationDetails = new List<VerificationDetails>
            {
                new VerificationDetails
                {
                    VerificationAuthority = "Additional Source",
                    VerificationId = "12345678"
                }
            };

            _sessionService.Setup(x => x.Get<ApplicationDetails>(It.IsAny<string>())).Returns(_applicationDetails);

            var registerStatus = new OrganisationRegisterStatus
            {
                ProviderTypeId = ApplicationRoute.MainProviderApplicationRoute,
                StatusId = OrganisationStatus.Removed,
                RemovedReasonId = RemovedReason.Breach,
                UkprnOnRegister = true
            };
            _roatpApiClient.Setup(x => x.GetOrganisationRegisterStatus(It.IsAny<long>())).ReturnsAsync(registerStatus);

            _roatpStatusValidator.Setup(x => x.ProviderEligibleToJoinRegister(It.IsAny<OrganisationRegisterStatus>()))
                .Returns(false);

            var model = new SearchByUkprnViewModel
            {
                UKPRN = "10001000"
            };

            var result = _controller.VerifyOrganisationDetails().GetAwaiter().GetResult();

            result.Should().BeOfType<RedirectToActionResult>();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.ActionName.Should().Be("IneligibleToJoin");
        }

    }
}