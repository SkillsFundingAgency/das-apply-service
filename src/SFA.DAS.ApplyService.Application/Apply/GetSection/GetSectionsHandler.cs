using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply.GetPage;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.GetSection
{
    public class GetSectionsHandler : IRequestHandler<GetSectionsRequest, List<ApplicationSection>>
    {
        private readonly IApplyRepository _applyRepository;

        public GetSectionsHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<List<ApplicationSection>> Handle(GetSectionsRequest request, CancellationToken cancellationToken)
        {
            var sections = await _applyRepository.GetSections(request.ApplicationId, request.SequenceId, request.UserId);
            if (sections == null)
            {
                throw new BadRequestException("Application not found");
            }

            return sections;
        }
    }
}