using System;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public static class ManagementHierarchyValidator
    {
        public const string FullNameMinLengthError = "Enter a full name";
        public const string ManagementHierarchyNameMaxLengthError = "Enter a full name using 255 characters or less";
        public const string JobRoleMinLengthError = "Enter a job role";
        public const string JobRoleMaxLengthError = "Enter a job role using 255 characters or less";
        public const string TimeInRoleError = "Enter a year and month";
        public const string TimeInRoleMonthsTooBigError = "Enter 11 or less for month";
        public const string TimeInRoleYearsTooBigError = "Enter 99 or less for year";
        public const string IsPartOfOtherOrgThatGetsFundingError = "Tell us if this person is part of another organisation that receives funding directly from ESFA or as a subcontractor";
        public const string OtherOrgNameError = "Enter the organisation's name";
        public const string OtherOrgNameLengthError = "Enter the organisation's name using 750 characters or less";

        public static List<ValidationErrorDetail> Validate(AddEditManagementHierarchyViewModel model)
        {
            var errorMessages = new List<ValidationErrorDetail>();
                        
            EvaluateFullName(model, errorMessages);
            EvaluateJobRole(model, errorMessages);
            EvaluateTimeInRole(model, errorMessages);
            EvaluateOrganisationDetails(model, errorMessages);

            return errorMessages;
        }

        private static void EvaluateOrganisationDetails(AddEditManagementHierarchyViewModel model, List<ValidationErrorDetail> errorMessages)
        {
            if (string.IsNullOrEmpty(model.IsPartOfOtherOrgThatGetsFunding))
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = IsPartOfOtherOrgThatGetsFundingError,
                        Field = "IsPartOfOtherOrgThatGetsFunding"
                    });
            }
            else if (model.IsPartOfOtherOrgThatGetsFunding == "Yes") 
            {
                if (string.IsNullOrEmpty(model.OtherOrgName))
                {
                    errorMessages.Add(
                        new ValidationErrorDetail
                        {
                            ErrorMessage = OtherOrgNameError,
                            Field = "IsPartOfOtherOrgThatGetsFunding"
                        });
                }
                else if (model.OtherOrgName.Length>750)
                {
                    errorMessages.Add(
                        new ValidationErrorDetail
                        {
                            ErrorMessage = OtherOrgNameLengthError,
                            Field = "IsPartOfOtherOrgThatGetsFunding"
                        });
                }
            }
        }


        private static void EvaluateFullName(AddEditManagementHierarchyViewModel model, ICollection<ValidationErrorDetail> errorMessages)
        {
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
        }

        private static void EvaluateJobRole(AddEditManagementHierarchyViewModel model, List<ValidationErrorDetail> errorMessages)
        {
            if (string.IsNullOrEmpty(model.JobRole))
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = JobRoleMinLengthError,
                        Field = "JobRole"
                    });
            }
            else if (model.JobRole.Length > 255)
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = JobRoleMaxLengthError,
                        Field = "JobRole"
                    });
            }
        }

        private static void EvaluateTimeInRole(AddEditManagementHierarchyViewModel model, List<ValidationErrorDetail> errorMessages)
        {
            if (string.IsNullOrEmpty(model.TimeInRoleMonths) || string.IsNullOrEmpty(model.TimeInRoleYears))
            {
                errorMessages.Add(
                    new ValidationErrorDetail
                    {
                        ErrorMessage = TimeInRoleError,
                        Field = "TimeInRole"
                    });
            }
            else
            {
                var monthsOk = int.TryParse(model.TimeInRoleMonths, out var months);
                var yearsOk = int.TryParse(model.TimeInRoleYears, out var years);

                if (!monthsOk || !yearsOk || months + years <= 0)
                {
                    errorMessages.Add(
                        new ValidationErrorDetail
                        {
                            ErrorMessage = TimeInRoleError,
                            Field = "TimeInRole"
                        });
                }
                else
                {
                    if (months > 11)
                    {
                        errorMessages.Add(
                            new ValidationErrorDetail
                            {
                                ErrorMessage = TimeInRoleMonthsTooBigError,
                                Field = "TimeInRole"
                            });
                    }

                    if (years > 99)
                    {
                        errorMessages.Add(
                            new ValidationErrorDetail
                            {
                                ErrorMessage = TimeInRoleYearsTooBigError,
                                Field = "TimeInRole"
                            });
                    }
                }
            }
        }
    }
}
