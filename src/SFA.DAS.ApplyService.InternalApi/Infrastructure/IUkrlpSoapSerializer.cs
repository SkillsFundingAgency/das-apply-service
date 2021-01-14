using SFA.DAS.ApplyService.InternalApi.Models.Ukrlp;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public interface IUkrlpSoapSerializer
    {
        string BuildUkrlpSoapRequest(int ukprn, string stakeholderId, string queryId);
        MatchingProviderRecords DeserialiseMatchingProviderRecordsResponse(string soapXml);
    }
}
