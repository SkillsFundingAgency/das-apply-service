namespace SFA.DAS.ApplyService.Web.Validators
{
    using System;
    
    public class UkprnValidator
    {
        public static bool IsValidUkprn(string stringToCheck, out long ukprn)
        {
            if (String.IsNullOrWhiteSpace(stringToCheck))
            {
                ukprn = 0;
                return false;
            }

            if (!long.TryParse(stringToCheck, out ukprn))
            {
                return false;
            }

            if (ukprn >= 10000000 && ukprn <= 99999999)
            {
                return true;
            }

            return false;
        }
    }
}
