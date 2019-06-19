namespace SFA.DAS.ApplyService.Domain.Roatp
{
    public class ApplicationRoute
    {
        public int Id { get; set; }
        public string RouteName { get; set; }
        
        public const int MainProviderApplicationRoute = 1;
        public const int EmployerProviderApplicationRoute = 2;
        public const int SupportingProviderApplicationRoute = 3;
    }
}
