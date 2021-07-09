﻿using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class ClosedAssessorApplicationsRequest : IRequest<List<ClosedApplicationSummary>>
    {
        public ClosedAssessorApplicationsRequest(string userId, string sortOrder, string sortColumn)
        {
            UserId = userId;
            SortOrder = sortOrder;
            SortColumn = sortColumn;
        }

        public string SortColumn { get; set; }
        public string UserId { get; }
        public string SortOrder { get; }
    }
}
