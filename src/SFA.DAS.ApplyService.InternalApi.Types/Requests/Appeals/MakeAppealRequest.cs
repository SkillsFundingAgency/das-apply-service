namespace SFA.DAS.ApplyService.InternalApi.Types.Requests.Appeals
{
    public class MakeAppealRequest
    {
        public string HowFailedOnPolicyOrProcesses { get; set; }
        public string HowFailedOnEvidenceSubmitted { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
