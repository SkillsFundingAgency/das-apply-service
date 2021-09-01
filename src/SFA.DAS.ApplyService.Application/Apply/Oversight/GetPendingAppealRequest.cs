﻿using MediatR;
using SFA.DAS.ApplyService.Domain.QueryResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class GetPendingAppealRequest : IRequest<PendingAppealOutcomes>
    {
        public GetPendingAppealRequest(string searchTerm, string sortColumn, string sortOrder)
        {
            SearchTerm = searchTerm;
            SortColumn = sortColumn;
            SortOrder = sortOrder;
        }

        public string SearchTerm { get; }
        public string SortColumn { get; }
        public string SortOrder { get; }
    }
}
