using System;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public static class ManagementHierarchyValidator
    {
        public const string FullNameMinLengthError = "Enter a full name";
        public const string ManagementHierarchyNameMaxLengthError = "Enter a full name using 255 characters or less";
        public const string JobRoleMinLengthError = "Enter a job title";
        public const string JobRoleMaxLengthError = "Enter a job title using 255 characters or less";

        public static List<ValidationErrorDetail> Validate(AddEditManagementHierarchyViewModel model)
        {
            var errorMessages = new List<ValidationErrorDetail>();
                        
            if (string.IsNullOrEmpty(model.FullName))
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = FullNameMinLengthError,
                        Field = "FullName"
                    });
            }
            else if (model.FullName.Length > 255)
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = ManagementHierarchyNameMaxLengthError,
                        Field = "FullName"
                    });
            }


            if (string.IsNullOrEmpty(model.JobRole))
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = JobRoleMinLengthError,
                        Field = "JobRole"
                    });
            }
            else if (model.FullName.Length > 255)
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = JobRoleMaxLengthError,
                        Field = "JobRole"
                    });
            }


            return errorMessages;
        }
    }
}
