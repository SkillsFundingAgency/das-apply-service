using NUnit.Framework;
using AutoMapper;
using SFA.DAS.ApplyService.InternalApi.AutoMapper;
using SFA.DAS.ApplyService.Application.Apply;
using Moq;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Models.Ukrlp;
using SFA.DAS.ApplyService.Domain.Ukrlp;
using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.InternalApi.Types.CompaniesHouse;
using SFA.DAS.ApplyService.InternalApi.Types;
using SFA.DAS.ApplyService.InternalApi.Services;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class GatewayApiChecksControllerTests
    {
        private Mock<IApplyRepository> _applyRepository;
        private Mock<ILogger<GatewayApiChecksController>> _logger;
        private Mock<CompaniesHouseApiClient> _companiesHouseApiClient;
        private Mock<CharityCommissionApiClient> _charityCommissionApiClient;
        private Mock<RoatpApiClient> _roatpApiClient;
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private IGatewayApiChecksService _gatewayApiChecksService;

        private GatewayApiChecksController _controller;

        [OneTimeSetUp]
        public void Before_all_tests()
        {
            Mapper.Reset();

            Mapper.Initialize(cfg =>
            {

                cfg.AddProfile<UkrlpCharityCommissionProfile>();
                cfg.AddProfile<UkrlpCompaniesHouseProfile>();
            });

            Mapper.AssertConfigurationIsValid();
        }

        [SetUp]
        public void Before_each_test()
        {
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.GetConfig()).ReturnsAsync(new ApplyConfig { RoatpApiAuthentication = new RoatpApiAuthentication { ApiBaseAddress = "https://localhost"} });

            var roatpTokenService = new Mock<IRoatpTokenService>();
            roatpTokenService.Setup(x => x.GetToken()).Returns(string.Empty);

            _applyRepository = new Mock<IApplyRepository>();
            _logger = new Mock<ILogger<GatewayApiChecksController>>();
            _companiesHouseApiClient = new Mock<CompaniesHouseApiClient>();
            _charityCommissionApiClient = new Mock<CharityCommissionApiClient>();
            _roatpApiClient = new Mock<RoatpApiClient>(Mock.Of<ILogger<RoatpApiClient>>(), configurationService.Object, roatpTokenService.Object);
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _gatewayApiChecksService = new GatewayApiChecksService(_companiesHouseApiClient.Object, _charityCommissionApiClient.Object,
                                                                   _roatpApiClient.Object, _qnaApiClient.Object);

            _controller = new GatewayApiChecksController(_applyRepository.Object, _logger.Object, _gatewayApiChecksService);
        }

        [Test]
        public void External_api_checks_for_organisation_that_is_not_a_company_or_a_charity()
        {
            var ukrlpDetails = new UkprnLookupResponse
            {
                Success = true,
                Results = new System.Collections.Generic.List<Models.Ukrlp.ProviderDetails>
                {
                    new Models.Ukrlp.ProviderDetails
                    {
                        ProviderName = "Provider",
                        UKPRN = "10001234",                     
                        VerificationDetails = new System.Collections.Generic.List<Models.Ukrlp.VerificationDetails>
                        {
                            new Models.Ukrlp.VerificationDetails
                            {
                                PrimaryVerificationSource = true,
                                VerificationAuthority = VerificationAuthorities.SoleTraderPartnershipAuthority,
                                VerificationId = "123"
                            }
                        }
                    }
                }
            };

            _roatpApiClient.Setup(x => x.GetUkrlpDetails(It.IsAny<string>())).ReturnsAsync(ukrlpDetails);

            var registerStatus = new OrganisationRegisterStatus
            {
                UkprnOnRegister = false
            };
            _roatpApiClient.Setup(x => x.GetOrganisationRegisterStatus(It.IsAny<string>())).ReturnsAsync(registerStatus);


            var applyData = new ApplyData
            {
                ApplyDetails = new ApplyDetails
                {
                    UKPRN = "10001234"
                }
            };

            _applyRepository.Setup(x => x.GetApplyData(It.IsAny<Guid>())).ReturnsAsync(applyData);
            _applyRepository.Setup(x => x.UpdateApplyData(It.IsAny<Guid>(), It.IsAny<ApplyData>(), It.IsAny<string>())).ReturnsAsync(true);

            _companiesHouseApiClient.Setup(x => x.GetCompany(It.IsAny<string>())).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharity(It.IsAny<int>())).Verifiable();

            var result = _controller.ExternalApiChecks(Guid.NewGuid(), "GatewayAssessor").GetAwaiter().GetResult();

            var response = result as OkObjectResult;
            response.Should().NotBeNull();
            var externalApiData = response.Value as ApplyGatewayDetails;
            externalApiData.Should().NotBeNull();

            externalApiData.CharityCommissionDetails.Should().BeNull();
            externalApiData.CompaniesHouseDetails.Should().BeNull();
            externalApiData.RoatpRegisterDetails.UkprnOnRegister.Should().BeFalse();
            externalApiData.UkrlpDetails.ProviderName.Should().Be(ukrlpDetails.Results[0].ProviderName);

            _companiesHouseApiClient.Verify(x => x.GetCompany(It.IsAny<string>()), Times.Never);
            _charityCommissionApiClient.Verify(x => x.GetCharity(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void External_api_checks_for_organisation_that_is_a_company()
        {
            var ukrlpDetails = new UkprnLookupResponse
            {
                Success = true,
                Results = new System.Collections.Generic.List<Models.Ukrlp.ProviderDetails>
                {
                    new Models.Ukrlp.ProviderDetails
                    {
                        ProviderName = "Provider",
                        UKPRN = "10001234",
                        VerificationDetails = new System.Collections.Generic.List<Models.Ukrlp.VerificationDetails>
                        {
                            new Models.Ukrlp.VerificationDetails
                            {
                                PrimaryVerificationSource = true,
                                VerificationAuthority = VerificationAuthorities.CompaniesHouseAuthority,
                                VerificationId = "12345678"
                            }
                        }
                    }
                }
            };

            _roatpApiClient.Setup(x => x.GetUkrlpDetails(It.IsAny<string>())).ReturnsAsync(ukrlpDetails);

            var registerStatus = new OrganisationRegisterStatus
            {
                UkprnOnRegister = false
            };
            _roatpApiClient.Setup(x => x.GetOrganisationRegisterStatus(It.IsAny<string>())).ReturnsAsync(registerStatus);


            var applyData = new ApplyData
            {
                ApplyDetails = new ApplyDetails
                {
                    UKPRN = "10001234"
                }
            };

            _applyRepository.Setup(x => x.GetApplyData(It.IsAny<Guid>())).ReturnsAsync(applyData);
            _applyRepository.Setup(x => x.UpdateApplyData(It.IsAny<Guid>(), It.IsAny<ApplyData>(), It.IsAny<string>())).ReturnsAsync(true);

            var companyDetails = new ApiResponse<Company> 
            {   
                Success = true,
                Response = new Company
                {
                    CompanyName = "My Ltd",
                    CompanyNumber = "12345678"
                }
            };
            _companiesHouseApiClient.Setup(x => x.GetCompany(It.IsAny<string>())).ReturnsAsync(companyDetails).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharity(It.IsAny<int>())).Verifiable();

            var result = _controller.ExternalApiChecks(Guid.NewGuid(), "GatewayAssessor").GetAwaiter().GetResult();

            var response = result as OkObjectResult;
            response.Should().NotBeNull();
            var externalApiData = response.Value as ApplyGatewayDetails;
            externalApiData.Should().NotBeNull();

            externalApiData.CharityCommissionDetails.Should().BeNull();
            externalApiData.CompaniesHouseDetails.Should().NotBeNull();
            externalApiData.CompaniesHouseDetails.CompanyName.Should().Be(companyDetails.Response.CompanyName);
            externalApiData.CompaniesHouseDetails.CompanyNumber.Should().Be(companyDetails.Response.CompanyNumber);
            externalApiData.RoatpRegisterDetails.UkprnOnRegister.Should().BeFalse();
            externalApiData.UkrlpDetails.ProviderName.Should().Be(ukrlpDetails.Results[0].ProviderName);

            _companiesHouseApiClient.Verify(x => x.GetCompany(It.IsAny<string>()), Times.Once);
            _charityCommissionApiClient.Verify(x => x.GetCharity(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void External_api_checks_for_organisation_that_is_a_charity()
        {
            var ukrlpDetails = new UkprnLookupResponse
            {
                Success = true,
                Results = new System.Collections.Generic.List<Models.Ukrlp.ProviderDetails>
                {
                    new Models.Ukrlp.ProviderDetails
                    {
                        ProviderName = "Provider",
                        UKPRN = "10001234",
                        VerificationDetails = new System.Collections.Generic.List<Models.Ukrlp.VerificationDetails>
                        {
                            new Models.Ukrlp.VerificationDetails
                            {
                                PrimaryVerificationSource = true,
                                VerificationAuthority = VerificationAuthorities.CharityCommissionAuthority,
                                VerificationId = "12345678"
                            }
                        }
                    }
                }
            };

            _roatpApiClient.Setup(x => x.GetUkrlpDetails(It.IsAny<string>())).ReturnsAsync(ukrlpDetails);

            var registerStatus = new OrganisationRegisterStatus
            {
                UkprnOnRegister = false
            };
            _roatpApiClient.Setup(x => x.GetOrganisationRegisterStatus(It.IsAny<string>())).ReturnsAsync(registerStatus);


            var applyData = new ApplyData
            {
                ApplyDetails = new ApplyDetails
                {
                    UKPRN = "10001234"
                }
            };

            _applyRepository.Setup(x => x.GetApplyData(It.IsAny<Guid>())).ReturnsAsync(applyData);
            _applyRepository.Setup(x => x.UpdateApplyData(It.IsAny<Guid>(), It.IsAny<ApplyData>(), It.IsAny<string>())).ReturnsAsync(true);

            var charityDetails = new Types.CharityCommission.Charity
            {
                Name = "My charity",
                CharityNumber = "12345678",
                Status = "Active",
                Type = "type of company"
            };

            _companiesHouseApiClient.Setup(x => x.GetCompany(It.IsAny<string>())).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharity(It.IsAny<int>())).ReturnsAsync(charityDetails).Verifiable();

            var result = _controller.ExternalApiChecks(Guid.NewGuid(), "GatewayAssessor").GetAwaiter().GetResult();

            var response = result as OkObjectResult;
            response.Should().NotBeNull();
            var externalApiData = response.Value as ApplyGatewayDetails;
            externalApiData.Should().NotBeNull();

            externalApiData.CharityCommissionDetails.Should().NotBeNull();
            externalApiData.CharityCommissionDetails.CharityName.Should().Be(charityDetails.Name);
            externalApiData.CharityCommissionDetails.CharityNumber.Should().Be(charityDetails.CharityNumber);
            externalApiData.CharityCommissionDetails.Status.Should().Be(charityDetails.Status);
            externalApiData.CharityCommissionDetails.Type.Should().Be(charityDetails.Type);

            externalApiData.CompaniesHouseDetails.Should().BeNull();
            externalApiData.RoatpRegisterDetails.UkprnOnRegister.Should().BeFalse();
            externalApiData.UkrlpDetails.ProviderName.Should().Be(ukrlpDetails.Results[0].ProviderName);

            _companiesHouseApiClient.Verify(x => x.GetCompany(It.IsAny<string>()), Times.Never);
            _charityCommissionApiClient.Verify(x => x.GetCharity(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void External_api_checks_for_organisation_that_is_a_company_and_a_charity()
        {
            var ukrlpDetails = new UkprnLookupResponse
            {
                Success = true,
                Results = new System.Collections.Generic.List<Models.Ukrlp.ProviderDetails>
                {
                    new Models.Ukrlp.ProviderDetails
                    {
                        ProviderName = "Provider",
                        UKPRN = "10001234",
                        VerificationDetails = new System.Collections.Generic.List<Models.Ukrlp.VerificationDetails>
                        {
                            new Models.Ukrlp.VerificationDetails
                            {
                                PrimaryVerificationSource = true,
                                VerificationAuthority = VerificationAuthorities.CompaniesHouseAuthority,
                                VerificationId = "12345678"
                            },
                            new Models.Ukrlp.VerificationDetails
                            {
                                PrimaryVerificationSource = false,
                                VerificationAuthority = VerificationAuthorities.CharityCommissionAuthority,
                                VerificationId = "11122221"
                            }
                        }
                    }
                }
            };

            _roatpApiClient.Setup(x => x.GetUkrlpDetails(It.IsAny<string>())).ReturnsAsync(ukrlpDetails);

            var registerStatus = new OrganisationRegisterStatus
            {
                UkprnOnRegister = false
            };
            _roatpApiClient.Setup(x => x.GetOrganisationRegisterStatus(It.IsAny<string>())).ReturnsAsync(registerStatus);


            var applyData = new ApplyData
            {
                ApplyDetails = new ApplyDetails
                {
                    UKPRN = "10001234"
                }
            };

            _applyRepository.Setup(x => x.GetApplyData(It.IsAny<Guid>())).ReturnsAsync(applyData);
            _applyRepository.Setup(x => x.UpdateApplyData(It.IsAny<Guid>(), It.IsAny<ApplyData>(), It.IsAny<string>())).ReturnsAsync(true);

            var companyDetails = new ApiResponse<Company>
            {
                Success = true,
                Response = new Company
                {
                    CompanyName = "My Ltd",
                    CompanyNumber = "12345678"
                }
            };
            var charityDetails = new Types.CharityCommission.Charity
            {
                CharityNumber = "11122221",
                Name = "My charity with new name"
            };

            _companiesHouseApiClient.Setup(x => x.GetCompany(It.IsAny<string>())).ReturnsAsync(companyDetails).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharity(It.IsAny<int>())).ReturnsAsync(charityDetails).Verifiable();

            var result = _controller.ExternalApiChecks(Guid.NewGuid(), "GatewayAssessor").GetAwaiter().GetResult();

            var response = result as OkObjectResult;
            response.Should().NotBeNull();
            var externalApiData = response.Value as ApplyGatewayDetails;
            externalApiData.Should().NotBeNull();

            externalApiData.CompaniesHouseDetails.Should().NotBeNull();
            externalApiData.CompaniesHouseDetails.CompanyName.Should().Be(companyDetails.Response.CompanyName);
            externalApiData.CompaniesHouseDetails.CompanyNumber.Should().Be(companyDetails.Response.CompanyNumber);
            externalApiData.CharityCommissionDetails.Should().NotBeNull();
            externalApiData.CharityCommissionDetails.CharityName.Should().Be(charityDetails.Name);
            externalApiData.CharityCommissionDetails.CharityNumber.Should().Be(charityDetails.CharityNumber);
            externalApiData.RoatpRegisterDetails.UkprnOnRegister.Should().BeFalse();
            externalApiData.UkrlpDetails.ProviderName.Should().Be(ukrlpDetails.Results[0].ProviderName);

            _companiesHouseApiClient.Verify(x => x.GetCompany(It.IsAny<string>()), Times.Once);
            _charityCommissionApiClient.Verify(x => x.GetCharity(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void External_api_checks_for_organisation_that_is_already_on_the_register()
        {
            var ukrlpDetails = new UkprnLookupResponse
            {
                Success = true,
                Results = new System.Collections.Generic.List<Models.Ukrlp.ProviderDetails>
                {
                    new Models.Ukrlp.ProviderDetails
                    {
                        ProviderName = "Provider",
                        UKPRN = "10001234",
                        VerificationDetails = new System.Collections.Generic.List<Models.Ukrlp.VerificationDetails>
                        {
                            new Models.Ukrlp.VerificationDetails
                            {
                                PrimaryVerificationSource = true,
                                VerificationAuthority = VerificationAuthorities.SoleTraderPartnershipAuthority,
                                VerificationId = "123"
                            }
                        }
                    }
                }
            };

            _roatpApiClient.Setup(x => x.GetUkrlpDetails(It.IsAny<string>())).ReturnsAsync(ukrlpDetails);

            var registerStatus = new OrganisationRegisterStatus
            {
                UkprnOnRegister = true,
                OrganisationId = Guid.NewGuid(),
                StatusDate = DateTime.Now.AddMonths(-3),
                StatusId = OrganisationStatus.Active,
                RemovedReasonId = null,
                ProviderTypeId = 1
            };
            _roatpApiClient.Setup(x => x.GetOrganisationRegisterStatus(It.IsAny<string>())).ReturnsAsync(registerStatus);


            var applyData = new ApplyData
            {
                ApplyDetails = new ApplyDetails
                {
                    UKPRN = "10001234"
                }
            };

            _applyRepository.Setup(x => x.GetApplyData(It.IsAny<Guid>())).ReturnsAsync(applyData);
            _applyRepository.Setup(x => x.UpdateApplyData(It.IsAny<Guid>(), It.IsAny<ApplyData>(), It.IsAny<string>())).ReturnsAsync(true);

            _companiesHouseApiClient.Setup(x => x.GetCompany(It.IsAny<string>())).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharity(It.IsAny<int>())).Verifiable();

            var result = _controller.ExternalApiChecks(Guid.NewGuid(), "GatewayAssessor").GetAwaiter().GetResult();

            var response = result as OkObjectResult;
            response.Should().NotBeNull();
            var externalApiData = response.Value as ApplyGatewayDetails;
            externalApiData.Should().NotBeNull();

            externalApiData.CharityCommissionDetails.Should().BeNull();
            externalApiData.CompaniesHouseDetails.Should().BeNull();
            externalApiData.RoatpRegisterDetails.UkprnOnRegister.Should().BeTrue();
            externalApiData.RoatpRegisterDetails.OrganisationId.Should().Be(registerStatus.OrganisationId);
            externalApiData.RoatpRegisterDetails.StatusDate.Should().Be(registerStatus.StatusDate);
            externalApiData.RoatpRegisterDetails.StatusId.Should().Be(registerStatus.StatusId);
            externalApiData.RoatpRegisterDetails.ProviderTypeId.Should().Be(registerStatus.ProviderTypeId);
            externalApiData.RoatpRegisterDetails.RemovedReasonId.Should().BeNull();
            externalApiData.UkrlpDetails.ProviderName.Should().Be(ukrlpDetails.Results[0].ProviderName);

            _companiesHouseApiClient.Verify(x => x.GetCompany(It.IsAny<string>()), Times.Never);
            _charityCommissionApiClient.Verify(x => x.GetCharity(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void External_api_checks_for_organisation_that_was_removed_from_the_register()
        {
            var ukrlpDetails = new UkprnLookupResponse
            {
                Success = true,
                Results = new System.Collections.Generic.List<Models.Ukrlp.ProviderDetails>
                {
                    new Models.Ukrlp.ProviderDetails
                    {
                        ProviderName = "Provider",
                        UKPRN = "10001234",
                        VerificationDetails = new System.Collections.Generic.List<Models.Ukrlp.VerificationDetails>
                        {
                            new Models.Ukrlp.VerificationDetails
                            {
                                PrimaryVerificationSource = true,
                                VerificationAuthority = VerificationAuthorities.SoleTraderPartnershipAuthority,
                                VerificationId = "123"
                            }
                        }
                    }
                }
            };

            _roatpApiClient.Setup(x => x.GetUkrlpDetails(It.IsAny<string>())).ReturnsAsync(ukrlpDetails);

            var registerStatus = new OrganisationRegisterStatus
            {
                UkprnOnRegister = true,
                OrganisationId = Guid.NewGuid(),
                StatusDate = DateTime.Now.AddMonths(-3),
                StatusId = OrganisationStatus.Removed,
                RemovedReasonId = RemovedReason.InadequateOfstedGrade,
                ProviderTypeId = 2
            };
            _roatpApiClient.Setup(x => x.GetOrganisationRegisterStatus(It.IsAny<string>())).ReturnsAsync(registerStatus);


            var applyData = new ApplyData
            {
                ApplyDetails = new ApplyDetails
                {
                    UKPRN = "10001234"
                }
            };

            _applyRepository.Setup(x => x.GetApplyData(It.IsAny<Guid>())).ReturnsAsync(applyData);
            _applyRepository.Setup(x => x.UpdateApplyData(It.IsAny<Guid>(), It.IsAny<ApplyData>(), It.IsAny<string>())).ReturnsAsync(true);

            _companiesHouseApiClient.Setup(x => x.GetCompany(It.IsAny<string>())).Verifiable();
            _charityCommissionApiClient.Setup(x => x.GetCharity(It.IsAny<int>())).Verifiable();

            var result = _controller.ExternalApiChecks(Guid.NewGuid(), "GatewayAssessor").GetAwaiter().GetResult();

            var response = result as OkObjectResult;
            response.Should().NotBeNull();
            var externalApiData = response.Value as ApplyGatewayDetails;
            externalApiData.Should().NotBeNull();

            externalApiData.CharityCommissionDetails.Should().BeNull();
            externalApiData.CompaniesHouseDetails.Should().BeNull();
            externalApiData.RoatpRegisterDetails.UkprnOnRegister.Should().BeTrue();
            externalApiData.RoatpRegisterDetails.OrganisationId.Should().Be(registerStatus.OrganisationId);
            externalApiData.RoatpRegisterDetails.StatusDate.Should().Be(registerStatus.StatusDate);
            externalApiData.RoatpRegisterDetails.StatusId.Should().Be(registerStatus.StatusId);
            externalApiData.RoatpRegisterDetails.ProviderTypeId.Should().Be(registerStatus.ProviderTypeId);
            externalApiData.RoatpRegisterDetails.RemovedReasonId.Should().Be(registerStatus.RemovedReasonId);
            externalApiData.UkrlpDetails.ProviderName.Should().Be(ukrlpDetails.Results[0].ProviderName);

            _companiesHouseApiClient.Verify(x => x.GetCompany(It.IsAny<string>()), Times.Never);
            _charityCommissionApiClient.Verify(x => x.GetCharity(It.IsAny<int>()), Times.Never);
        }
    }
}
