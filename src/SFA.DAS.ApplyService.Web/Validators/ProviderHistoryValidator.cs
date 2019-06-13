namespace SFA.DAS.ApplyService.Web.Validators
{
    using System;
    using SFA.DAS.ApplyService.Domain.Roatp;

    public static class ProviderHistoryValidator
    {
        public static bool HasSufficientHistory(int applicationRouteId, DateTime? startDate)
        {
            if (!startDate.HasValue)
            {
                return false;
            }

            switch (applicationRouteId)
            {
                case ApplicationRoute.SupportingProviderApplicationRoute:
                {
                    return (startDate <= DateTime.Today.AddMonths(-3));
                }
                case ApplicationRoute.MainProviderApplicationRoute:
                case ApplicationRoute.EmployerProviderApplicationRoute:
                {
                    return (startDate <= DateTime.Today.AddMonths(-12));
                }
            }

            return false;
        }
    }
}
