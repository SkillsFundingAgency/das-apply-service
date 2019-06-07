using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class EmailAddressIsValidValidator : Validator
    {
        public override List<KeyValuePair<string, string>> Validate(Answer answer)
        {
            var errorMessages = base.Validate(answer);

            var value = GetValue(answer);
            if(!string.IsNullOrEmpty(value) && !IsValidEmail(value))
            {
                errorMessages.Add(new KeyValuePair<string, string>(GetFieldId(answer), ValidationDefinition.ErrorMessage));
            }

            return errorMessages;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var rx = new Regex(
                    @"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
                return rx.IsMatch(email);
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
