using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public static class DateOfBirthValidator
    {
        public static List<ValidationErrorDetail> ValidateDateOfBirth(Answer dobMonth, Answer dobYear)
        {
            return new List<ValidationErrorDetail>();
        }
    }
}
