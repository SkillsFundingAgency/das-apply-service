using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.UpdateFileAnswer
{
    public class UpdateFileAnswerRequest : IRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid UserId { get; set; }
        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public string PageId { get; set; }
        public string QuestionId { get; set; }
        public string FileName { get; set; }
    }
}