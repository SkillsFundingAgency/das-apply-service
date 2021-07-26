namespace SFA.DAS.ApplyService.Web.Validators
{    
    public static class UkprnValidator
    {
        public static bool IsValidUkprn(string stringToCheck, out int ukprn)
        {
            if (!int.TryParse(stringToCheck, out ukprn))
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
