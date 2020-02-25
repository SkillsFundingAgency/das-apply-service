using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.GetSection
{
    using System.Collections.Generic;

    public class GetSectionsHandler : IRequestHandler<GetSectionsRequest, IEnumerable<ApplicationSection>> 
    {
        private readonly IApplyRepository _applyRepository;

        public GetSectionsHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<IEnumerable<ApplicationSection>> Handle(GetSectionsRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetSections(request.ApplicationId, request.SequenceId, request.UserId);
        }
    }
}