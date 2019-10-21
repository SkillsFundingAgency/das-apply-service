using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Configuration
{
    public class CustomValidationConfiguration
    {
        public string PageId { get; set; }
        public string QuestionId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string ErrorMessage { get; set; }
    }
}
