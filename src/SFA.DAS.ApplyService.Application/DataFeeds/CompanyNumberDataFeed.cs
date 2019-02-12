using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Organisations;

namespace SFA.DAS.ApplyService.Application.DataFeeds
{
    public class UkPrnDataFeed : IDataFeed
    {
        private readonly IOrganisationRepository _organisationRepository;

        public UkPrnDataFeed(IOrganisationRepository organisationRepository)
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
            
            return new DataFedAnswerResult() {Answer = organisation?.OrganisationUkprn?.ToString()};
        }
    }
    
    public class CompanyNumberDataFeed : IDataFeed
    {
        private readonly IOrganisationRepository _organisationRepository;

        public CompanyNumberDataFeed(IOrganisationRepository organisationRepository)
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

            return new DataFedAnswerResult() {Answer = organisation?.OrganisationDetails?.CompanyNumber};
        }
    }
}