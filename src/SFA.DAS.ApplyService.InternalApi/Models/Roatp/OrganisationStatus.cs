namespace SFA.DAS.ApplyService.InternalApi.Models.Roatp
{
    public class OrganisationStatus
    {
        public int Id { get; set; }
        public string Status { get; set; }

        public const int Removed = 0;
        public const int Active = 1;
        public const int ActiveNotTakingOnNewApprentices = 2;
        public const int Onboarding = 3;
    }
}
