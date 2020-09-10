using SFA.DAS.ApplyService.Web.Configuration;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface INotRequiredOverridesService
    {
        void RefreshNotRequiredOverrides(Guid applicationId);
        List<NotRequiredOverrideConfiguration> GetNotRequiredOverrides(Guid applicationId);
    }
}
