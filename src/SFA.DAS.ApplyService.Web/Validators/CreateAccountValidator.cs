using FluentValidation;
using Microsoft.Extensions.Localization;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountViewModel>
    {
        public CreateAccountValidator(IStringLocalizer<CreateAccountViewModel> localizer)
        {
            RuleFor(vm => vm.Email).EmailAddress().WithMessage(localizer["Email must be valid"])
                .NotEmpty().WithMessage(localizer["Email must not be empty"]);
            RuleFor(vm => vm.FamilyName).NotEmpty().WithMessage(localizer["Last Name must not be empty"]);
            RuleFor(vm => vm.GivenName).NotEmpty().WithMessage(localizer["First Name must not be empty"]);
        }
    }
}