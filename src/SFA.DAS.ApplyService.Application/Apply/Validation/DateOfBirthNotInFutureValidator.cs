using System;
using System.Collections.Generic;
using System.Globalization;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class DateOfBirthNotInFutureValidator : DateNotInFutureValidator
    {
        public DateOfBirthNotInFutureValidator()
            : base(new string[] { "M,yyyy" })
        {
        }
    }
}