using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply.GetPage;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.GetSequence
{
    public class GetSequencesHandler : IRequestHandler<GetSequencesRequest, List<Sequence>>
    {
        private readonly IApplyRepository _applyRepository;

        public GetSequencesHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<List<Sequence>> Handle(GetSequencesRequest request, CancellationToken cancellationToken)
        {
            var application = await _applyRepository.GetEntity(request.ApplicationId, request.UserId);
            if (application == null)
            {
                throw new BadRequestException("Application not found");
            }

            return application.QnAWorkflow.Sequences;
        }
    }
}