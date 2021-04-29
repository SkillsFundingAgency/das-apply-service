using SFA.DAS.ApplyService.Web.Infrastructure;
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

        public async Task<bool> IsWhitelistedUkprn(int ukprn)
        {
            return await _whitelistedProvidersApiClient.CheckIsWhitelistedUkprn(ukprn);
        }
    }
}
