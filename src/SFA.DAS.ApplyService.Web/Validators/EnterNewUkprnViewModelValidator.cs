using FluentValidation;
using SFA.DAS.ApplyService.Web.Resources;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public class EnterNewUkprnViewModelValidator : AbstractValidator<EnterNewUkprnViewModel>
    {
        public EnterNewUkprnViewModelValidator(IAllowedUkprnValidator ukprnWhitelistValidator)
        {
            RuleFor(x => x.Ukprn)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(UkprnValidationMessages.MissingUkprn)
                .Must(x => UkprnValidator.IsValidUkprn(x, out _))
                    .WithMessage(UkprnValidationMessages.InvalidUkprn)
                .MustAsync(async (ukprn, token) => await ukprnWhitelistValidator.CanUkprnStartApplication(int.Parse(ukprn)))
                    .WithMessage(UkprnValidationMessages.NotWhitelistedUkprn);
        }
    }
}
