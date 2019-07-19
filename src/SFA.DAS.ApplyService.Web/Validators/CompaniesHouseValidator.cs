using System;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public static class CompaniesHouseValidator
    {
        private static string[] StatusOnlyCompanyNumberPrefixes = new[] { "IP", "SP", "IC", "SI", "NP", "NV", "RC", "SR", "NR", "NO" };

        public static bool CompaniesHouseStatusValid(string companyNumber, string companyStatus)
        {
            bool companyReturnsStatus = true;

            foreach (var prefix in StatusOnlyCompanyNumberPrefixes)
            {
                if (companyNumber.ToUpper().StartsWith(prefix))
                {
                    companyReturnsStatus = false;
                    break;
                }
            }

            if (companyReturnsStatus)
            {
                if (companyStatus != null && companyStatus.ToLower() == "active")
                {
                    return true;
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(companyStatus) || companyStatus.ToLower() == "active")
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
