using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;
using SFA.DAS.ApplyService.InternalApi.Types.CharityCommission;
using SFA.DAS.ApplyService.Web.AutoMapper;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Infrastructure.Services;

namespace SFA.DAS.ApplyService.Web.UnitTests.Services
{

    [TestFixture]
    public class RefreshTrusteeServiceTests
    {
        private  Mock<IQnaApiClient> _qnaApiClient;
        private  Mock<IOuterApiClient> _outerApiClient;
        private  Mock<IOrganisationApiClient> _organisationApiClient;
        private  Mock<IApplicationApiClient> _applicationApiClient;
        private  Mock<ILogger<RefreshTrusteesService>> _logger;
        private RefreshTrusteesService _service;
        private Guid _applicationId;
        private Guid _userId;

        [SetUp]
        public void Before_each_test()
        {
            _qnaApiClient = new Mock<IQnaApiClient>();
            _outerApiClient = new Mock<IOuterApiClient>();
            _organisationApiClient = new Mock<IOrganisationApiClient>();
            _applicationApiClient = new Mock<IApplicationApiClient>();
            _logger = new Mock<ILogger<RefreshTrusteesService>>();
            _userId = Guid.NewGuid();
            _applicationId = Guid.NewGuid();

            Mapper.Reset();

            Mapper.Initialize(cfg => { cfg.AddProfile<CharityCommissionProfile>(); });

            _service = new RefreshTrusteesService(_qnaApiClient.Object,
                _outerApiClient.Object,
                _organisationApiClient.Object,
                _applicationApiClient.Object,
                _logger.Object
            );
        }


