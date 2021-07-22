using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.AllowedProviders;
using SFA.DAS.ApplyService.Domain.Apply.AllowedProviders;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [HttpGet("/AllowedProviders/{ukprn}/can-start-application")]
        public async Task<bool> CanUkprnStartApplication(int ukprn)
        {
            return await _mediator.Send(new CanUkprnStartApplicationRequest(ukprn));
        }

        [HttpGet("/AllowedProviders")]
        public async Task<List<AllowedProvider>> GetAllowedProvidersList(string sortColumn, string sortOrder)
        {
            return await _mediator.Send(new GetAllowedProvidersListRequest(sortColumn, sortOrder));
        }

        [HttpPost("/AllowedProviders")]
        public async Task<bool> AddAllowedProvider([FromBody] AllowedProvider entry)
        {
            return await _mediator.Send(new AddAllowedProviderRequest(entry.Ukprn, entry.StartDateTime, entry.EndDateTime));
        }

        [HttpGet("/AllowedProviders/{ukprn}")]
        public async Task<AllowedProvider> GetAllowedProviderDetails(int ukprn)
        {
            return await _mediator.Send(new GetAllowedProviderDetailsRequest(ukprn));
        }

        [HttpDelete("/AllowedProviders/{ukprn}")]
        public async Task<bool> RemoveAllowedProvider(int ukprn)
        {
            return await _mediator.Send(new RemoveAllowedProviderRequest(ukprn));
        }
    }
}