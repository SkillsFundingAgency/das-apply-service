using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Services.Assessor
{
    public interface IAssessorPageService
    {
        Task<AssessorPage> GetPage(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId);
    }
}
