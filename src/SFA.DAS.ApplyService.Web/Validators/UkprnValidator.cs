namespace SFA.DAS.ApplyService.Web.Validators
{
    using System;
    using Resources;

    public class UkprnValidator
    {
        public static string IsValidUkprn(string stringToCheck, out long ukprn)
        {
            if (String.IsNullOrWhiteSpace(stringToCheck))
            {
                ukprn = 0;
                return UkprnValidationMessages.MissingUkprn;
            }

            if (!long.TryParse(stringToCheck, out ukprn))
            {
                return UkprnValidationMessages.InvalidUkprn;
            }

            if (ukprn >= 10000000 && ukprn <= 99999999)
            {
                return string.Empty;
            }

            return UkprnValidationMessages.InvalidUkprn;
        }
    }
}
