using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface ITabularDataRepository
    {
        Task<TabularData> GetTabularDataAnswer(Guid applicationId, string questionTag);
        Task<bool> SaveTabularDataAnswer(Guid applicationId, Guid sectionId, string pageId, string questionId, TabularData tabularData);
        Task<bool> UpsertTabularDataRecord(Guid applicationId, Guid sectionId, string pageId, string questionId, string questionTag, TabularDataRow record);
        Task<bool> EditTabularDataRecord(Guid applicationId, Guid sectionId, string pageId, string questionId, string questionTag, TabularDataRow record, int index);
    }
}
