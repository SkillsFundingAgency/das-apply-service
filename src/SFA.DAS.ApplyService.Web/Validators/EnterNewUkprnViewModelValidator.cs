using FluentValidation;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public class EnterNewUkprnViewModelValidator : AbstractValidator<EnterNewUkprnViewModel>
    {
        public EnterNewUkprnViewModelValidator()
        {
            RuleFor(x => x.Ukprn).NotEmpty();
        }
    }
}
