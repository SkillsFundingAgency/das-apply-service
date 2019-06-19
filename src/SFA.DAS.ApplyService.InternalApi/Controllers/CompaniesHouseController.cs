namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using SFA.DAS.ApplyService.InternalApi.Infrastructure;
    using System.Threading.Tasks;

    public class CompaniesHouseController : Controller
    {
        private ILogger<CompaniesHouseController> _logger;

        private CompaniesHouseApiClient _apiClient;

        public CompaniesHouseController(ILogger<CompaniesHouseController> logger, CompaniesHouseApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        [Route("companies-house-lookup")]
        public async Task<IActionResult> CompanyDetails(string companyNumber)
        {
            var companyDetails = await _apiClient.GetCompany(companyNumber);

            if (companyDetails == null)
            {
                return NotFound();
            }

            return Ok(companyDetails);
        }
    }
}
