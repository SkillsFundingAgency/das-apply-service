using FluentValidation;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetStagedFiles
{
    public class GetStagedFilesRequestValidator : AbstractValidator<GetStagedFilesRequest>
    {
        public GetStagedFilesRequestValidator()
        {
            RuleFor(x => x.ApplicationId).NotEmpty();
        }
    }
}
