namespace SFA.DAS.ApplyService.Application.Apply
{
    // Derived from SFA.DAS.QnA.Api.Types, at some point we should be able to use the nuget
    public class GetNextActionResponse
    {
        public string NextAction { get; set; }

        public string NextActionId { get; set; }

        public GetNextActionResponse()
        { }

        public GetNextActionResponse(string nextAction, string nextActionId)
        {
            NextAction = nextAction;
            NextActionId = nextActionId;
        }
    }
}
