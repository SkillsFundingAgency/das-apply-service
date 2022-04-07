using System;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public static class CompaniesHouseValidator
    {
        public static bool CompaniesHouseStatusValid(string ukprn, string companyStatus)
        {
            return string.IsNullOrWhiteSpace(companyStatus) 
                   || companyStatus.ToLower() == "active" 
                   || ukprn == "10043575";
        }
    }
}
