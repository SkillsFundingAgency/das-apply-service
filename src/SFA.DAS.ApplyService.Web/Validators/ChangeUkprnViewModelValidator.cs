using FluentValidation;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public class ChangeUkprnViewModelValidator : AbstractValidator<ChangeUkprnViewModel>
    {
        public ChangeUkprnViewModelValidator()
        {
            RuleFor(x => x.Confirmed).NotNull().WithMessage("Tell us if you're sure you want to change your organisation’s UKPRN");
        }
    }
}
