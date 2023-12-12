using FluentValidation;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public class AddUserDetailsViewModelValidator : AbstractValidator<AddUserDetailsViewModel>
    {
        public const string FirstNameMinLengthError = "Enter your first name";
        public const string FirstNameMaxLengthError = "Enter your first name using 250 characters or less";

        public const string LastNameMinLengthError = "Enter your last name";
        public const string LastNameMaxLengthError = "Enter your last name using 250 characters or less";

        public AddUserDetailsViewModelValidator()
        {
            RuleFor(x => x.FirstName)
                 .NotEmpty().WithMessage(FirstNameMinLengthError)
                 .MaximumLength(250).WithMessage(FirstNameMaxLengthError);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(LastNameMinLengthError)
                .MaximumLength(250).WithMessage(LastNameMaxLengthError);
        }
    }
}
