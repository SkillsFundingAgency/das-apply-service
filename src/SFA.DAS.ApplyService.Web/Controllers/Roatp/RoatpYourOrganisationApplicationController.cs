
namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    public class RoatpYourOrganisationApplicationController : Controller
    {
        public async Task<IActionResult> ProviderRoute(Guid applicationId)
        {
            // reset questions and show change provider route - does this need a confirm page first?
            return null;
        }

        public async Task<IActionResult> WhatYouWillNeed(Guid applicationId)
        {
            // return guidance appropriate to route
            return null;           
        }
    }
}
