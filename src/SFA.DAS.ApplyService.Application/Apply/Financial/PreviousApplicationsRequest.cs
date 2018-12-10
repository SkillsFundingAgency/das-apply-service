using System;
using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class PreviousApplicationsRequest : IRequest<List<PreviousApplicationsResponse>>
    {
        
    }

    public class PreviousApplicationsResponse
    {
        public string ApplyingOrganisationName { get; set; }
        public string GradedBy { get; set; }
        public DateTime GradedDateTime { get; set; }
        public string Grade { get; set; }
        public Guid ApplicationId { get; set; }
    }
}