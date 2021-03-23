namespace SFA.DAS.ApplyService.Types
{
    public enum AppealStatus
    {
        None = 0,
        Successful = 1,
        SuccessfulAlreadyActive = 2,
        Unsuccessful = 3,
        UnsuccessfulPartiallyUpheld = 4
    }
}
