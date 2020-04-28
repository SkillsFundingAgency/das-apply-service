using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class UpdateNotRequiredOverridesHandler : IRequestHandler<UpdateNotRequiredOverridesRequest, bool>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly ILogger<UpdateNotRequiredOverridesHandler> _logger;

        public UpdateNotRequiredOverridesHandler(IApplyRepository applyRepository, ILogger<UpdateNotRequiredOverridesHandler> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateNotRequiredOverridesRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.SaveNotRequiredOverrides(request.ApplicationId, request.NotRequiredOverrides);
        }
    }
}
