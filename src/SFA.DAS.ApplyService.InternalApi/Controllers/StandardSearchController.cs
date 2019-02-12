using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Types;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    public class StandardSearchController: Controller
    {
        private readonly ILogger<StandardSearchController> _logger;
        private readonly AssessorServiceApiClient _assessorServiceApiClient;

        public StandardSearchController(AssessorServiceApiClient assessorServiceApiClient, ILogger<StandardSearchController> logger)
        {
            _assessorServiceApiClient = assessorServiceApiClient;
            _logger = logger;
        }

        [HttpGet("all-standards")]
        public async Task<IEnumerable<StandardCollation>> GetAllStandards()
        {
            _logger.LogInformation("Get All Standards Request");
            var results = await _assessorServiceApiClient.GetStandards();
            return results;
        }
    }
}
