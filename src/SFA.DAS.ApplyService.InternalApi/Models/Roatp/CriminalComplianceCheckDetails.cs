using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Models.Roatp
{
    public class CriminalComplianceCheckDetails
    {
        public string Title { get; set; }
        public string PageId { get; set; }
        public string QuestionId { get; set; }
        public string Answer { get; set; }
        public string FurtherQuestionId { get; set; }
        public string FurtherAnswer { get; set; }
        public string QuestionText { get; set; }
    }
}
