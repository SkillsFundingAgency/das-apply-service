using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public interface IApplyRepository
    {
        Task<Workflow> GetCurrentWorkflow(string requestApplicationType, Guid requestApplyingOrganisationId);
        Task SetOrganisationApplication(Workflow workflow, Guid applyingOrganisationId, string username);
        Task<Entity> GetEntity(Guid applicationId, Guid userId);
        Task SaveEntity(Entity entity, Guid applicationId, Guid userId);
    }
}