namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using SFA.DAS.ApplyService.Domain.Ukrlp;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUkrlpApiClient
    {
        Task<List<ProviderDetails>> GetTrainingProviderByUkprn(long ukprn);
    }
}
