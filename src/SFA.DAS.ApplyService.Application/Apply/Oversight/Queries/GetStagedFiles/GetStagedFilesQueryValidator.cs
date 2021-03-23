using FluentValidation;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetStagedFiles
{
    public class GetStagedFilesQueryValidator : AbstractValidator<GetStagedFilesQuery>
    {
        public GetStagedFilesQueryValidator()
        {
            RuleFor(x => x.ApplicationId).NotEmpty();
        }
    }
}
