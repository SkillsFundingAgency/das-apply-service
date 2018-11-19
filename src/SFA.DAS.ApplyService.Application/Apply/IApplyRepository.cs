using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public interface IApplyRepository
    {

//        Task SetOrganisationApplication(Workflow workflow, Guid applyingOrganisationId, string username);
        Task<Domain.Entities.Application> GetEntity(Guid applicationId, Guid userId);
        //Task SaveEntity(Domain.Entities.Application application, Guid applicationId, Guid userId);
        Task<List<Domain.Entities.Application>> GetApplications(Guid userId);
        Task<ApplicationSection> GetSection(Guid applicationId, int sequenceId,  int sectionId, Guid userId);
        Task<List<ApplicationSection>> GetSections(Guid applicationId, int sequenceId, Guid userId);
        Task<List<Asset>> GetAssets();
        Task<Guid> CreateApplication(string applicationType, Guid applyingOrganisationId, Guid userId, Guid workflowId);
        Task<Guid> GetLatestWorkflow(string applicationType);
        Task<List<ApplicationSection>> CopyWorkflowToApplication(Guid applicationId, Guid workflowId, int organisationId);
        Task UpdateSections(List<ApplicationSection> sections);
        Task SaveSection(ApplicationSection section, Guid userId);
        Task<Guid> CreateNewWorkflow(string workflowType);
        Task CreateSequence(Guid workflowId, double sequenceId);
        Task CreateSection(WorkflowSection section);
        Task AddAssets(Dictionary<string,string> assets);
        Task<List<Domain.Entities.Application>> GetApplicationsToReview();
    }
}