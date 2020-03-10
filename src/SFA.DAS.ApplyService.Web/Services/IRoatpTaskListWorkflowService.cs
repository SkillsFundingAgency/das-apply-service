using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface IRoatpTaskListWorkflowService
    {
        string SectionStatus(Guid applicationId, int sequenceId, int sectionId, List<ApplicationSequence> applicationSequences, OrganisationVerificationStatus organisationVerificationStatus);
        bool PreviousSectionCompleted(Guid applicationId, int sequenceId, int sectionId, List<ApplicationSequence> applicationSequences, OrganisationVerificationStatus organisationVerificationStatus);
        bool ApplicationSequencesCompleted(Guid applicationId, List<ApplicationSequence> applicationSequences, OrganisationVerificationStatus organisationVerificationStatus);
        List<ApplicationSequence> GetApplicationSequences(Guid applicationId);
    }
}
