using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class GetNextApplicationReferenceHandler : IRequestHandler<GetNextApplicationReferenceRequest, string>
    {
        private readonly IApplyRepository _repository;
        private readonly ILogger<GetNextApplicationReferenceHandler> _logger;
        
        public GetNextApplicationReferenceHandler(IApplyRepository repository, ILogger<GetNextApplicationReferenceHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<string> Handle(GetNextApplicationReferenceRequest request, CancellationToken cancellationToken)
        {
            var applicationReference = await _repository.GetNextRoatpApplicationReference();

            return await Task.FromResult(applicationReference);
        }
    }
}
