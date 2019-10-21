using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class RemoveApplicationSectionCompletedHandler : IRequestHandler<RemoveApplicationSectionCompletedRequest,bool>
    {
        private readonly IApplyRepository _repository;
        private readonly ILogger<RemoveApplicationSectionCompletedHandler> _logger;

        public RemoveApplicationSectionCompletedHandler(IApplyRepository repository, ILogger<RemoveApplicationSectionCompletedHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(RemoveApplicationSectionCompletedRequest request, CancellationToken cancellationToken)
        {
            await _repository.RemoveSectionCompleted(request.ApplicationId, request.ApplicationSectionId);
            return true;

        }
    }
}
