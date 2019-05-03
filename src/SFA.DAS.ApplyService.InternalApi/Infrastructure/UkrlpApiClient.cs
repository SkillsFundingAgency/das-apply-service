using AutoMapper;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Configuration;
    using Microsoft.Extensions.Logging;
    using UKRLP;
    using SFA.DAS.ApplyService.InternalApi.Models.Ukrlp;

    public class UkrlpApiClient : IUkrlpApiClient
    {
        private readonly ProviderQueryPortType _client;

        private readonly ILogger<UkrlpApiClient> _logger;

        private IApplyConfig _applyConfig;

        public UkrlpApiClient(ProviderQueryPortType client, ILogger<UkrlpApiClient> logger, IApplyConfig applyConfig)
        {
            _client = client;
            _logger = logger;
            _applyConfig = applyConfig;
        }

        public async Task<List<ProviderDetails>> GetTrainingProviderByUkprn(long ukprn)
        {
            var searchCriteria = new SelectionCriteriaStructure
            {
                ApprovedProvidersOnly = YesNoType.No,
                ApprovedProvidersOnlySpecified = true,
                UnitedKingdomProviderReferenceNumberList = new[] { ukprn.ToString() },
                StakeholderId = _applyConfig.UkrlpApiAuthentication.StakeholderId,
                ProviderStatus = "A",
                CriteriaCondition = QueryCriteriaConditionType.OR,
                CriteriaConditionSpecified = true
            };

            var query = new ProviderQueryStructure
            {
                QueryId = _applyConfig.UkrlpApiAuthentication.QueryId,
                SelectionCriteria = searchCriteria
            };

            var request = new ProviderQueryParam(query);

            var searchResult = await _client.retrieveAllProvidersAsync(request);

            var providerDetails = new List<ProviderDetails>();

            if (searchResult.ProviderQueryResponse.MatchingProviderRecords != null)
            {
                providerDetails =
                    Mapper.Map<List<ProviderDetails>>(searchResult.ProviderQueryResponse.MatchingProviderRecords);
            }

            return await Task.FromResult(providerDetails);
        }
    }
}
