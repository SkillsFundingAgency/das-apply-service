using SFA.DAS.ApplyService.InternalApi.Models.Ukrlp;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public interface IUkrlpSoapSerializer
    {
        string BuildUkrlpSoapRequest(long ukprn, string stakeholderId, string queryId);
        MatchingProviderRecords DeserialiseMatchingProviderRecordsResponse(string soapXml);
    }
}
