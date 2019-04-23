using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Organisations.UpdateOrganisation
{
    public class UpdateRoEpaoApprovedFlagHandler: IRequestHandler<UpdateRoEpaoApprovedFlagRequest,Organisation>
    {
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IApplyRepository _applyRepository;

        public UpdateRoEpaoApprovedFlagHandler(IOrganisationRepository organisationRepository, IApplyRepository applyRepository)
        {
            _organisationRepository = organisationRepository;
            _applyRepository = applyRepository;
        }

        public async Task<Organisation> Handle(UpdateRoEpaoApprovedFlagRequest request, CancellationToken cancellationToken)
        {
                var existingOrganisation = await _organisationRepository.GetOrganisationByApplicationId(request.ApplicationId);

                if (existingOrganisation != null)
                {
                    existingOrganisation.OrganisationDetails.EndPointAssessmentOrgId =
                        request.EndPointAssessorOrganisationId;
                    existingOrganisation.RoEPAOApproved = request.RoEpaoApprovedFlag;

                   return await _organisationRepository.UpdateOrganisation(existingOrganisation, request.ContactId);
                }

                return null;
        }
    }
}
