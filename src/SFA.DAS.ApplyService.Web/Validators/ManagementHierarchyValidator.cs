using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using FluentValidation;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public class ManagementHierarchyValidator : AbstractValidator<AddEditManagementHierarchyViewModel>
    {
        public const string FirstNameMinLengthError = "Enter first name";
        public const string FirstNameMaxLengthError = "Enter a first name using 255 characters or less";

        public const string LastNameMinLengthError = "Enter last name";
        public const string LastNameMaxLengthError = "Enter a last name using 255 characters or less";

        public const string JobRoleMinLengthError = "Enter a job role";
        public const string JobRoleMaxLengthError = "Enter a job role using 255 characters or less";

        public const string TimeInRoleError = "Enter a year and month";
        public const string TimeInRoleMonthsTooBigError = "Enter 11 or less for month";
        public const string TimeInRoleYearsTooBigError = "Enter 99 or less for year";

        public const string IsPartOfOtherOrgThatGetsFundingError = "Tell us if this person is part of any other organisations";
        public const string OtherOrgNameError = "Enter the names of all these organisations";
        public const string OtherOrgNameLengthError = "Enter the names of all these organisations using 750 characters or less";

        public const string ValidEmailPattern = EmailValidation.RegexPattern;
        public const string EmailError = "Enter your email";
        public const string EmailErrorTooLong = "Enter your email using 320 characters or less";
        public const string EmailErrorWrongFormat = "Enter your email address using an acceptable format (eg name@location.com)";

        public const string ContactNumberError = "Enter your contact number";
        public const string ContactNumberTooLongError = "Enter your contact number using 50 characters or less";

        public ManagementHierarchyValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(FirstNameMinLengthError)
                .MaximumLength(255).WithMessage(FirstNameMaxLengthError);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(LastNameMinLengthError)
                .MaximumLength(255).WithMessage(LastNameMaxLengthError);

            RuleFor(x => x.JobRole)
                .NotEmpty().WithMessage(JobRoleMinLengthError)
                .MaximumLength(255).WithMessage(JobRoleMaxLengthError);

            RuleFor(x => x.TimeInRoleMonths)
                .Transform(value => long.TryParse(value, out long val) ? (long?)val : null)
                .NotEmpty().WithMessage(TimeInRoleError)
                .LessThanOrEqualTo(11).WithMessage(TimeInRoleMonthsTooBigError)
                .DependentRules(() =>
                {
                    RuleFor(x => x.TimeInRoleYears)
                        .Transform(value => long.TryParse(value, out long val) ? (long?)val : null)
                        .NotEmpty().WithMessage(TimeInRoleError)
                        .LessThanOrEqualTo(99).WithMessage(TimeInRoleYearsTooBigError);
                });

            RuleFor(x => x.IsPartOfOtherOrgThatGetsFunding)
                .NotEmpty().WithMessage(IsPartOfOtherOrgThatGetsFundingError)
                .DependentRules(() =>
                {
                    RuleFor(x => x.OtherOrgName)
                        .NotEmpty().When(x => x.IsPartOfOtherOrgThatGetsFunding == "Yes").WithMessage(OtherOrgNameError)
                        .MaximumLength(750).When(x => x.IsPartOfOtherOrgThatGetsFunding == "Yes").WithMessage(OtherOrgNameLengthError);
                });

            RuleFor(x => x)
                .Custom((viewModel, content) =>
                {
                    // maybe move away from this and use Transform trick
                    const string DobFieldPrefix = "Dob";
                    var dobErrorMessages = DateOfBirthAnswerValidator.ValidateDateOfBirth(viewModel.DobMonth, viewModel.DobYear, DobFieldPrefix);
                    foreach(var error in dobErrorMessages)
                    {
                        content.AddFailure(error.Field, error.ErrorMessage);
                    }
                });

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(EmailError)
                .MaximumLength(320).WithMessage(EmailErrorTooLong)
                .Matches(ValidEmailPattern).WithMessage(EmailErrorWrongFormat);

            RuleFor(x => x.ContactNumber)
                .NotEmpty().WithMessage(ContactNumberError)
                .MaximumLength(50).WithMessage(ContactNumberTooLongError);
        }
    }
}
