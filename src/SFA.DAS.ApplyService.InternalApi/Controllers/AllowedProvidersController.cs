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

        [HttpGet("/AllowedProviders/ukprn/{ukprn}")]
        public async Task<bool> IsUkprnOnAllowedProviderList(int ukprn)
        {
            return await _mediator.Send(new IsUkprnOnAllowedProvidersListRequest(ukprn));
        }

        [HttpGet("/AllowedProviders")]
        public async Task<List<AllowedProvider>> GetAllowedProvidersList(string sortColumn, string sortOrder)
        {
            return await _mediator.Send(new GetAllowedProvidersListRequest(sortColumn, sortOrder));
        }

        [HttpPost("/AllowedProviders")]
        public async Task<bool> AddAllowedProvider(AllowedProvider entry)
        {
            return await _mediator.Send(new AddAllowedProviderRequest(entry.Ukprn, entry.StartDateTime, entry.EndDateTime));
        }
    }
}