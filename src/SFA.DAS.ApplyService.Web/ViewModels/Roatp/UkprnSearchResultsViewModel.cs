namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    using System.Linq;
    using Domain.CharityCommission;
    using Domain.CompaniesHouse;
    using Domain.Ukrlp;
    using InternalApi.Types.CharityCommission;

    public class UkprnSearchResultsViewModel
    {
        private ProviderContact _legalContactDetails;
        private ProviderDetails _providerDetails;

        public ProviderDetails ProviderDetails
        {
            get { return _providerDetails; }
            set
            {
                _providerDetails = value;
                if (_providerDetails?.ContactDetails == null)
                {
                    return;
                }

                var legalContact = _providerDetails.ContactDetails.FirstOrDefault(x => x.ContactType == "L");
                if (legalContact != null)
                {
                    _legalContactDetails = legalContact;
                }
            }
        }

        public ProviderContact LegalContactDetails
        {
            get { return _legalContactDetails; }
        }
    
        public string UKPRN { get; set; }
        public int ApplicationRouteId { get; set; }

        public string CompaniesHouseNumber
        {
            get
            {
                return ProviderDetails.VerificationDetails.FirstOrDefault(x => x.VerificationAuthority == "Companies House")?.VerificationId;
            }
        }

        public CompaniesHouseSummary CompaniesHouseInformation { get; set; }

        public string CharityNumber
        {
            get
            {
                return ProviderDetails.VerificationDetails.FirstOrDefault(x => x.VerificationAuthority == "Charity Commission")?.VerificationId;
            }
        }

        public CharityCommissionSummary CharityCommissionInformation { get; set; }
    }
}
