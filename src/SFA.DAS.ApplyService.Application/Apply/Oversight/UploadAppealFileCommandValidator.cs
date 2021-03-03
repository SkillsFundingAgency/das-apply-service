using FluentValidation;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class UploadAppealFileCommandValidator : AbstractValidator<UploadAppealFileCommand>
    {
        public UploadAppealFileCommandValidator()
        {
            RuleFor(x => x.ApplicationId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.File).NotNull();
        }
    }
}
