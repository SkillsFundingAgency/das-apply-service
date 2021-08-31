using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.AllowedProviders;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.AllowedProviders
{
    public class GetAllowedProvidersListRequest : IRequest<List<AllowedProvider>>
    {
        public string SortColumn { get; }
        public string SortOrder { get; }

        public GetAllowedProvidersListRequest(string sortColumn, string sortOrder)
        {
            SortColumn = sortColumn;
            SortOrder = sortOrder;
        }
    }
}
