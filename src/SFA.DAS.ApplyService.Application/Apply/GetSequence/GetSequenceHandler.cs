using MediatR;
using SFA.DAS.ApplyService.Application.Apply.GetPage;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.GetSequence
{
    public class GetSequenceHandler : IRequestHandler<GetSequenceRequest, ApplicationSequence>
    {
        private readonly IApplyRepository _applyRepository;

        public GetSequenceHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<ApplicationSequence> Handle(GetSequenceRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetSequence(request.ApplicationId, request.SequenceId, request.UserId);
        }
    }
}
