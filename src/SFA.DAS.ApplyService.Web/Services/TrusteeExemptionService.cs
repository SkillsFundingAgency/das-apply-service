using SFA.DAS.ApplyService.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Services;

public class TrusteeExemptionService : ITrusteeExemptionService
{
    private readonly IConfigurationService _configurationService;

    public TrusteeExemptionService(IConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }

    public async Task<bool> IsProviderExempted(string ukprn)
    {
        var config = await _configurationService.GetConfig();
        var exemptionListing = config.ProvidersExemptedFromHavingTrustees;
        var exemptions = exemptionListing.Split(',');

        var isPresent = exemptions.Any(x => x == ukprn);
        return isPresent;
    }
}