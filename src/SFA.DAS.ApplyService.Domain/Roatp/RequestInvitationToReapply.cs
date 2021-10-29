namespace SFA.DAS.ApplyService.Domain.Roatp
{
    public class RequestInvitationToReapply
    {
        public string OrganisationName { get; set; }
        public string UKPRN { get; set; }
        public string EmailAddress { get; set; }
    }
}