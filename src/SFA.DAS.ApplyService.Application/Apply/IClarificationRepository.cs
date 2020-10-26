using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public interface IClarificationRepository
    {
        Task<List<ClarificationPageReviewOutcome>> GetAllClarificationPageReviewOutcomes(Guid applicationId);
    }
}
