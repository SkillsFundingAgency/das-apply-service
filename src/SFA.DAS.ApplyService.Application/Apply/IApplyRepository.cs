using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public interface IApplyRepository
    {
        Task<Guid> StartApplication(Guid applicationId, ApplyData applyData, Guid organisationId, Guid createdBy);

        Task<Domain.Entities.Apply> GetApplication(Guid applicationId);
        Task<List<Domain.Entities.Apply>> GetUserApplications(Guid userId);
        Task<List<Domain.Entities.Apply>> GetOrganisationApplications(Guid userId);

        Task<bool> CanSubmitApplication(Guid applicationId);
        Task SubmitApplication(Guid applicationId, ApplyData applyData, Guid submittedBy);

        Task<bool> ChangeProviderRoute(Guid applicationId, int ProviderRoute);


        // NOTE: This is old stuff or things which are not migrated over yet
        Task<ApplicationSection> GetSection(Guid applicationId, int sequenceId,  int sectionId, Guid? userId);
        Task<List<ApplicationSection>> GetSections(Guid applicationId, int sequenceId, Guid? userId);
        Task<List<ApplicationSection>> GetSections(Guid applicationId);
        Task<ApplicationSequence> GetSequence(Guid applicationId, int sequenceId, Guid? userId);
        Task<ApplicationSequence> GetActiveSequence(Guid applicationId);
        Task<List<Asset>> GetAssets();
        Task<Guid> CreateApplication(Guid applicationId, string applicationType, Guid applyingOrganisationId, Guid userId, Guid workflowId);
        Task<Guid> GetLatestWorkflow(string applicationType);
        Task<List<ApplicationSection>> CopyWorkflowToApplication(Guid applicationId, Guid workflowId, string organisationType);
        Task UpdateSections(List<ApplicationSection> sections);
        Task UpdateSequences(List<ApplicationSequence> sequences);
        Task SaveSection(ApplicationSection section, Guid? userId = null);
        Task<List<ApplicationSummaryItem>> GetOpenApplications(int sequenceId);
        Task<List<ApplicationSummaryItem>> GetFeedbackAddedApplications();
        Task<List<ApplicationSummaryItem>> GetClosedApplications();
        Task<List<FinancialApplicationSummaryItem>> GetOpenFinancialApplications();
        Task<List<FinancialApplicationSummaryItem>> GetFeedbackAddedFinancialApplications();
        Task<List<FinancialApplicationSummaryItem>> GetClosedFinancialApplications();        
        Task UpdateSequenceStatus(Guid applicationId, int sequenceId, string sequenceStatus, string applicationStatus);
        Task CloseSequence(Guid applicationId, int sequenceId);
        Task<List<ApplicationSequence>> GetSequences(Guid applicationId);
        Task OpenSequence(Guid applicationId, int nextSequenceId);
        Task UpdateApplicationStatus(Guid applicationId, string status);
        Task DeleteRelatedApplications(Guid applicationId);

        Task StartApplicationReview(Guid applicationId, int sectionId);
        Task StartFinancialReview(Guid applicationId);
        Task<Organisation> GetOrganisationForApplication(Guid applicationId);

        Task<string> CheckOrganisationStandardStatus(Guid applicationId, int standardId);

        Task<int> GetNextAppReferenceSequence();
        Task<string> GetWorkflowReferenceFormat(Guid requestApplicationId);

        Task<bool> MarkSectionAsCompleted(Guid applicationId, Guid applicationSectionId);
        Task<bool> IsSectionCompleted(Guid applicationId, Guid applicationSectionId);

        Task RemoveSectionCompleted(Guid applicationId, Guid applicationSectionId);
        
        Task<IEnumerable<RoatpApplicationStatus>> GetExistingApplicationStatusByUkprn(string ukprn);

        Task<string> GetNextRoatpApplicationReference();
    }
}