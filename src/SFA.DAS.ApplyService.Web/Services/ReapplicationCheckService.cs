﻿using System;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class ReapplicationCheckService : IReapplicationCheckService
    {
        private readonly IApplicationApiClient _applicationApiClient;


        public ReapplicationCheckService(IApplicationApiClient applicationApiClient)
        {
            _applicationApiClient = applicationApiClient;
        }

        public async Task<bool> ReapplicationAllowed(Guid signInId, Guid? organisationId)
        {
            if (organisationId == null)
                return true;

            var applications = await _applicationApiClient.GetApplications(signInId, true);
            var application = applications?.OrderByDescending(x => x.UpdatedAt)
                .FirstOrDefault(x => x.OrganisationId == organisationId);
            var applyDetails = application?.ApplyData?.ApplyDetails;
            if (applyDetails?.UKPRN == null) return false;
            var allowedProviderDetails = await _applicationApiClient.GetAllowedProvider(applyDetails.UKPRN);
            if (allowedProviderDetails == null || allowedProviderDetails.EndDateTime < DateTime.Today) return false;

            var reapplicationAllowed = false;

            if (applyDetails.RequestToReapplyMade==true)
            {
                reapplicationAllowed = application.ApplicationStatus == ApplicationStatus.Rejected 
                                       || (application.ApplicationStatus == ApplicationStatus.AppealSuccessful &&
                                           application.GatewayReviewStatus == GatewayReviewStatus.Fail);
            }

            return reapplicationAllowed;
        }

        public async Task<bool> ReapplicationRequestedAndPending(Guid signInId, Guid? organisationId)
        {
            if (organisationId == null)
                return false;

            var applications = await _applicationApiClient.GetApplications(signInId, true);
            var application = applications?.OrderByDescending(x => x.UpdatedAt)
                .FirstOrDefault(x => x.OrganisationId == organisationId);
            var applyDetails = application?.ApplyData?.ApplyDetails;
            if (applyDetails?.UKPRN == null) return false;
            

            var reapplicationAllowed = false;

            if (applyDetails.RequestToReapplyMade == true)
            {
                reapplicationAllowed = application.ApplicationStatus == ApplicationStatus.Rejected
                                       || (application.ApplicationStatus == ApplicationStatus.AppealSuccessful &&
                                           application.GatewayReviewStatus == GatewayReviewStatus.Fail);
            }

            if (!reapplicationAllowed) return false;

            var allowedProviderDetails = await _applicationApiClient.GetAllowedProvider(applyDetails.UKPRN);
            if (allowedProviderDetails == null || allowedProviderDetails.EndDateTime < DateTime.Today) return true;

            return false;
        }

        public async Task<string> ReapplicationUkprnForUser(Guid signInId)
        {
            var applications = await _applicationApiClient.GetApplications(signInId, true);
            var applicationsRejectedAndRequestToReapplyMade = applications.FirstOrDefault(x =>
                x.ApplyData?.ApplyDetails?.RequestToReapplyMade == true &&
                    (x.ApplicationStatus == ApplicationStatus.Rejected || 
                     (x.ApplicationStatus==ApplicationStatus.AppealSuccessful && x.GatewayReviewStatus==GatewayReviewStatus.Fail))
               );

            return applicationsRejectedAndRequestToReapplyMade?.ApplyData?.ApplyDetails?.UKPRN;
        }

        public async Task<Guid?> ReapplicationApplicationIdForUser(Guid signInId)
        {
            var applications = await _applicationApiClient.GetApplications(signInId, true);
            var applicationsRejectedAndRequestToReapplyMade = applications.FirstOrDefault(x =>
                x.ApplyData?.ApplyDetails?.RequestToReapplyMade == true &&
                (x.ApplicationStatus == ApplicationStatus.Rejected ||
                 (x.ApplicationStatus == ApplicationStatus.AppealSuccessful && x.GatewayReviewStatus == GatewayReviewStatus.Fail))
            );

            return applicationsRejectedAndRequestToReapplyMade?.ApplicationId;
        }

        public async Task<bool> ApplicationInFlightWithDifferentUser(Guid signInId, string ukprn)
        {
            var applications = await _applicationApiClient.GetApplicationsByUkprn(ukprn);

            var contact = await _applicationApiClient.GetContactBySignInId(signInId);
            var contactId = contact?.Id.ToString()?.ToUpper();

            var applicationsPresentAgainstDifferentUser = applications?.OrderByDescending(x => x.UpdatedAt)
                .Where(x => x?.ApplyData?.ApplyDetails?.UKPRN == ukprn && x.CreatedBy!=contactId && x.ApplicationStatus!=ApplicationStatus.Cancelled);
            
            return applicationsPresentAgainstDifferentUser != null && applicationsPresentAgainstDifferentUser.Any();
        }
    }
}