﻿namespace SFA.DAS.ApplyService.InternalApi.Types.Requests
{
    public class EvaluateGatewayApplicationRequest
    {
        public bool IsGatewayApproved { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
