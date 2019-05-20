namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::AutoMapper;
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models.Roatp;
    using SFA.DAS.ApplyService.Domain.Roatp;

    public class RoatpApplicationController : Controller
    {
        private ILogger<RoatpApplicationController> _logger;

        private IRoatpApiClient _apiClient;

        public RoatpApplicationController(ILogger<RoatpApplicationController> logger, IRoatpApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        [Route("all-roatp-routes")]
        public async Task<IActionResult> GetApplicationRoutes()
        {
            var providerTypes = await _apiClient.GetProviderTypes();

            var applicationRoutes = Mapper.Map<IEnumerable<ApplicationRoute>>(providerTypes);

            return Ok(applicationRoutes);
        }

        [Route("ukprn-on-register")]
        public async Task<IActionResult> UkprnOnRegister(long ukprn)
        {
            var response = await _apiClient.DuplicateUKPRNCheck(Guid.Empty, ukprn);

            if (response.DuplicateFound)
            {
                var reapplyStatus = await _apiClient.GetOrganisationRegisterStatus(response.DuplicateOrganisationId);
                reapplyStatus.ExistingUKPRN = response.DuplicateFound;

                return Ok(reapplyStatus);
            }

            return Ok(new OrganisationRegisterStatus {ExistingUKPRN = false});
        }
    }
}
