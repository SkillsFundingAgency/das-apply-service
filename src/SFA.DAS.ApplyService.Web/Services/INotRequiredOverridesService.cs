using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface INotRequiredOverridesService
    {
        Task RefreshNotRequiredOverrides(Guid applicationId);
        Task<List<NotRequiredOverride>> GetNotRequiredOverrides(Guid applicationId);
    }
}
