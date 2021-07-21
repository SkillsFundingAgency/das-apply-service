﻿using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Financial.Applications
{
    public class ClarificationFinancialApplicationsRequest : IRequest<List<RoatpFinancialSummaryItem>>
    {
        public ClarificationFinancialApplicationsRequest(string searchTerm, string sortColumn, string sortOrder)
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
