using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ApplyService.InternalApi.Services;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class BankHolidayController : Controller
    {
        private readonly IBankHolidayService _service;

        public BankHolidayController(IBankHolidayService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("working-days/{startDate}/{numberOfDays}")]
        public async Task<IActionResult> GetGetWorkingDays(DateTime? startDate, int numberOfDays)
        {
            var workingDays = _service.GetWorkingDaysAheadDate(startDate, numberOfDays);


            if (workingDays == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(workingDays);
            }
        }
    }
}
