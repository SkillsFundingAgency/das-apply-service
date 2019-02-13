using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Organisations;

namespace SFA.DAS.ApplyService.Application.DataFeeds
{
    public class CharityNumberDataFeed : IDataFeed
    {
        private readonly IOrganisationRepository _organisationRepository;

        public CharityNumberDataFeed(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }
        
        public async Task<DataFedAnswerResult> GetAnswer(Guid applicationId)
        {
            var organisation = await _organisationRepository.GetOrganisationByApplicationId(applicationId);
            if (organisation == null)
            {
                throw new ArgumentException("Could not find Organisation for supplied applicationId");
            }

            return new DataFedAnswerResult() {Answer = organisation?.OrganisationDetails?.CharityNumber};
        }
    }
}