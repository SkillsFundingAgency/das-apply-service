using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.AllowedProviders;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class AllowedProvidersController : Controller
    {
        private readonly IMediator _mediator;

        public AllowedProvidersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/AllowedProviders/ukprn/{ukprn}")]
        public async Task<bool> IsUkprnOnAllowedProviderList(int ukprn)
        {
            return await _mediator.Send(new IsUkprnOnAllowedProvidersListRequest(ukprn));
        }
    }
}