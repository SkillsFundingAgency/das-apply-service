using SFA.DAS.ApplyService.Application.Apply.Validation;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class QuestionViewModel
    {
        public string QuestionId { get; set; }
        public string Type { get; set; }
        public string Label { get; set; }
        public string Hint { get; set; }
        public string Value { get; set; }
        public dynamic Options { get; set; }
        public List<ValidationErrorDetail> ErrorMessages { get; set; }
    }
}