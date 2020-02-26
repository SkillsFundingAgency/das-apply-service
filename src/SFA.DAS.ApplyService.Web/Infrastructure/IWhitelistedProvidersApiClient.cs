using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public interface IWhitelistedProvidersApiClient
    {
        Task<bool> CheckIsWhitelistedUkprn(int ukprn);
    }
}
