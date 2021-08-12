using FluentValidation;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetAppeal
{
    // TODO: APPEALREVIEW - Review once appeal work starts
    public class GetAppealQueryValidator : AbstractValidator<GetAppealQuery>
    {
        public GetAppealQueryValidator()
        {
            RuleFor(x => x.ApplicationId).NotEmpty();
            RuleFor(x => x.OversightReviewId).NotEmpty();
        }
    }
}