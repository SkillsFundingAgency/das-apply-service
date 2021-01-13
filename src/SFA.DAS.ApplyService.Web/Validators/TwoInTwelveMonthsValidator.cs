using FluentValidation;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public class TwoInTwelveMonthsValidator : AbstractValidator<TwoInTwelveMonthsViewModel>
    {
        public TwoInTwelveMonthsValidator()
        {
            RuleFor(x => x.HasTwoInTwelveMonths)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Tell us if you've submitted 2 applications to join the RoATP in the last 12 months");
        }
    }
}
