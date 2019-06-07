using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class RegisteredCharityNumberValidator : Validator
    {
        public override List<KeyValuePair<string, string>> Validate(Answer answer)
        {
            var errorMessages = base.Validate(answer);

            if (!string.IsNullOrEmpty(GetValue(answer)) && !IsValidRegisteredCharityNumber(GetValue(answer)))
            {
                errorMessages.Add(new KeyValuePair<string, string>(GetFieldId(answer), ValidationDefinition.ErrorMessage));
            }

            return errorMessages;
        }

        private static bool IsValidRegisteredCharityNumber(string registeredCharityNumber)
        {
            try
            {
                // MFC 28/01/2019 left in cos specific rules unclear
                //var rx = new Regex(@"^[0-9]{7}$");
                //if (registeredCharityNumber.Length==8)
                //    registeredCharityNumber = registeredCharityNumber.Replace("-","");

                var rx = new Regex(@"^[0-9-]{1,}$");
                return rx.IsMatch(registeredCharityNumber);
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
