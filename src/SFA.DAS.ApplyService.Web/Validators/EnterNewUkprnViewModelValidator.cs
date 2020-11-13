using FluentValidation;
using SFA.DAS.ApplyService.Web.Resources;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public class EnterNewUkprnViewModelValidator : AbstractValidator<EnterNewUkprnViewModel>
    {
        public EnterNewUkprnViewModelValidator(IUkprnWhitelistValidator ukprnWhitelistValidator)
        {
            RuleFor(x => x.Ukprn)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(UkprnValidationMessages.MissingUkprn)
                .Must(x => UkprnValidator.IsValidUkprn(x, out _)).WithMessage(UkprnValidationMessages.InvalidUkprn)
                .Must(((model, s) => s.Trim() != model.CurrentUkprn.ToString()))
                    .WithMessage(UkprnValidationMessages.NewUkprnMustNotBeSame)
                .MustAsync(async (s,
                    token) => await ukprnWhitelistValidator.IsWhitelistedUkprn(long.Parse(s)))
                    .WithMessage(UkprnValidationMessages.NotWhitelistedUkprn);
        }
    }
}
