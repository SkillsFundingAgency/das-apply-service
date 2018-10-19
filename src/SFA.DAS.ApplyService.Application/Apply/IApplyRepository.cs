using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public interface IApplyRepository
    {
        Task<Workflow> GetCurrentWorkflow(string requestApplicationType, Guid requestApplyingOrganisationId);
        Task SetOrganisationApplication(Workflow workflow, Guid applyingOrganisationId, string username);
    }
}