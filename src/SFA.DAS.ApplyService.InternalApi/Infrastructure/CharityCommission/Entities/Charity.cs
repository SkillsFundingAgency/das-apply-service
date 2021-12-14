using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure.CharityCommission.Entities
{
    public class Charity
    {
        [JsonProperty("organisation_number")]
        public long OrganisationNumber { get; set; }

        [JsonProperty("reg_charity_number")]
        public int RegisteredCharityNumber { get; set; }

        [JsonProperty("charity_co_reg_number")]
        public string RegisteredCompanyNumber { get; set; }

        [JsonProperty("charity_name")]
        public string CharityName { get; set; }

        [JsonProperty("charity_type")]
        public string Type { get; set; }

        [JsonProperty("insolvent")]
        public bool Insolvent { get; set; }

        [JsonProperty("in_administration")]
        public bool InAdministration { get; set; }

        [JsonProperty("cio_dissolution_ind")]
        public bool CioDissolved { get; set; }

        [JsonProperty("reg_status")]
        public string RegistrationStatus { get; set; }

        [JsonIgnore]
        public string Status
        {
            get
            {
                // Note: This may be overkill and we can just see if it's 'R' or 'RM'
                return "RM".Equals(RegistrationStatus) ? "removed" :
                        CioDissolved ? "dissolved" :
                            Insolvent ? "insolvent" :
                                InAdministration ? "suspended" :
                                    "R".Equals(RegistrationStatus) ? "registered" : "unknown";
            }
        }

        [JsonProperty("date_of_registration")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? RegistrationDate { get; set; }

        [JsonProperty("date_of_removal")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? RegistrationRemovalDate { get; set; }

        [JsonProperty("removal_reason")]
        public string RemovalReason { get; set; }

        ////// 
        // See if we can put this into accounts object
        [JsonProperty("latest_acc_fin_year_start_date")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? LatestAccountsStartDate { get; set; }

        [JsonProperty("latest_acc_fin_year_end_date")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime? LatestAccountsEndDate { get; set; }

        [JsonProperty("latest_income")]
        public long? LatestIncome { get; set; }

        [JsonProperty("latest_expenditure")]
        public long? LatestExpenditure { get; set; }
        //end
        ///////

        ////// 
        // See if we can put this into address object
        [JsonProperty("address_line_one")]
        public string AddressLine1 { get; set; }

        [JsonProperty("address_line_two")]
        public string AddressLine2 { get; set; }

        [JsonProperty("address_line_three")]
        public string AddressLine3 { get; set; }

        [JsonProperty("address_line_four")]
        public string AddressLine4 { get; set; }

        [JsonProperty("address_line_five")]
        public string AddressLine5 { get; set; }

        [JsonProperty("address_post_code")]
        public string AddressPostcode { get; set; }
        //end
        ///////

        [JsonProperty("phone")]
        public string TelephoneNumber { get; set; }

        [JsonProperty("email")]
        public string EmailAddress { get; set; }

        [JsonProperty("web")]
        public string WebsiteAddress { get; set; }

        [JsonProperty("trustee_names")]
        public Trustee[] Trustees { get; set; }

        [JsonProperty("who_what_where")]
        public Classification[] Classifications { get; set; }

        [JsonIgnore]
        public string[] NatureOfBusiness
        {
            get
            {
                return Classifications?.Where(c => c.Type == ClassificationType.What).Select(c => c.Description).ToArray();
            }
        }

        [JsonProperty("last_modified_time")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime LastModifiedTime { get; set; }
    }
}
