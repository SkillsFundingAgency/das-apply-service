using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.UpdatePageAnswers
{
    public class UpdatePageAnswersRequest : IRequest<UpdatePageAnswersResult>
    {
        public Guid ApplicationId { get; }
        public Guid UserId { get; }
        public string PageId { get; }
        public List<Answer> Answers { get; }

        public UpdatePageAnswersRequest(Guid applicationId, Guid userId, string pageId, List<Answer> answers)
        {
            ApplicationId = applicationId;
            UserId = userId;
            PageId = pageId;
            Answers = answers;
        }
    }
}