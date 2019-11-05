using System;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public static class PartnerDetailsValidator
    {
        public const string PartnerNameMinLengthErrorIndividual = "Enter a full name";
        public const string PartnerNameMinLengthErrorOrganisation = "Enter the partner organisation’s name";
        public const string PartnerNameMaxLengthError = "Enter a full name using 255 characters or less";
        public const string DobFieldPrefix = "PartnerDob";

        public static List<ValidationErrorDetail> Validate(AddEditPartnerViewModel model)
        {
            var errorMessages = new List<ValidationErrorDetail>();

            if (String.IsNullOrWhiteSpace(model.PartnerName)
                && String.IsNullOrWhiteSpace(model.PartnerDobMonth)
                && String.IsNullOrWhiteSpace(model.PartnerDobYear))
            {
                var organisationType = "organisation";
                if (model.PartnerTypeIndividual)
                {
                    organisationType = "individual";
                }
                
                var message = $"Enter the {organisationType}'s details";

                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = message,
                        Field = "PartnerName"
                    });

                return errorMessages; 
            }
            
            if (String.IsNullOrEmpty(model.PartnerName))
            {
                var message = PartnerNameMinLengthErrorOrganisation;
                if (model.PartnerTypeIndividual)
                {
                    message = PartnerNameMinLengthErrorIndividual;
                }
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = message,
                        Field = "PartnerName"
                    });
            }
            else if (model.PartnerName.Length > 255)
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = PartnerNameMaxLengthError,
                        Field = "PartnerName"
                    });
            }

            var dobErrorMessages = DateOfBirthAnswerValidator.ValidateDateOfBirth(model.PartnerDobMonth, model.PartnerDobYear, DobFieldPrefix);
            if (dobErrorMessages.Any() && model.PartnerTypeIndividual)
            {
                errorMessages.AddRange(dobErrorMessages);
            }

            return errorMessages;
        }
    }
}
