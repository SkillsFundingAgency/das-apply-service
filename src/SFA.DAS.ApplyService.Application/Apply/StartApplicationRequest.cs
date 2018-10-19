using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public class StartApplicationRequest : IRequest
    {
        public string ApplicationType { get; }
        public Guid ApplyingOrganisationId { get; }
        public string Username { get; set; }

        public StartApplicationRequest(string applicationType, Guid applyingOrganisationId, string username)
        {
            ApplicationType = applicationType;
            ApplyingOrganisationId = applyingOrganisationId;
            Username = username;
        }
    }
}