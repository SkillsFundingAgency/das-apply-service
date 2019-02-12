using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class PreviousApplicationsHandler : IRequestHandler<PreviousApplicationsRequest, List<PreviousApplicationsResponse>>
    {
        private readonly IApplyRepository _applyRepository;

        public PreviousApplicationsHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<List<PreviousApplicationsResponse>> Handle(PreviousApplicationsRequest request, CancellationToken cancellationToken)
        {
            var previous = await _applyRepository.GetPreviousFinancialApplications();
            return previous.Select(r => new PreviousApplicationsResponse
            {
                ApplicationId = r.Id,
                ApplyingOrganisationName = r.Name,
                GradedBy = r.GradedBy,
                GradedDateTime = DateTime.Parse(r.GradedDateTime),
                Grade = r.Grade
            }).ToList();
        }
    }
}