namespace SFA.DAS.ApplyService.Web.Validators
{
    using System;
    using SFA.DAS.ApplyService.Domain.Roatp;

    public static class IncorporationDateValidator
    {
        public static bool IsValidIncorporationDate(int applicationRouteId, DateTime? incorporationDate)
        {
            if (!incorporationDate.HasValue)
            {
                return false;
            }

            switch (applicationRouteId)
            {
                case ApplicationRoute.SupportingProviderApplicationRoute:
                {
                    return (incorporationDate <= DateTime.Today.AddMonths(-3));
                }
                case ApplicationRoute.MainProviderApplicationRoute:
                case ApplicationRoute.EmployerProviderApplicationRoute:
                {
                    return (incorporationDate <= DateTime.Today.AddMonths(-12));
                }
            }

            return false;
        }
    }
}
