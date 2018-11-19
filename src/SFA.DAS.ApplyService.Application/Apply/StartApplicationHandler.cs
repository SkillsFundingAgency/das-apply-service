using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Users;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public class StartApplicationHandler : IRequestHandler<StartApplicationRequest>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IOrganisationRepository _organisationRepository;

        public StartApplicationHandler(IApplyRepository applyRepository, IOrganisationRepository organisationRepository)
        {
            _applyRepository = applyRepository;
            _organisationRepository = organisationRepository;
        }
        
        public async Task<Unit> Handle(StartApplicationRequest request, CancellationToken cancellationToken)
        {
            var assets = await _applyRepository.GetAssets();
            try
            {
                var org = await _organisationRepository.GetUserOrganisation(request.UserId); 
                
                var workflowId = await _applyRepository.GetLatestWorkflow("EPAO");
                var applicationId =
                    await _applyRepository.CreateApplication("EPAO", org.Id, request.UserId, workflowId);

                var sections = await _applyRepository.CopyWorkflowToApplication(applicationId, workflowId, int.Parse(org.OrganisationType));

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