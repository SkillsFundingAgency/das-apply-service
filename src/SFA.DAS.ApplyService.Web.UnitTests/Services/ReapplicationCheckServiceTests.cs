using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Domain.Apply.AllowedProviders;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;

namespace SFA.DAS.ApplyService.Web.UnitTests.Services
{
    [TestFixture]
    public class ReapplicationCheckServiceTests
    {
        private Mock<IApplicationApiClient> _applicationApiClient;
        private ReapplicationCheckService _service;
        private Guid _applicationId;
        private Guid _signInId;
        private Guid _organisationId;
        private string _ukprn;

        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();
            _signInId = Guid.NewGuid();
            _organisationId = Guid.NewGuid();
            _ukprn = "12345432";
            _applicationApiClient = new Mock<IApplicationApiClient>();
            _service = new ReapplicationCheckService(_applicationApiClient.Object);
        }

        [Test]
        public async Task Reapplication_Allowed_when_organisation_id_is_null()
        {
            var result = await _service.ReapplicationAllowed(_signInId, null);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Reapplication_Allowed_false_when_application_is_null()
        {
            var result = await _service.ReapplicationAllowed(_signInId, _organisationId);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Reapplication_Allowed_false_when_ukprn_is_null()
        {
            var applications = new List<Apply>
                {
                    new Apply
                    {
                        OrganisationId = _organisationId,
                        ApplyData = new ApplyData
                        {
                            ApplyDetails = new ApplyDetails()
                        }
                    }
                };
            _applicationApiClient.Setup(x => x.GetApplications(_signInId, true)).ReturnsAsync(applications);
            var result = await _service.ReapplicationAllowed(_signInId, _organisationId);
            Assert.IsFalse(result);
        }


        [Test]
        public async Task Reapplication_Allowed_false_when_not_in_allowed_provider_details()
        {
            var applications = new List<Apply>
            {
                new Apply
                {
                    OrganisationId = _organisationId,
                    ApplyData = new ApplyData
                    {
                        ApplyDetails = new ApplyDetails
                        {
                            UKPRN = _ukprn
                        }
                    }
                }
            };

            _applicationApiClient.Setup(x => x.GetApplications(_signInId, true)).ReturnsAsync(applications);
            var result = await _service.ReapplicationAllowed(_signInId, _organisationId);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Reapplication_Allowed_false_when_allowed_provider_details_is_not_in_date()
        {
            var applications = new List<Apply>
            {
                new Apply
                {
                    OrganisationId = _organisationId,
                    ApplyData = new ApplyData
                    {
                        ApplyDetails = new ApplyDetails
                        {
                            UKPRN = _ukprn
                        }
                    }
                }
            };

            _applicationApiClient.Setup(x => x.GetApplications(_signInId, true)).ReturnsAsync(applications);
            _applicationApiClient.Setup(x => x.GetAllowedProvider(_ukprn)).ReturnsAsync(new AllowedProvider
                { EndDateTime = DateTime.Today.AddDays(-1) });
            var result = await _service.ReapplicationAllowed(_signInId, _organisationId);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task Reapplication_Allowed_false_when_request_to_reapply_false()
        {
            var applications = new List<Apply>
            {
                new Apply
                {
                    OrganisationId = _organisationId,
                    ApplyData = new ApplyData
                    {
                        ApplyDetails = new ApplyDetails
                        {
                            UKPRN = _ukprn,
                            RequestToReapplyMade = false
                        }
                    }
                }
            };

            _applicationApiClient.Setup(x => x.GetApplications(_signInId, true)).ReturnsAsync(applications);
            _applicationApiClient.Setup(x => x.GetAllowedProvider(_ukprn)).ReturnsAsync(new AllowedProvider
                { EndDateTime = DateTime.Today });
            var result = await _service.ReapplicationAllowed(_signInId, _organisationId);
            Assert.IsFalse(result);
        }

        [TestCase(ApplicationStatus.AppealSuccessful, GatewayReviewStatus.Fail, true)]
        [TestCase(ApplicationStatus.AppealSuccessful, GatewayReviewStatus.Rejected, false)]
        [TestCase(ApplicationStatus.AppealSuccessful, GatewayReviewStatus.ClarificationSent, false)]
        [TestCase(ApplicationStatus.AppealSuccessful, GatewayReviewStatus.InProgress, false)]
        [TestCase(ApplicationStatus.AppealSuccessful, GatewayReviewStatus.Draft, false)]
        [TestCase(ApplicationStatus.AppealSuccessful, GatewayReviewStatus.New, false)]
        [TestCase(ApplicationStatus.Rejected, GatewayReviewStatus.Fail, true)]
        [TestCase(ApplicationStatus.Rejected, GatewayReviewStatus.Rejected, true)]
        [TestCase(ApplicationStatus.Rejected, GatewayReviewStatus.ClarificationSent, true)]
        [TestCase(ApplicationStatus.Rejected, GatewayReviewStatus.InProgress, true)]
        [TestCase(ApplicationStatus.Rejected, GatewayReviewStatus.Draft, true)]
        [TestCase(ApplicationStatus.Rejected, GatewayReviewStatus.New, true)]
        [TestCase(ApplicationStatus.Cancelled, GatewayReviewStatus.Fail, false)]
        [TestCase(ApplicationStatus.Cancelled, GatewayReviewStatus.Rejected, false)]
        [TestCase(ApplicationStatus.Cancelled, GatewayReviewStatus.ClarificationSent, false)]
        [TestCase(ApplicationStatus.Cancelled, GatewayReviewStatus.InProgress, false)]
        [TestCase(ApplicationStatus.Cancelled, GatewayReviewStatus.Draft, false)]
        [TestCase(ApplicationStatus.Cancelled, GatewayReviewStatus.New, false)]
        [TestCase(ApplicationStatus.GatewayAssessed, GatewayReviewStatus.Fail, false)]
        [TestCase(ApplicationStatus.InProgress, GatewayReviewStatus.Rejected, false)]
        [TestCase(ApplicationStatus.InProgressAppeal, GatewayReviewStatus.ClarificationSent, false)]
        [TestCase(ApplicationStatus.InProgressOutcome, GatewayReviewStatus.InProgress, false)]
        [TestCase(ApplicationStatus.New, GatewayReviewStatus.Draft, false)]
        [TestCase(ApplicationStatus.Removed, GatewayReviewStatus.New, false)]
        public async Task Reapplication_Allowed_status_when_applicationStatus_and_gateway_status_inserted(string applicationStatus, string gatewayStatus, bool expectedResult)
        {
            var applications = new List<Apply>
            {
                new Apply
                {
                    ApplicationStatus = applicationStatus,
                    GatewayReviewStatus = gatewayStatus,
                    OrganisationId = _organisationId,
                    ApplyData = new ApplyData
                    {
                        ApplyDetails = new ApplyDetails
                        {
                            UKPRN = _ukprn,
                            RequestToReapplyMade = true
                        }
                    }
                }
            };

            _applicationApiClient.Setup(x => x.GetApplications(_signInId, true)).ReturnsAsync(applications);
            _applicationApiClient.Setup(x => x.GetAllowedProvider(_ukprn)).ReturnsAsync(new AllowedProvider
                { EndDateTime = DateTime.Today });
            var result = await _service.ReapplicationAllowed(_signInId, _organisationId);
            Assert.AreEqual(expectedResult,result);
        }

        [TestCase(ApplicationStatus.AppealSuccessful, GatewayReviewStatus.Fail, true)]
        [TestCase(ApplicationStatus.Rejected, GatewayReviewStatus.Rejected, true)]
        [TestCase(ApplicationStatus.AppealSuccessful, GatewayReviewStatus.Rejected, false)]
        [TestCase(ApplicationStatus.AppealSuccessful, GatewayReviewStatus.ClarificationSent, false)]
        [TestCase(ApplicationStatus.AppealSuccessful, GatewayReviewStatus.InProgress, false)]
        [TestCase(ApplicationStatus.AppealSuccessful, GatewayReviewStatus.Draft, false)]
        [TestCase(ApplicationStatus.AppealSuccessful, GatewayReviewStatus.New, false)]
        [TestCase(ApplicationStatus.Rejected, GatewayReviewStatus.Fail, true)]
        [TestCase(ApplicationStatus.Rejected, GatewayReviewStatus.Rejected, true)]
        [TestCase(ApplicationStatus.Rejected, GatewayReviewStatus.ClarificationSent, true)]
        [TestCase(ApplicationStatus.Rejected, GatewayReviewStatus.InProgress, true)]
        [TestCase(ApplicationStatus.Rejected, GatewayReviewStatus.Draft, true)]
        [TestCase(ApplicationStatus.Rejected, GatewayReviewStatus.New, true)]
        [TestCase(ApplicationStatus.Cancelled, GatewayReviewStatus.Fail, false)]
        [TestCase(ApplicationStatus.Cancelled, GatewayReviewStatus.Rejected, false)]
        [TestCase(ApplicationStatus.Cancelled, GatewayReviewStatus.ClarificationSent, false)]
        [TestCase(ApplicationStatus.Cancelled, GatewayReviewStatus.InProgress, false)]
        [TestCase(ApplicationStatus.Cancelled, GatewayReviewStatus.Draft, false)]
        [TestCase(ApplicationStatus.Cancelled, GatewayReviewStatus.New, false)]
        [TestCase(ApplicationStatus.GatewayAssessed, GatewayReviewStatus.Fail, false)]
        [TestCase(ApplicationStatus.InProgress, GatewayReviewStatus.Rejected, false)]
        [TestCase(ApplicationStatus.InProgressAppeal, GatewayReviewStatus.ClarificationSent, false)]
        [TestCase(ApplicationStatus.InProgressOutcome, GatewayReviewStatus.InProgress, false)]
        [TestCase(ApplicationStatus.New, GatewayReviewStatus.Draft, false)]
        [TestCase(ApplicationStatus.Removed, GatewayReviewStatus.New, false)]
        public async Task check_for_ukprn_for_reapplication_returned(string applicationStatus, string gatewayStatus, bool expectedResult)
        {
            var applications = new List<Apply>
            {
                new Apply
                {
                    ApplicationStatus = applicationStatus,
                    GatewayReviewStatus = gatewayStatus,
                    OrganisationId = _organisationId,
                    ApplyData = new ApplyData
                    {
                        ApplyDetails = new ApplyDetails
                        {
                            UKPRN = _ukprn,
                            RequestToReapplyMade = true
                        }
                    }
                }
            };

            _applicationApiClient.Setup(x => x.GetApplications(_signInId, true)).ReturnsAsync(applications);
            var result = await _service.ReapplicationUkprnForUser(_signInId);
            var ukprnReturned = _ukprn == result;
            Assert.AreEqual(expectedResult, ukprnReturned);
        }


        [Test]
        public async Task check_application_inflight_with_different_user()
        {
            var contactId = Guid.NewGuid();
            var differentContactId = Guid.NewGuid();

            var applications = new List<Apply>
            {
                new Apply
                {
                    OrganisationId = _organisationId,
                    ApplyData = new ApplyData
                    {
                        ApplyDetails = new ApplyDetails
                        {
                            UKPRN = _ukprn
                        }
                    },
                    CreatedBy = differentContactId.ToString()
                }
            };

            _applicationApiClient.Setup(x => x.GetApplicationsByUkprn(_ukprn)).ReturnsAsync(applications);
            _applicationApiClient.Setup(x => x.GetContactBySignInId(contactId))
                .ReturnsAsync(new Contact { Id = contactId });

            var result = await _service.ApplicationInFlightWithDifferentUser(_signInId, _ukprn);

            Assert.IsTrue(result);
        }
    }

}
