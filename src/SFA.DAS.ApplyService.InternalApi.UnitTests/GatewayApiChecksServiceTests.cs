using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.InternalApi.AutoMapper;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Models.Ukrlp;
using SFA.DAS.ApplyService.InternalApi.Services;
using SFA.DAS.ApplyService.InternalApi.Types;
using SFA.DAS.ApplyService.InternalApi.Types.CharityCommission;
using SFA.DAS.ApplyService.InternalApi.Types.CompaniesHouse;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class GatewayApiChecksServiceTests
    {
        private Mock<CompaniesHouseApiClient> _companiesHouseApiClient;
        private Mock<CharityCommissionApiClient> _charityCommissionApiClient;
        private Mock<IRoatpApiClient> _roatpApiClient;
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private Mock<ILogger<GatewayApiChecksService>> _logger;

        private GatewayApiChecksService _service;
        private string _ukprn;
        private Guid _applicationId;
        private UkprnLookupResponse _ukrlpDetails;
        private OrganisationRegisterStatus _registerStatus;

        [SetUp]
        public void Before_each_test()
        {
            Mapper.Reset();

            Mapper.Initialize(cfg =>
            {

                cfg.AddProfile<UkrlpCharityCommissionProfile>();
                cfg.AddProfile<UkrlpCompaniesHouseProfile>();
            });

            Mapper.AssertConfigurationIsValid();

            _ukprn = "10001234";
            _applicationId = Guid.NewGuid();
            _companiesHouseApiClient = new Mock<CompaniesHouseApiClient>();
            _charityCommissionApiClient = new Mock<CharityCommissionApiClient>();
            _roatpApiClient = new Mock<IRoatpApiClient>();
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _logger = new Mock<ILogger<GatewayApiChecksService>>();

            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.UKPRN)).ReturnsAsync(_ukprn);

            _ukrlpDetails = new UkprnLookupResponse 
            {
                Success = true,
                Results = new List<ProviderDetails>
                {
                    new ProviderDetails
                    {
                        UKPRN = _ukprn,
                        ProviderName = "Test Provider",
                        VerificationDetails = new List<VerificationDetails>
                        {
                            new VerificationDetails
                            {
                                PrimaryVerificationSource = true,
                                VerificationAuthority = "Government statute"
                            }
                        }
                    }
                }
            };

            _roatpApiClient.Setup(x => x.GetUkrlpDetails(_ukprn)).ReturnsAsync(_ukrlpDetails);

            _registerStatus = new OrganisationRegisterStatus
            {
                UkprnOnRegister = false
            };

            _roatpApiClient.Setup(x => x.GetOrganisationRegisterStatus(_ukprn)).ReturnsAsync(_registerStatus);

            _service = new GatewayApiChecksService(_companiesHouseApiClient.Object, _charityCommissionApiClient.Object,
                                                   _roatpApiClient.Object, _qnaApiClient.Object, _logger.Object);
        }

        [Test]
        public void Service_returns_ukrlp_and_roatp_details()
        {
            var result = _service.GetExternalApiCheckDetails(_applicationId, "test user").GetAwaiter().GetResult();

            result.SourcesCheckedOn.Should().NotBeNull();
            result.UkrlpDetails.UKPRN.Should().Be(_ukrlpDetails.Results[0].UKPRN);
            result.UkrlpDetails.ProviderName.Should().Be(_ukrlpDetails.Results[0].ProviderName);
            result.RoatpRegisterDetails.UkprnOnRegister.Should().Be(_registerStatus.UkprnOnRegister);
            result.CompaniesHouseDetails.Should().BeNull(); 
        }

        [Test]
        public void Service_returns_companies_house_details()
        {
            var companyNumber = "12345678";

            _ukrlpDetails.Results[0].VerificationDetails = new List<VerificationDetails>
            {
                new VerificationDetails
                {
                    PrimaryVerificationSource = true,
                    VerificationAuthority = Domain.Ukrlp.VerificationAuthorities.CompaniesHouseAuthority,
                    VerificationId = companyNumber
                }
            };
            _roatpApiClient.Setup(x => x.GetUkrlpDetails(_ukprn)).ReturnsAsync(_ukrlpDetails);

            var companyDetails = new ApiResponse<Company>
            {
                Success = true,
                Response = new Company
                {
                    CompanyNumber = companyNumber, 
                    CompanyName = "Test Name"
                }
            };

            _companiesHouseApiClient.Setup(x => x.GetCompany(companyNumber)).ReturnsAsync(companyDetails);

            var result = _service.GetExternalApiCheckDetails(_applicationId, "test user").GetAwaiter().GetResult();

            result.CompaniesHouseDetails.Should().NotBeNull();
            result.CompaniesHouseDetails.CompanyNumber.Should().Be(companyDetails.Response.CompanyNumber);
            result.CompaniesHouseDetails.CompanyName.Should().Be(companyDetails.Response.CompanyName);
        }

        [Test]
        public void Service_returns_charity_commission_details()
        {
            int charityNumber = 112233;

            _ukrlpDetails.Results[0].VerificationDetails = new List<VerificationDetails>
            {
                new VerificationDetails
                {
                    PrimaryVerificationSource = true,
                    VerificationAuthority = Domain.Ukrlp.VerificationAuthorities.CharityCommissionAuthority,
                    VerificationId = charityNumber.ToString()
                }
            };
            _roatpApiClient.Setup(x => x.GetUkrlpDetails(_ukprn)).ReturnsAsync(_ukrlpDetails);

            var charityDetails = new Charity
            {
                CharityNumber = charityNumber.ToString(),
                Name = "Test Name"                
            };

            _charityCommissionApiClient.Setup(x => x.GetCharity(charityNumber)).ReturnsAsync(charityDetails);

            var result = _service.GetExternalApiCheckDetails(_applicationId, "test user").GetAwaiter().GetResult();

            result.CharityCommissionDetails.Should().NotBeNull();
            result.CharityCommissionDetails.CharityNumber.Should().Be(charityDetails.CharityNumber);
            result.CharityCommissionDetails.CharityName.Should().Be(charityDetails.Name);
        }

        [Test]
        public void Service_throws_exception_if_no_ukrlp_details_returned()
        {
            _ukrlpDetails = null;
            _roatpApiClient.Setup(x => x.GetUkrlpDetails(_ukprn)).ReturnsAsync(_ukrlpDetails);

            Action serviceCall = () => _service.GetExternalApiCheckDetails(_applicationId, "test user").GetAwaiter().GetResult();
            serviceCall.Should().Throw<ServiceUnavailableException>();
        }

        [Test]
        public void Service_throws_exception_if_no_roatp_details_returned()
        {
            _registerStatus = null;
            _roatpApiClient.Setup(x => x.GetOrganisationRegisterStatus(_ukprn)).ReturnsAsync(_registerStatus);
            Action serviceCall = () => _service.GetExternalApiCheckDetails(_applicationId, "test user").GetAwaiter().GetResult();
            serviceCall.Should().Throw<ServiceUnavailableException>();
        }

        [Test]
        public void Service_throws_exception_if_no_companies_house_details_returned()
        {
            var companyNumber = "12345678";

            _ukrlpDetails.Results[0].VerificationDetails = new List<VerificationDetails>
            {
                new VerificationDetails
                {
                    PrimaryVerificationSource = true,
                    VerificationAuthority = Domain.Ukrlp.VerificationAuthorities.CompaniesHouseAuthority,
                    VerificationId = companyNumber
                }
            };
            _roatpApiClient.Setup(x => x.GetUkrlpDetails(_ukprn)).ReturnsAsync(_ukrlpDetails);

            ApiResponse<Company> apiResponse = null;
            _companiesHouseApiClient.Setup(x => x.GetCompany(companyNumber)).ReturnsAsync(apiResponse);

            Action serviceCall = () => _service.GetExternalApiCheckDetails(_applicationId, "test user").GetAwaiter().GetResult();
            serviceCall.Should().Throw<ServiceUnavailableException>();
        }

        [Test]
        public void Service_throws_exception_if_no_charity_commission_details_returned()
        {
            int charityNumber = 112233;

            _ukrlpDetails.Results[0].VerificationDetails = new List<VerificationDetails>
            {
                new VerificationDetails
                {
                    PrimaryVerificationSource = true,
                    VerificationAuthority = Domain.Ukrlp.VerificationAuthorities.CharityCommissionAuthority,
                    VerificationId = charityNumber.ToString()
                }
            };
            _roatpApiClient.Setup(x => x.GetUkrlpDetails(_ukprn)).ReturnsAsync(_ukrlpDetails);

            Charity charityDetails = null;
            _charityCommissionApiClient.Setup(x => x.GetCharity(charityNumber)).ReturnsAsync(charityDetails);

            Action serviceCall = () => _service.GetExternalApiCheckDetails(_applicationId, "test user").GetAwaiter().GetResult();
            serviceCall.Should().Throw<ServiceUnavailableException>();
        }
    }
}
