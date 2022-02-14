using FluentValidation;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public class CreateAccountViewModelValidator : AbstractValidator<CreateAccountViewModel>
    {
        public const string FirstNameMinLengthError = "Enter your first name";
        public const string FirstNameMaxLengthError = "Enter your first name using 250 characters or less";

        public const string LastNameMinLengthError = "Enter your last name";
        public const string LastNameMaxLengthError = "Enter your last name using 250 characters or less";

        public const string ValidEmailPattern = EmailValidation.RegexPattern;
        public const string EmailError = "Enter your email";
        public const string EmailErrorTooLong = "Enter your email using 256 characters or less";

        public const string EmailErrorWrongFormat =
            "Enter your email address using an acceptable format (eg name@location.com)";

        public CreateAccountViewModelValidator()
        {
            RuleFor(x => x.GivenName)
                 .NotEmpty().WithMessage(FirstNameMinLengthError)
                 .MaximumLength(250).WithMessage(FirstNameMaxLengthError);

            RuleFor(x => x.FamilyName)
                .NotEmpty().WithMessage(LastNameMinLengthError)
                .MaximumLength(250).WithMessage(LastNameMaxLengthError);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(EmailError)
                .MaximumLength(256).WithMessage(EmailErrorTooLong)
                .Matches(ValidEmailPattern).WithMessage(EmailErrorWrongFormat);
        }
    }
}
