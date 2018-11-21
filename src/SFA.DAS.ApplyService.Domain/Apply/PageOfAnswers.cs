using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class PageOfAnswers
    {
        public Guid Id { get; set; }
        public List<Answer> Answers { get; set; }
    }
}