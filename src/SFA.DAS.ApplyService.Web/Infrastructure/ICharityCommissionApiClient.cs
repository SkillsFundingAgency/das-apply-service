namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using InternalApi.Types.CharityCommission;
    using System.Threading.Tasks;

    public interface ICharityCommissionApiClient
    {
        Task<Charity> GetCharityDetails(int charityNumber);
    }
}
