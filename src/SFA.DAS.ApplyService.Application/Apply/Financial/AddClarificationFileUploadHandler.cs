using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class AddClarificationFileUploadHandler : IRequestHandler<AddClarificationFileUploadRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;

        private readonly ILogger<AddClarificationFileUploadHandler> _logger;

        public AddClarificationFileUploadHandler(IApplyRepository applyRepository, ILogger<AddClarificationFileUploadHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(AddClarificationFileUploadRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Adding clarification file [{request.FileName}] for application ID {request.ApplicationId}");
            return await _applyRepository.AddFinancialReviewClarificationFile(request.ApplicationId, request.FileName);
        }
    }
}
