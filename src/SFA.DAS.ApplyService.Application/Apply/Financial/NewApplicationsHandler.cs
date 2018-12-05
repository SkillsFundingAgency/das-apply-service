using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class NewApplicationsHandler : IRequestHandler<NewApplicationsRequest, List<NewApplicationsResponse>>
    {
        private readonly IApplyRepository _applyRepository;

        public NewApplicationsHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<List<NewApplicationsResponse>> Handle(NewApplicationsRequest request, CancellationToken cancellationToken)
        {
            return (await _applyRepository.GetNewFinancialApplications()).Select(r => new NewApplicationsResponse
            {
                ApplicationId = r.Id,
                ApplyingOrganisationName = r.Name,
                Status = r.Status
            }).ToList();
        }
    }
}