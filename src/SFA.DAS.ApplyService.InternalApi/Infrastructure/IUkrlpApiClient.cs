namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.Ukrlp;

    public interface IUkrlpApiClient
    {
        Task<List<ProviderDetails>> GetTrainingProviderByUkprn(long ukprn);
    }
}
