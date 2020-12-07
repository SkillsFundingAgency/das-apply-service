using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway
{
    public class UpdateGatewayPageAnswerRequestHandler : IRequestHandler<UpdateGatewayPageAnswerRequest>
    {
        private readonly IApplyRepository _applyRepository;

        public UpdateGatewayPageAnswerRequestHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<Unit> Handle(UpdateGatewayPageAnswerRequest request, CancellationToken cancellationToken)
        {
            await _applyRepository.SubmitGatewayPageAnswer(request.ApplicationId, request.PageId, request.UserId, request.UserName,
                request.Status, request.Comments);

            return Unit.Value;
        }
    }
}
