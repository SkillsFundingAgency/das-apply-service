using SFA.DAS.ApplyService.Web.Infrastructure;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public class AllowedUkprnValidator : IAllowedUkprnValidator
    {
        private readonly IAllowedProvidersApiClient _allowedProvidersApiClient;

        public AllowedUkprnValidator(IAllowedProvidersApiClient allowedProvidersApiClient)
        {
            _allowedProvidersApiClient = allowedProvidersApiClient;
        }

        public async Task<bool> IsUkprnOnAllowedList(int ukprn)
        {
            return await _allowedProvidersApiClient.IsUkprnOnAllowedList(ukprn);
        }
    }
}
