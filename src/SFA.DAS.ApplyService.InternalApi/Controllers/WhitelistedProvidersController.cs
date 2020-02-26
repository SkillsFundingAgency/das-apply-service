using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.WhitelistedProviders;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class WhitelistedProvidersController : Controller
    {
        private readonly IMediator _mediator;

        public WhitelistedProvidersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/whitelistedproviders/ukprn/{ukprn}")]
        public async Task<bool> CheckIsUkprnWhitelisted(int ukprn)
        {
            return await _mediator.Send(new CheckIsUkprnWhitelistedRequest(ukprn));
        }
    }
}