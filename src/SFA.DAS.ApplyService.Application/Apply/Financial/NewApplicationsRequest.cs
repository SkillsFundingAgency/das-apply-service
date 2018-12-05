using System;
using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class NewApplicationsRequest : IRequest<List<NewApplicationsResponse>>
    {
        
    }

    public class NewApplicationsResponse
    {
        public string ApplyingOrganisationName { get; set; }
        public string Status { get; set; }
        public Guid ApplicationId { get; set; }
    }
}