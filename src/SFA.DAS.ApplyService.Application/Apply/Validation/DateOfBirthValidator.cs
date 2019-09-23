using System;
using System.Collections.Generic;
using System.Globalization;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class DateOfBirthValidator : DateValidator
    {
        public DateOfBirthValidator()
            : base(new string[] { "M,yyyy" })
        {
        }
    }
}