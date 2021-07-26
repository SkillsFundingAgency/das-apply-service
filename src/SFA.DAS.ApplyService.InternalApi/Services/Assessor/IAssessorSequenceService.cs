using SFA.DAS.ApplyService.InternalApi.Types.Assessor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services.Assessor
{
    public interface IAssessorSequenceService
    {
        bool IsValidSequenceNumber(int sequenceNumber);
        Task<List<AssessorSequence>> GetSequences(Guid applicationId);
    }
}