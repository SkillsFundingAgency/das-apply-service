namespace SFA.DAS.ApplyService.Types
{
    public enum OversightReviewStatus
    {
        None = 0,
        Successful = 1,
        SuccessfulAlreadyActive = 2,
        SuccessfulFitnessForFunding = 3,
        Unsuccessful = 4,
        InProgress = 5,
        Rejected = 6,
        Withdrawn = 7,
        Removed = 8
    }
}
