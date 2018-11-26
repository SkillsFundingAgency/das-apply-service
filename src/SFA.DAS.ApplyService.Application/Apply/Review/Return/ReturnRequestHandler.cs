using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Return
{
    public class ReturnRequestHandler : IRequestHandler<ReturnRequest>
    {
        private readonly IApplyRepository _applyRepository;

        public ReturnRequestHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<Unit> Handle(ReturnRequest request, CancellationToken cancellationToken)
        {
            // There will probably be some sort of decision of notifications here based on what status the Application
            // is being set to.  But for now I'm just setting it and forgetting.

            var sequence = await _applyRepository.GetActiveSequence(request.ApplicationId);
            if (request.RequestReturnType == "ReturnWithFeedback")
            {
                await _applyRepository.UpdateSequenceStatus(request.ApplicationId, request.SequenceId,
                    ApplicationSequenceStatus.FeedbackAdded);
            }
            else if (request.RequestReturnType == "Approve")
            {
                await _applyRepository.UpdateSequenceStatus(request.ApplicationId, request.SequenceId,
                    ApplicationSequenceStatus.Approved);

                await _applyRepository.CloseSequence(request.ApplicationId, request.SequenceId);

                var nextSequence =
                    (await _applyRepository.GetSequences(request.ApplicationId))
                    .FirstOrDefault(seq => seq.SequenceId != request.SequenceId);


                if (nextSequence != null)
                {
                    await _applyRepository.OpenSequence(request.ApplicationId, nextSequence.SequenceId);
                }
            }
            else
            {
                await _applyRepository.UpdateSequenceStatus(request.ApplicationId, request.SequenceId,
                    ApplicationSequenceStatus.Rejected);
            }
                
            return Unit.Value;
        }
    }
}