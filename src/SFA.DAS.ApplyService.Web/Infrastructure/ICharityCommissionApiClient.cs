namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using InternalApi.Types.CharityCommission;

    public interface ICharityCommissionApiClient
    {
        Charity GetCharityDetails(string charityNumber);
    }
}
