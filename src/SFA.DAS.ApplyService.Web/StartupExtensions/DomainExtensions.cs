namespace SFA.DAS.ApplyService.Web.StartupExtensions
{
    public static class DomainExtensions
    {
        public static string GetDomain(string environment)
        {
            if (environment.ToLower() == "local")
            {
                return "";
            }

            var environmentPart = environment.ToLower() == "prd" ? "" : $"{environment.ToLower()}-";
            return $"{environmentPart}apply.apprenticeships.education.gov.uk";
        }
    }   
}