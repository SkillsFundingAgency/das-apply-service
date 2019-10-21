
namespace SFA.DAS.ApplyService.Web.Services
{
    public static class StringExtensions
    {
        public static bool StartsWithVowel(this string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 1)
            {
                return false;
            }

            var firstLetter = value.Substring(0, 1).ToUpper();
            if (firstLetter == "A" || firstLetter == "E" || firstLetter == "I" || firstLetter == "O" || firstLetter == "U")
            {
                return true;
            }

            return false;
        }
    }
}
