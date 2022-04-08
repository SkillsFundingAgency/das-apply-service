using System;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public static class CompaniesHouseValidator
    {
        public static bool CompaniesHouseStatusValid(string ukprn, string companyStatus, ILogger logger)
        {
            // APR-2989 required this to pass even though not 'active'
            if (ukprn == "10043575")
            {
                logger.LogWarning("CompaniesHouseValidator call - bypassing companies house status check for ukprn {ukprn}",ukprn);
                return true;
            }

            if (string.IsNullOrWhiteSpace(companyStatus) || companyStatus.ToLower() == "active")
                return true;

            return false;
        }
    }
}
