namespace SFA.DAS.ApplyService.InternalApi.Types.Requests
{
    public class EvaluateGatewayApplicationRequest
    {
        public bool IsGatewayApproved { get; set; }
        public string EvaluatedBy { get; set; }
    }
}
