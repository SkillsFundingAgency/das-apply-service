﻿using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Gateway.Applications
{
    public class InProgressGatewayApplicationsRequest : IRequest<List<RoatpGatewaySummaryItem>>
    {
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
    }
}
