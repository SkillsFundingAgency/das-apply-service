using System.Threading.Tasks;
using Refit;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Domain.Roatp.Models;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;

namespace SFA.DAS.ApplyService.InternalApi.Services;

public class RoatpService(IRoatpApiClient _roatpApiClient) : IRoatpService
{
    public async Task<OrganisationRegisterStatus> GetRegisterStatus(int ukprn)
    {
        ApiResponse<OrganisationModel> apiResponse = await _roatpApiClient.GetOrganisation(ukprn);
        if (apiResponse == null || !apiResponse.IsSuccessStatusCode)
        {
            return new OrganisationRegisterStatus
            {
                UkprnOnRegister = false
            };
        }
        return new OrganisationRegisterStatus
        {
            UkprnOnRegister = true,
            OrganisationId = apiResponse.Content.OrganisationId,
            ProviderTypeId = (int)apiResponse.Content.ProviderType,
            StatusId = (int)apiResponse.Content.Status,
            RemovedReasonId = apiResponse.Content.RemovedReasonId,
            StatusDate = apiResponse.Content.ApplicationDeterminedDate
        };
    }
}
