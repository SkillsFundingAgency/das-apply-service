﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ApplyService.InternalApi.Services;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class WorkingDaysController : Controller
    {
        private readonly IBankHolidayService _service;

        public WorkingDaysController(IBankHolidayService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("working-days/{startDate}/{numberOfDays}")]
        public async Task<IActionResult> GetWorkingDays(DateTime? startDate, int numberOfDays)
        {
            if(startDate.HasValue)
            {
                var workingDays = await _service.GetWorkingDaysAheadDate(startDate.Value, numberOfDays);
                return Ok(workingDays);
            }
            else
            {
                return Ok(startDate);
            }
        }
    }
}
