using System;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class Answer
    {
        public Guid Id { get; set; }
        public string QuestionId { get; set; }
        public string Value { get; set; }
    }
}