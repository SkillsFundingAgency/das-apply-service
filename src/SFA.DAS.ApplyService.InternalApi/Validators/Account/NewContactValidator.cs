using FluentValidation;
using Microsoft.Extensions.Localization;
using SFA.DAS.ApplyService.InternalApi.Types;

namespace SFA.DAS.ApplyService.InternalApi.Validators.Account
{
    public class NewContactValidator : AbstractValidator<NewContact>
    {
        public NewContactValidator(IStringLocalizer<NewContact> localizer)
        {
            RuleFor(vm => vm.Email).EmailAddress().WithMessage(localizer["Email must be valid"])
                .NotEmpty().WithMessage(localizer["Email must not be empty"]);
            RuleFor(vm => vm.FamilyName).NotEmpty().WithMessage(localizer["Last name must not be empty"]);
            RuleFor(vm => vm.GivenName).NotEmpty().WithMessage(localizer["First name must not be empty"]);
        }
    }
}