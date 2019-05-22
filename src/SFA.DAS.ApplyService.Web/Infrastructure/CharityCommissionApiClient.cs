using SFA.DAS.ApplyService.InternalApi.Types.CharityCommission;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class CharityCommissionApiClient : ICharityCommissionApiClient
    {
        public Charity GetCharityDetails(string charityNumber)
        { 
            // implementation of call to API in story APR-449
            return new Charity {CharityNumber = charityNumber};
        }
    }
}
