using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply.GetPage;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.GetSequence
{
    public class GetSequenceHandler : IRequestHandler<GetSequenceRequest, Sequence> 
    {
        private readonly IApplyRepository _applyRepository;

        public GetSequenceHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<Sequence> Handle(GetSequenceRequest request, CancellationToken cancellationToken)
        {
            var application = await _applyRepository.GetEntity(request.ApplicationId, request.UserId);
            if (application == null)
            {
                throw new BadRequestException("Application not found");
            }
            var sequence = application.QnAWorkflow.Sequences.Single(s => s.SequenceId == request.SequenceId);

            if (!sequence.Active)
            {
                throw new UnauthorisedException("Sequence not active");
            }

            return sequence;
        }
    }
}