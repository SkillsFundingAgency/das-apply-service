using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Commands.CreateAppeal
{
	public class CreateAppealCommand : IRequest
	{
        public Guid ApplicationId { get; set; }
        public Guid OversightReviewId { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
	}
}