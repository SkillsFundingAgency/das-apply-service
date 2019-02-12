using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;


namespace SFA.DAS.ApplyService.Application.Apply.CheckOrganisationStandardStatus
{

    public class CheckOrganisationStandardStatusHandler : IRequestHandler<CheckOrganisationStandardStatusRequest, string>
    {
        private readonly IApplyRepository _applyRepository;

        public CheckOrganisationStandardStatusHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<string> Handle(CheckOrganisationStandardStatusRequest statusRequest, CancellationToken cancellationToken)
        {
            return await _applyRepository.CheckOrganisationStandardStatus(statusRequest.ApplicationId, statusRequest.StandardId);
        }
    }
}
