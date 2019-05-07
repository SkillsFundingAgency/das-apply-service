using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class QuestionOptionsController : Controller
    {
        private readonly AssessorServiceApiClient _assessorServiceApiClient;

        public QuestionOptionsController(AssessorServiceApiClient assessorServiceApiClient)
        {
            _assessorServiceApiClient = assessorServiceApiClient;
        }

        [HttpGet("QuestionOptions/DeliveryAreas")]
        public async Task<List<Option>> DeliveryAreas()
        {
            return (await _assessorServiceApiClient.GetDeliveryAreas()).Select(da => new Option(){Label = da.Area, Value = da.Area}).ToList();
        }
    }
}