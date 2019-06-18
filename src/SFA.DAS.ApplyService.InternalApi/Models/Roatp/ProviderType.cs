namespace SFA.DAS.ApplyService.InternalApi.Models.Roatp
{
    public class ProviderType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public const int MainProvider = 1;
        public const int EmployerProvider = 2;
        public const int SupportingProvider = 3;
    }
}