        [TestCase(null, "12345678", "registered", "In Progress",false, false)] 
        [TestCase(87654321, null, "registered", "In Progress", false, false)]
        [TestCase(87654321, "12345678", "not registered", "In Progress", false, false)]
        [TestCase(87654321, "12345678", "registered", "New", false, false)]
        [TestCase(87654321, "12345678", "registered", "Not In Progress", false, false)]
        [TestCase(87654321, "12345678", "registered", "In Progress", true, false)]
        [TestCase(87654321, "12345678", "registered", "In Progress", false, true)]
        public async Task refresh_trustees_and_return_unsuccessful_with_breaking_details(int? ukprn, string charityNumber, string charityStatus, string applicationStatus, bool setCharityDetailsToNull, bool setTrusteesToEmpty)
        { 
            var organisation = new Organisation { OrganisationUkprn = ukprn };

            if (int.TryParse(charityNumber, out var charityNumberValue))
            {
                organisation.OrganisationDetails = new OrganisationDetails { CharityNumber = charityNumber, CharityCommissionDetails = new CharityCommissionDetails()};
            }
          
            _organisationApiClient.Setup(x => x.GetByApplicationId(_applicationId)).ReturnsAsync(organisation);

            var listOfTrustees = new List<Trustee>
            {
                new Trustee
                {
                    Id = 1234,
                    Name = "Mr A Trustee"
                },
                new Trustee
                {
                    Id = 1235,
                    Name = "Mr B Trustee"
                }
            };

            var charity = new Charity
            {
                CharityNumber = charityNumber,
                Trustees = listOfTrustees,
                Status = charityStatus
            };

            if (setCharityDetailsToNull)
                charity = null;
            else
            {
                if (setTrusteesToEmpty)
                {
                    charity.Trustees = new List<Trustee>();
                }
            }

            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.UKPRN)).ReturnsAsync(ukprn.ToString());
            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.UKRLPVerificationCharityRegNumber)).ReturnsAsync(charityNumber);

            _organisationApiClient.Setup(x => x.UpdateTrustees(ukprn.ToString(), listOfTrustees, It.IsAny<Guid>())).ReturnsAsync(true); 

            var currentStatuses = new List<RoatpApplicationStatus>();
            currentStatuses.Add(new RoatpApplicationStatus {ApplicationId = _applicationId, Status = applicationStatus});
            _applicationApiClient.Setup(x => x.GetExistingApplicationStatus(ukprn.ToString())).ReturnsAsync(currentStatuses);
            _outerApiClient.Setup(x => x.GetCharityDetails(charityNumberValue)).ReturnsAsync(charity).Verifiable();

            var updateResponse = new SetPageAnswersResponse {ValidationPassed = true};  
            _qnaApiClient.Setup(x => x.UpdatePageAnswers(_applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, It.IsAny<string>(), It.IsAny<List<Answer>>())).ReturnsAsync(updateResponse);

            var result = await _service.RefreshTrustees(_applicationId, _userId);

            var redirectResult = result as RefreshTrusteesResult;
            redirectResult.CharityNumber.Should().Be(charityNumber);
            redirectResult.CharityDetailsNotFound.Should().BeTrue();

            _organisationApiClient.Verify(x => x.UpdateTrustees(ukprn.ToString(), listOfTrustees, It.IsAny<Guid>()), Times.Never);
            _qnaApiClient.Verify(x => x.UpdatePageAnswers(_applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.CharityCommissionTrustees, It.IsAny<List<Answer>>()), Times.Never);
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.CharityCommissionTrustees), Times.Never);
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.CharityCommissionTrusteesDob), Times.Never);
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySection(_applicationId, RoatpWorkflowSequenceIds.CriminalComplianceChecks, RoatpWorkflowSectionIds.CriminalComplianceChecks.CheckOnWhosInControl), Times.Never);
        }
        
        [Test]
        public async Task refresh_trustees_and_send_successful_details()
        {
            const int ukprn = 12345678;
            const string charityNumber = "87654321";
            
            var organisation = new Organisation { OrganisationUkprn = ukprn };

            if (int.TryParse(charityNumber, out var charityNumberValue))
            {
                organisation.OrganisationDetails = new OrganisationDetails { CharityNumber = charityNumber, CharityCommissionDetails = new CharityCommissionDetails() };
            }

            _organisationApiClient.Setup(x => x.GetByApplicationId(_applicationId)).ReturnsAsync(organisation);

            var listOfTrustees = new List<Trustee>
            {
                new Trustee
                {
                    Id = 1234,
                    Name = "Mr A Trustee"
                },
                new Trustee
                {
                    Id = 1235,
                    Name = "Mr B Trustee"
                }
            };

            var charity = new Charity
            {
                CharityNumber = charityNumber,
                Trustees = listOfTrustees,
                Status = "registered"
            };

            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.UKPRN)).ReturnsAsync(ukprn.ToString());
            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.UKRLPVerificationCharityRegNumber)).ReturnsAsync(charityNumber);

            _organisationApiClient.Setup(x => x.UpdateTrustees(ukprn.ToString(), listOfTrustees, It.IsAny<Guid>())).ReturnsAsync(true);

            var currentStatuses = new List<RoatpApplicationStatus>();
            currentStatuses.Add(new RoatpApplicationStatus { ApplicationId = _applicationId, Status = ApplicationStatus.InProgress });
            _applicationApiClient.Setup(x => x.GetExistingApplicationStatus(ukprn.ToString())).ReturnsAsync(currentStatuses);
            _outerApiClient.Setup(x => x.GetCharityDetails(charityNumberValue)).ReturnsAsync(charity).Verifiable();

            var updateResponse = new SetPageAnswersResponse { ValidationPassed = true };
            _qnaApiClient.Setup(x => x.UpdatePageAnswers(_applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, It.IsAny<string>(), It.IsAny<List<Answer>>())).ReturnsAsync(updateResponse);

            var result = await _service.RefreshTrustees(_applicationId, _userId);

            var redirectResult = result as RefreshTrusteesResult;
            redirectResult.CharityNumber.Should().Be(charityNumber);
            redirectResult.CharityDetailsNotFound.Should().BeFalse();

            _organisationApiClient.Verify(x => x.UpdateTrustees(ukprn.ToString(), listOfTrustees, It.IsAny<Guid>()), Times.Once);
            _qnaApiClient.Verify(x => x.UpdatePageAnswers(_applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.CharityCommissionTrustees, It.IsAny<List<Answer>>()), Times.Once);
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.CharityCommissionTrustees), Times.Once);
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.CharityCommissionTrusteesDob), Times.Once);
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySection(_applicationId, RoatpWorkflowSequenceIds.CriminalComplianceChecks, RoatpWorkflowSectionIds.CriminalComplianceChecks.CheckOnWhosInControl), Times.Once);
        }

        [Test]
        public void refresh_trustees_and_update_trustees_fails_exception_thrown()
        {
            const int ukprn = 12345678;
            const string charityNumber = "87654321";

            var organisation = new Organisation { OrganisationUkprn = ukprn };

            if (int.TryParse(charityNumber, out var charityNumberValue))
            {
                organisation.OrganisationDetails = new OrganisationDetails { CharityNumber = charityNumber, CharityCommissionDetails = new CharityCommissionDetails() };
            }

            _organisationApiClient.Setup(x => x.GetByApplicationId(_applicationId)).ReturnsAsync(organisation);

            var listOfTrustees = new List<Trustee>
            {
                new Trustee
                {
                    Id = 1234,
                    Name = "Mr A Trustee"
                },
                new Trustee
                {
                    Id = 1235,
                    Name = "Mr B Trustee"
                }
            };

            var charity = new Charity
            {
                CharityNumber = charityNumber,
                Trustees = listOfTrustees,
                Status = "registered"
            };

            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.UKPRN)).ReturnsAsync(ukprn.ToString());
            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.UKRLPVerificationCharityRegNumber)).ReturnsAsync(charityNumber);

            _organisationApiClient.Setup(x => x.UpdateTrustees(ukprn.ToString(), listOfTrustees, It.IsAny<Guid>())).ReturnsAsync(false);

            var currentStatuses = new List<RoatpApplicationStatus>();
            currentStatuses.Add(new RoatpApplicationStatus { ApplicationId = _applicationId, Status = ApplicationStatus.InProgress });
            _applicationApiClient.Setup(x => x.GetExistingApplicationStatus(ukprn.ToString())).ReturnsAsync(currentStatuses);
            _outerApiClient.Setup(x => x.GetCharityDetails(charityNumberValue)).ReturnsAsync(charity).Verifiable();

            var updateResponse = new SetPageAnswersResponse { ValidationPassed = true };
            _qnaApiClient.Setup(x => x.UpdatePageAnswers(_applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, It.IsAny<string>(), It.IsAny<List<Answer>>())).ReturnsAsync(updateResponse);

            Assert.ThrowsAsync<InvalidOperationException>(() => _service.RefreshTrustees(_applicationId, _userId));
        }

        [Test]
        public void refresh_trustees_and_update_page_answers_fails_exception_thrown()
        {
            const int ukprn = 12345678;
            const string charityNumber = "87654321";

            var organisation = new Organisation { OrganisationUkprn = ukprn };

            if (int.TryParse(charityNumber, out var charityNumberValue))
            {
                organisation.OrganisationDetails = new OrganisationDetails { CharityNumber = charityNumber, CharityCommissionDetails = new CharityCommissionDetails() };
            }

            _organisationApiClient.Setup(x => x.GetByApplicationId(_applicationId)).ReturnsAsync(organisation);

            var listOfTrustees = new List<Trustee>
            {
                new Trustee
                {
                    Id = 1234,
                    Name = "Mr A Trustee"
                },
                new Trustee
                {
                    Id = 1235,
                    Name = "Mr B Trustee"
                }
            };

            var charity = new Charity
            {
                CharityNumber = charityNumber,
                Trustees = listOfTrustees,
                Status = "registered"
            };

            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.UKPRN)).ReturnsAsync(ukprn.ToString());
            _qnaApiClient.Setup(x => x.GetQuestionTag(_applicationId, RoatpWorkflowQuestionTags.UKRLPVerificationCharityRegNumber)).ReturnsAsync(charityNumber);

            _organisationApiClient.Setup(x => x.UpdateTrustees(ukprn.ToString(), listOfTrustees, It.IsAny<Guid>())).ReturnsAsync(true);

            var currentStatuses = new List<RoatpApplicationStatus>();
            currentStatuses.Add(new RoatpApplicationStatus { ApplicationId = _applicationId, Status = ApplicationStatus.InProgress });
            _applicationApiClient.Setup(x => x.GetExistingApplicationStatus(ukprn.ToString())).ReturnsAsync(currentStatuses);
            _outerApiClient.Setup(x => x.GetCharityDetails(charityNumberValue)).ReturnsAsync(charity).Verifiable();

            var updateResponse = new SetPageAnswersResponse { ValidationPassed = false };
            _qnaApiClient.Setup(x => x.UpdatePageAnswers(_applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, It.IsAny<string>(), It.IsAny<List<Answer>>())).ReturnsAsync(updateResponse);

            Assert.ThrowsAsync<InvalidOperationException>(() => _service.RefreshTrustees(_applicationId, _userId));
        }
    }
}
