namespace SFA.DAS.ApplyService.Types
{
    // TODO: APPEALREVIEW - Review once appeal work starts
    public enum AppealStatus
    {
        None = 0,
        Successful = 1,
        SuccessfulAlreadyActive = 2,
        Unsuccessful = 3,
        UnsuccessfulPartiallyUpheld = 4
    }
}
