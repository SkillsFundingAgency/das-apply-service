using FluentValidation;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetOversightReview
{
    public class GetOversightReviewQueryValidator : AbstractValidator<GetOversightReviewQuery>
    {
        public GetOversightReviewQueryValidator()
        {
            RuleFor(x => x.ApplicationId).NotEmpty();
        }
    }
}