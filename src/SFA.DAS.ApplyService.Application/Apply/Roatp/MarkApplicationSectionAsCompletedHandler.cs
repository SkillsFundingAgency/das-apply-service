using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class MarkApplicationSectionAsCompletedHandler : IRequestHandler<MarkApplicationSectionAsCompletedRequest, bool>
    {
        private readonly IApplyRepository _repository;
        private readonly ILogger<MarkApplicationSectionAsCompletedHandler> _logger;
        
        public MarkApplicationSectionAsCompletedHandler(IApplyRepository repository, ILogger<MarkApplicationSectionAsCompletedHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(MarkApplicationSectionAsCompletedRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.MarkSectionAsCompleted(request.ApplicationId, request.ApplicationSectionId);

            return await Task.FromResult(result);
        }
    }
}
