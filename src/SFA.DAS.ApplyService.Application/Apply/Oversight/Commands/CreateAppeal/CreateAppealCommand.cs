using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.Models;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Commands.CreateAppeal
{
	public class CreateAppealCommand : IRequest
	{
        public Guid OversightReviewId { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
	}
}
