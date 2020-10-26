using SFA.DAS.ApplyService.Web.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface INotRequiredOverridesService
    {
        Task RefreshNotRequiredOverridesAsync(Guid applicationId);
        Task<List<NotRequiredOverrideConfiguration>> GetNotRequiredOverridesAsync(Guid applicationId);
    }
}
