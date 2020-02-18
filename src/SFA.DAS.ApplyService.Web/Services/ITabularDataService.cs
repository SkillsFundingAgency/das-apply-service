using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface ITabularDataService
    {
        bool IsRowAlreadyPresent(TabularData currentTabularData, TabularDataRow newRow);
        TabularData DeduplicateData(TabularData tabularData);
    }
}
