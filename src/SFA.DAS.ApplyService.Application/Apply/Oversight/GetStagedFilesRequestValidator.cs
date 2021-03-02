using FluentValidation;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class GetStagedFilesRequestValidator : AbstractValidator<GetStagedFilesRequest>
    {
        public GetStagedFilesRequestValidator()
        {
            RuleFor(x => x.ApplicationId).NotEmpty();
        }
    }
}
