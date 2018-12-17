using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Review
{
    public class NewApplicationsHandler : IRequestHandler<NewApplicationsRequest, List<object>>
    {
        private readonly IApplyRepository _repository;

        public NewApplicationsHandler(IApplyRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<List<dynamic>> Handle(NewApplicationsRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetNewApplications();
        }
    }
}