using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using FluentValidation;

namespace SFA.DAS.ApplyService.Web.Validators
{
    public class RemoveManagementHierarchyValidator : AbstractValidator<ConfirmRemoveManagementHierarchyViewModel>
    {
        public const string ConfirmationMissingError = "Tell us if you want to remove";

        public RemoveManagementHierarchyValidator()
        {
            RuleFor(x => x.Confirmation)
                .NotEmpty().WithMessage(x => $"{ConfirmationMissingError} {x?.Name}");
        }
    }
}
