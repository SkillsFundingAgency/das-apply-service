using FluentValidation;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    // TODO: APPEALREVIEW - Review once appeal work starts
    public class RemoveAppealFileCommandValidator : AbstractValidator<RemoveAppealFileCommand>
    {
        public RemoveAppealFileCommandValidator()
        {
            RuleFor(x => x.ApplicationId).NotEmpty();
            RuleFor(x => x.FileId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.UserName).NotEmpty();
        }
    }
}
