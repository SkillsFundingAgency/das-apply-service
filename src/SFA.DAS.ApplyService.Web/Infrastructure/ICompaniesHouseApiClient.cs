using SFA.DAS.ApplyService.Domain.CompaniesHouse;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public interface ICompaniesHouseApiClient
    {
        Task<CompaniesHouseSummary> GetCompanyDetails(string companiesHouseNumber);
    }
}
