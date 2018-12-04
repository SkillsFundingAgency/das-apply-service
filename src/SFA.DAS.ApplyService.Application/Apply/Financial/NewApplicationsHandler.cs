using System;
using System.Collections.Generic;
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
        
        public Task<List<NewApplicationsResponse>> Handle(NewApplicationsRequest request, CancellationToken cancellationToken)
        {
            //_applyRepository.GetApplication()
            throw new Exception();
        }
    }
}