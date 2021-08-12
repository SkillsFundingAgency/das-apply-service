using FluentValidation;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Commands.CreateAppeal
{
    // TODO: APPEALREVIEW - Review once appeal work starts
    public class CreateAppealCommandValidator : AbstractValidator<CreateAppealCommand>
    {
        public CreateAppealCommandValidator()
        {
            RuleFor(x => x.OversightReviewId).NotEmpty();
            RuleFor(x => x.Message).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.UserName).NotEmpty();
        }
    }
}