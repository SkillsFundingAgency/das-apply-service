namespace SFA.DAS.ApplyService.Web.Validators
{
    public class UkprnValidator
    {
        public static bool IsValidUkprn(string stringToCheck, out long ukprn)
        {
            if (!long.TryParse(stringToCheck, out ukprn))
            {
                return false;
            }

            return ukprn >= 10000000 && ukprn <= 99999999;
        }
    }
}
