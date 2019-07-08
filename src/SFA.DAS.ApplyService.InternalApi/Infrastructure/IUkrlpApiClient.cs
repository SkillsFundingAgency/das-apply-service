namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    using System.Threading.Tasks;
    using Models.Ukrlp;

    public interface IUkrlpApiClient
    {
        Task<UkprnLookupResponse> GetTrainingProviderByUkprn(long ukprn);
    }
}
