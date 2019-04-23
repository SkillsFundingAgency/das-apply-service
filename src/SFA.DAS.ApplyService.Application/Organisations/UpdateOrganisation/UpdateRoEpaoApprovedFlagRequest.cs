using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Organisations.UpdateOrganisation
{
    public class UpdateRoEpaoApprovedFlagRequest: IRequest<Organisation>
    {
        public UpdateRoEpaoApprovedFlagRequest(Guid applicationId, Guid contactId, string endPointAssessorOrganisationId, bool roEpaoApprovedFlag)
        {
            ApplicationId = applicationId;
            RoEpaoApprovedFlag = roEpaoApprovedFlag;
            EndPointAssessorOrganisationId = endPointAssessorOrganisationId;
        }

        public Guid ApplicationId { get; }
        public bool RoEpaoApprovedFlag { get; }
        public string EndPointAssessorOrganisationId { get; }
        public Guid ContactId { get; }
    }
}
