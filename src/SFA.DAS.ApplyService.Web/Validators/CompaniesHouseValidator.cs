using System;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public static class CompaniesHouseValidator
    {
        public static bool CompaniesHouseStatusValid(string companyNumber, string companyStatus)
        {
            if (String.IsNullOrWhiteSpace(companyStatus) || companyStatus.ToLower() == "active")
            {
                return true;
            }
            
            return false;
        }
    }
}
