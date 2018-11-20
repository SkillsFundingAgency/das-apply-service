using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Submit
{
    public class ApplicationSubmitHandler : IRequestHandler<ApplicationSubmitRequest>
    {
        private readonly IApplyRepository _applyRepository;

        public ApplicationSubmitHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<Unit> Handle(ApplicationSubmitRequest request, CancellationToken cancellationToken)
        {
            await _applyRepository.SubmitApplicationSequence(request);
            return Unit.Value;
        }
    }
}