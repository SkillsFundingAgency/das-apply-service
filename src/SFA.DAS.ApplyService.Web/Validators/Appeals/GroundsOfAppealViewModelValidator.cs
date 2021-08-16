using FluentValidation;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals;

namespace SFA.DAS.ApplyService.Web.Validators.Appeals
{
    public class GroundsOfAppealViewModelValidator : AbstractValidator<GroundsOfAppealViewModel>
    {
        public const string NoHowFailedOnPolicyOrProcessesEntered = "Tell us how the ESFA failed to follow its own policy or processes";
        public const string NoHowFailedOnEvidenceSubmittedEntered = "Tell us how the ESFA failed to understand or recognise the evidence submitted";

        public const int MaxLength = 10000;
        public const string MaxLengthError = "Your answer must be 10,000 characters or less";

        public GroundsOfAppealViewModelValidator()
        {
            When(x => x.AppealOnPolicyOrProcesses, () =>
            {
                RuleFor(x => x.HowFailedOnPolicyOrProcesses)
                    .NotEmpty().WithMessage(NoHowFailedOnPolicyOrProcessesEntered)
                    .MaximumLength(MaxLength).WithMessage(MaxLengthError);
            });

            When(x => x.AppealOnEvidenceSubmitted, () =>
            {
                RuleFor(x => x.HowFailedOnEvidenceSubmitted)
                    .NotEmpty().WithMessage(NoHowFailedOnEvidenceSubmittedEntered)
                    .MaximumLength(MaxLength).WithMessage(MaxLengthError);
            });
        }
    }
}
