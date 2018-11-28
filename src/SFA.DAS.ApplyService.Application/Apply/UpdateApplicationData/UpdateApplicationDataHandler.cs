using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;

namespace SFA.DAS.ApplyService.Application.Apply.UpdateApplicationData
{
    public class UpdateApplicationDataHandler : IRequestHandler<UpdateApplicationDataRequest>
    {
        private readonly IApplyRepository _applyRepository;

        public UpdateApplicationDataHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<Unit> Handle(UpdateApplicationDataRequest request, CancellationToken cancellationToken)
        {
            var serialisedData = JsonConvert.SerializeObject(request.ApplicationData);
            await _applyRepository.UpdateApplicationData(request.ApplicationId, serialisedData);
            
            return Unit.Value;
        }
    }
}