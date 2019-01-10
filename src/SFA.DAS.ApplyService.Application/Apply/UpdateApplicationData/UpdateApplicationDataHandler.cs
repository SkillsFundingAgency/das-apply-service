using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Entities;

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
            if (request.ApplicationData == null) return Unit.Value;
            var standardName = ((StandardApplicationData) request.ApplicationData).StandardName;
            var application = await _applyRepository.GetApplication(request.ApplicationId);
            if (application?.ApplicationData != null)
            {
                application.ApplicationData.StandardName = standardName;
                await _applyRepository.UpdateApplicationData(request.ApplicationId, application.ApplicationData);
            }
            else
            {
                await _applyRepository.UpdateApplicationData(request.ApplicationId, new ApplicationData
                {
                    StandardName = standardName
                });
            }

            return Unit.Value;
        }
    }
}