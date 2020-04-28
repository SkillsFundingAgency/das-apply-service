using SFA.DAS.ApplyService.Web.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface INotRequiredOverridesService
    {
        Task<List<NotRequiredOverrideConfiguration>> GetNotRequiredOverrides(Guid applicationId);
    }
}
