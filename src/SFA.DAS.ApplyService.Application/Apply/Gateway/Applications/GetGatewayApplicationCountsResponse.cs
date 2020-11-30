namespace SFA.DAS.ApplyService.Application.Apply.Gateway.Applications
{
    public class GetGatewayApplicationCountsResponse
    {
        public int NewApplicationsCount { get; set; }
        public int InProgressApplicationsCount { get; set; }
        public int ClosedApplicationsCount { get; set; }
    }
}