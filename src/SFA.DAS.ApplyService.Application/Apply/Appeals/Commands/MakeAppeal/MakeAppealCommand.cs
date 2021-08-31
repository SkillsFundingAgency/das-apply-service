using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Appeals.Commands.MakeAppeal
{
    public class MakeAppealCommand : IRequest
	{
        public Guid ApplicationId { get; set; }
        public string HowFailedOnPolicyOrProcesses { get; set; }
        public string HowFailedOnEvidenceSubmitted { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}