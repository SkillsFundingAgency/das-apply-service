using System;
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
            var application = applications.OrderByDescending(x => x.UpdatedAt)
                .FirstOrDefault(x => x.OrganisationId == organisationId);
            var applyDetails = application?.ApplyData?.ApplyDetails;
            if (applyDetails?.UKPRN == null) return false;
            var allowedProviderDetails = await _applicationApiClient.GetAllowedProvider(applyDetails?.UKPRN);
            if (allowedProviderDetails == null || allowedProviderDetails.EndDateTime < DateTime.Today) return false;
            return application.ApplicationStatus == ApplicationStatus.Rejected &&
                   applyDetails?.RequestToReapplyMade == true;
        }

        public async Task<string> ReapplicationUkprnForUser(Guid signInId)
        {
            var applications = await _applicationApiClient.GetApplications(signInId, true);
            var applicationsRejectedAndRequestToReapplyMade = applications.FirstOrDefault(x =>
                x.ApplicationStatus == ApplicationStatus.Rejected &&
                x?.ApplyData?.ApplyDetails?.RequestToReapplyMade == true);

            return applicationsRejectedAndRequestToReapplyMade?.ApplyData?.ApplyDetails?.UKPRN;
        }

        public async Task<bool> ReapplicationInProgressWithDifferentUser(Guid signInId, Guid organisationId)
        {
            var applications = await _applicationApiClient.GetApplications(signInId, false);

            var applicationsReapplicationAgainstDifferentUser = applications.OrderByDescending(x => x.UpdatedAt)
                .Where(x => x.OrganisationId == organisationId && x.CreatedBy!=signInId.ToString() && x.ApplicationStatus == ApplicationStatus.Rejected &&
                                     x?.ApplyData?.ApplyDetails?.RequestToReapplyMade == true);

            return applicationsReapplicationAgainstDifferentUser.Any();


        }
    }
}