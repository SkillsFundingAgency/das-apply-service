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
        public int SequenceId { get; }
        public int SectionId { get; }
        public string PageId { get; }
        public List<Answer> Answers { get; }
        public bool SaveNewAnswers { get; }

        public UpdatePageAnswersRequest(Guid applicationId, Guid userId, int sequenceId, int sectionId, string pageId, List<Answer> answers, bool saveNewAnswers)
        {
            ApplicationId = applicationId;
            UserId = userId;
            SequenceId = sequenceId;
            SectionId = sectionId;
            PageId = pageId;
            Answers = answers;
            SaveNewAnswers = saveNewAnswers;
        }
    }
}