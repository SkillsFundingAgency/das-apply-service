using System;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class Feedback
    {
        public string From { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
    }
}