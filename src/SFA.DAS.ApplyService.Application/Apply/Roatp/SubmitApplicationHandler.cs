using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class SubmitApplicationHandler : IRequestHandler<SubmitApplicationRequest, bool>
    { 
        private readonly IApplyRepository _repository;
        private readonly ILogger<SubmitApplicationHandler> _logger;

        public SubmitApplicationHandler(IApplyRepository repository, ILogger<SubmitApplicationHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(SubmitApplicationRequest request, CancellationToken cancellationToken)
        {
            var submitResult = await _repository.SubmitRoatpApplication(request.ApplicationData);

            return await Task.FromResult(submitResult);
        }
    }
}
