using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public interface IGatewayApiChecksService
    {
        Task<ApplyGatewayDetails> GetExternalApiCheckDetails(Guid applicationId, string userRequestedChecks);
    }
}
