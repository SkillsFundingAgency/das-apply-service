using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class GetApplicationSectionCompletedHandler : IRequestHandler<GetApplicationSectionCompletedRequest, bool>
    {
        private readonly IApplyRepository _repository;
        private readonly ILogger<GetApplicationSectionCompletedHandler> _logger;
        
        public GetApplicationSectionCompletedHandler(IApplyRepository repository, ILogger<GetApplicationSectionCompletedHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(GetApplicationSectionCompletedRequest request, CancellationToken cancellationToken)
        {
            var isCompleted = await _repository.IsSectionCompleted(request.ApplicationId, request.ApplicationSectionId);

            return await Task.FromResult(isCompleted);
        }
    }
}
