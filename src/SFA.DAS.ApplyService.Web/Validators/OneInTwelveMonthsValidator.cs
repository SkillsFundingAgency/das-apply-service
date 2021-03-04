using FluentValidation;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public class OneInTwelveMonthsValidator : AbstractValidator<OneInTwelveMonthsViewModel>
    {
        public OneInTwelveMonthsValidator()
        {
            RuleFor(x => x.HasOneInTwelveMonths)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Tell us if you've submitted an application to join the RoATP in the last 12 months");
        }
    }
}
