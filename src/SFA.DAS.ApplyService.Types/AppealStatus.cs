namespace SFA.DAS.ApplyService.Types
{
    public enum AppealStatus
    {
        None = 0,
        Submitted = 1,

        InProgress = 2,

        // TODO: Not sure about these ones. Check when we're getting towards end of the appeal flow
        Withdrawn = 3,
        Overturned = 4,
        Upheld = 5
        //Successful = 4,
        //SuccessfulAlreadyActive = 5,
        //SuccessfulFitnessForFunding = 6,
        //Unsuccessful = 7
    }
}
