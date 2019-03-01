using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.GetAnswers
{

    public class GetAnswersRequest : IRequest<GetAnswersResponse>
    {
        public Guid ApplicationId { get; }
        public string QuestionIdentifier { get; }
        public GetAnswersRequest(Guid applicationId, string questionIdentifier)
        {
            ApplicationId = applicationId;
            QuestionIdentifier = questionIdentifier;
        }
    }
}
