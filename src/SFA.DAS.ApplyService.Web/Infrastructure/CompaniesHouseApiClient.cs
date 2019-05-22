using SFA.DAS.ApplyService.InternalApi.Types.CompaniesHouse;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class CompaniesHouseApiClient : ICompaniesHouseApiClient
    {
        public Company GetCompanyDetails(string companiesHouseNumber)
        {
            // Implementation of call to API in story APR-448
            return new Company {CompanyNumber = companiesHouseNumber};
        }
    }
}
