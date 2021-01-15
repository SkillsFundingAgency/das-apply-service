namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using SFA.DAS.ApplyService.Domain.Ukrlp;
    using System.Threading.Tasks;

    public interface IUkrlpApiClient
    {
        Task<UkrlpLookupResults> GetTrainingProviderByUkprn(int ukprn);
    }
}
