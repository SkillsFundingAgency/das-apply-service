namespace SFA.DAS.ApplyService.Domain.Roatp
{
    public class OrganisationRegisterStatus
    {
        public bool ExistingUKPRN { get; set; }
        public int ProviderTypeId { get; set; }
        public int StatusId { get; set; }

        public const int ActiveStatus = 1;
        public const int RemovedStatus = 0;
    }
}
