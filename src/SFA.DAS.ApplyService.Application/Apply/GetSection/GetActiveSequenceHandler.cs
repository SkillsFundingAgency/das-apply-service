using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply.GetPage;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.GetSection
{
    public class GetActiveSequenceHandler : IRequestHandler<GetActiveSequenceRequest, ApplicationSequence>
    {
        private readonly IApplyRepository _applyRepository;

        public GetActiveSequenceHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<ApplicationSequence> Handle(GetActiveSequenceRequest request, CancellationToken cancellationToken)
        {
            var sequence = await _applyRepository.GetActiveSequence(request.ApplicationId);
            if (sequence == null)
            {
                throw new BadRequestException("Application not found");
            }

            return sequence;
        }
    }
}