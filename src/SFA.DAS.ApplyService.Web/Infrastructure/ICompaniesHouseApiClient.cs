using SFA.DAS.ApplyService.InternalApi.Types.CompaniesHouse;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public interface ICompaniesHouseApiClient
    {
        Company GetCompanyDetails(string companiesHouseNumber);
    }
}
