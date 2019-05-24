namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    public class CharityCommissionController : Controller
    {
        private ILogger<CharityCommissionController> _logger;

        private CharityCommissionApiClient _apiClient;

        public CharityCommissionController(ILogger<CharityCommissionController> logger, CharityCommissionApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        [Route("charity-commission-lookup")]
        public async Task<IActionResult> CharityDetails(int charityNumber)
        {
            var charityDetails = await _apiClient.GetCharity(charityNumber);

            if (charityDetails == null)
            {
                return NotFound();
            }

            return Ok(charityDetails);
        }
    }
}
