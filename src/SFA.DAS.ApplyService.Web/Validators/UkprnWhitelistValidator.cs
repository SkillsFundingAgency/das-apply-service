using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public class UkprnWhitelistValidator : IUkprnWhitelistValidator
    {
        private readonly IWhitelistedProvidersApiClient _whitelistedProvidersApiClient;

        public UkprnWhitelistValidator(IWhitelistedProvidersApiClient whitelistedProvidersApiClient)
        {
            _whitelistedProvidersApiClient = whitelistedProvidersApiClient;
        }

        public async Task<bool> IsWhitelistedUkprn(long longUkprnToCheck)
        {
            int ukprn;
            if (int.TryParse(longUkprnToCheck.ToString(), out ukprn))
            {
                return await _whitelistedProvidersApiClient.CheckIsWhitelistedUkprn(ukprn); 
            }
            else
            {
                return await Task.FromResult(false);
            }
        }
    }
}
