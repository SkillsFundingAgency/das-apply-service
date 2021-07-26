using FluentValidation;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetAppealUpload
{
    public class GetAppealUploadQueryValidator : AbstractValidator<GetAppealUploadQuery>
    {
        public GetAppealUploadQueryValidator()
        {
            RuleFor(x => x.ApplicationId).NotEmpty();
            RuleFor(x => x.AppealId).NotEmpty();
            RuleFor(x => x.AppealUploadId).NotEmpty();
        }
    }
}