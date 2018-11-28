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
                    ApplicationSequenceStatus.FeedbackAdded, ApplicationStatus.FeedbackAdded);
            }
            else if (request.RequestReturnType == "Approve")
            {
                await _applyRepository.UpdateSequenceStatus(request.ApplicationId, request.SequenceId,
                    ApplicationSequenceStatus.Approved, ApplicationStatus.InProgress);

                await _applyRepository.CloseSequence(request.ApplicationId, request.SequenceId);

                var nextSequence =
                    (await _applyRepository.GetSequences(request.ApplicationId))
                    .FirstOrDefault(seq => (int)seq.SequenceId == request.SequenceId + 1);

                if (nextSequence != null)
                {
                    await _applyRepository.OpenSequence(request.ApplicationId, (int)nextSequence.SequenceId);
                }
                else
                {
                    // this is the last sequence, so approve the whole application
                    await _applyRepository.UpdateApplicationStatus(request.ApplicationId, ApplicationStatus.Approved);
                }
            }
            else
            {
                await _applyRepository.UpdateSequenceStatus(request.ApplicationId, request.SequenceId,
                    ApplicationSequenceStatus.Rejected, ApplicationStatus.Rejected);
            }
                
            return Unit.Value;
        }
    }
}