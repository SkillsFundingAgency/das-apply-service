using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Clarification
{
    public class DeleteClarificationFileHandler : IRequestHandler<DeleteClarificationFileRequest>
    {
        private readonly IClarificationRepository _repository;
        private readonly ILogger<DeleteClarificationFileHandler> _logger;

        public DeleteClarificationFileHandler(IClarificationRepository repository, ILogger<DeleteClarificationFileHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteClarificationFileRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"DeleteClarificationFile for ApplicationId '{request.ApplicationId}' - PageId '{request.PageId}' - ClarificationFile '{request.ClarificationFile}'");

            await _repository.DeleteClarificationFile(request.ApplicationId,
                                                        request.SequenceNumber,
                                                        request.SectionNumber,
                                                        request.PageId,
                                                        request.ClarificationFile);

            return Unit.Value;
        }
    }
}
