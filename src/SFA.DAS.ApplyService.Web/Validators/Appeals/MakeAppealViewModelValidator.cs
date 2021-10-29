using FluentValidation;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals;

namespace SFA.DAS.ApplyService.Web.Validators.Appeals
{
    public class MakeAppealViewModelValidator : AbstractValidator<MakeAppealViewModel>
    {
        public const string NoAppealOptionSelectedError = "Select on what grounds your organisation is making an appeal";

        public MakeAppealViewModelValidator()
        {
            When(x => !x.AppealOnEvidenceSubmitted, () =>
            {
                RuleFor(x => x.AppealOnPolicyOrProcesses).NotEmpty().WithMessage(NoAppealOptionSelectedError);
            });
        }
    }
}
