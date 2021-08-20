namespace SFA.DAS.ApplyService.Types
{
    public enum AppealStatus
    {
        None = 0,
        Submitted = 1,
        InProgressOutcome = 2,
        Successful = 3,
        SuccessfulAlreadyActive = 4,
        SuccessfulFitnessForFunding = 5,
        Unsuccessful = 6
    }
}
