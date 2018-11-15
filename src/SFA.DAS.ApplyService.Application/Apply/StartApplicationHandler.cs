using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public class StartApplicationHandler : IRequestHandler<StartApplicationRequest>
    {
        private readonly IApplyRepository _applyRepository;

        public StartApplicationHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<Unit> Handle(StartApplicationRequest request, CancellationToken cancellationToken)
        {
            var assets = await _applyRepository.GetAssets();
            try
            {
                var workflowId = await _applyRepository.GetLatestWorkflow(request.ApplicationType);
                var applicationId =
                    await _applyRepository.CreateApplication(request.ApplicationType, request.ApplyingOrganisationId,
                        request.UserId, workflowId);

                var sections = await _applyRepository.CopyWorkflowToApplication(applicationId, workflowId, request.OrganisationType);

                foreach (var applicationSection in sections)
                {
                    foreach (var asset in assets)
                    {
                        applicationSection.QnAData = applicationSection.QnAData.Replace(asset.Reference, asset.Text);
                    }
                }

                await _applyRepository.UpdateSections(sections);
            }
            catch (Exception e)
            {
                var a = e;
            }
            return Unit.Value;
        }
    }
}