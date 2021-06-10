using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class OutcomeSectorDetailsViewModel
    {
        public new SectorDetails SectorDetails { get; set; }


        public int SequenceNumber { get; set; }
        public int SectionNumber { get; set; }
        public string PageId { get; set; }



        // MFCMFC note sure if this is needed

        public Guid ApplicationId { get; set; }
        public string Heading { get; set; }
        public string Caption { get; set; }
        public string ApplyLegalName { get; set; }
        public string Ukprn { get; set; }
        public string ApplicationRoute { get; set; }
        public DateTime? SubmittedDate { get; set; }

        public string ApplicantEmailAddress { get; set; }

        public string ApplicationRouteShortText
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ApplicationRoute))
                {
                    return string.Empty;
                }
                var index = ApplicationRoute.IndexOf(' ');
                if (index < 0)
                {
                    return ApplicationRoute;
                }
                return ApplicationRoute.Substring(0, index + 1);
            }
        }
    }


    public class SectorDetails
    {
        public string SectorName { get; set; }
        public string WhatStandardsOffered { get; set; }
        public string HowManyStarts { get; set; }
        public string HowManyEmployees { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobRole { get; set; }
        public string TimeInRole { get; set; }
        public string IsPartOfAnyOtherOrganisations { get; set; }
        public string OtherOrganisations { get; set; }
        public string DateOfBirth { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string ExperienceOfDelivering { get; set; }
        public string WhereDidTheyGainThisExperience { get; set; }
        public string DoTheyHaveQualifications { get; set; }
        public string WhatQualificationsDoTheyHave { get; set; }
        public string ApprovedByAwardingBodies { get; set; }
        public string NamesOfAwardingBodies { get; set; }
        public string DoTheyHaveTradeBodyMemberships { get; set; }
        public string NamesOfTradeBodyMemberships { get; set; }

        public string WhatTypeOfTrainingDelivered { get; set; }

        public string HowHaveTheyDeliveredTraining { get; set; }
        public string[] HowIsTrainingDelivered { get; set; }

        public string ExperienceOfDeliveringTraining { get; set; }
        public string TypicalDurationOfTraining { get; set; }
    }
}
