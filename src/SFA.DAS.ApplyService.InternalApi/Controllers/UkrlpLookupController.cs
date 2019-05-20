namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Ukrlp;
    using global::AutoMapper;
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class UkrlpLookupController : Controller
    {
        private ILogger<UkrlpLookupController> _logger;

        private IUkrlpApiClient _apiClient;

        public UkrlpLookupController(ILogger<UkrlpLookupController> logger, IUkrlpApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        [Route("ukrlp-lookup")]
        public async Task<IActionResult> UkrlpLookup(string ukprn)
        {
            long ukprnValue = Convert.ToInt64(ukprn);

            var providerData = await _apiClient.GetTrainingProviderByUkprn(ukprnValue);

            var matchingProviders = Mapper.Map<List<ProviderDetails>>(providerData);

            return Ok(matchingProviders);
        }
    }
}
