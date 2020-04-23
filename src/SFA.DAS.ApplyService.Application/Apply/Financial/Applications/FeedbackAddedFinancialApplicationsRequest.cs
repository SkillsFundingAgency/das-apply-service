﻿using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Financial.Applications
{
    public class FeedbackAddedFinancialApplicationsRequest : IRequest<List<RoatpFinancialSummaryItem>>
    {
    }
}
