using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public interface IApplyRepository
    {


        Task<List<Domain.Entities.Application>> GetApplications(Guid userId);
        Task<ApplicationSection> GetSection(Guid applicationId, int sequenceId,  int sectionId, Guid? userId);
        Task<ApplicationSequence> GetActiveSequence(Guid applicationId);
        Task<List<Asset>> GetAssets();
        Task<Guid> CreateApplication(string applicationType, Guid applyingOrganisationId, Guid userId, Guid workflowId);
        Task<Guid> GetLatestWorkflow(string applicationType);
        Task<List<ApplicationSection>> CopyWorkflowToApplication(Guid applicationId, Guid workflowId, string organisationType);
        Task UpdateSections(List<ApplicationSection> sections);
        Task SaveSection(ApplicationSection section, Guid? userId = null);
        Task<Guid> CreateNewWorkflow(string workflowType);
        Task CreateSequence(Guid workflowId, double sequenceId, bool isActive);
        Task CreateSection(WorkflowSection section);
        Task AddAssets(Dictionary<string,string> assets);
        Task<List<dynamic>> GetNewApplications();
        Task SubmitApplicationSequence(ApplicationSubmitRequest request);
        Task UpdateSequenceStatus(Guid applicationId, int sequenceId, string status, string applicationStatus);
        Task CloseSequence(Guid applicationId, int sequenceId);
        Task<List<ApplicationSequence>> GetSequences(Guid applicationId);
        Task OpenSequence(Guid applicationId, int nextSequenceId);
        Task UpdateApplicationData(Guid requestApplicationId, string serialisedData);
        Task<Domain.Entities.Application> GetApplication(Guid requestApplicationId);
        Task UpdateApplicationStatus(Guid applicationId, string status);
        Task<List<ApplicationSection>> GetSections(Guid applicationId);
        Task<List<dynamic>> GetNewFinancialApplications();
        Task StartApplicationReview(Guid applicationId, int sectionId);
        Task StartFinancialReview(Guid applicationId);
        Task<Organisation> GetOrganisationForApplication(Guid applicationId);
        Task<List<dynamic>> GetPreviousFinancialApplications();

        Task ClearAssets();
        Task<List<ApplicationSection>> GetApplicationSections();
        Task<List<WorkflowSection>> GetWorkflowSections();
    }
}