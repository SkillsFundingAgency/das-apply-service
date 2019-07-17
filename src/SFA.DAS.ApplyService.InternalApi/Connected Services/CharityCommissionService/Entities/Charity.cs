using System;
using System.Linq;
using System.Xml.Serialization;

namespace CharityCommissionService
{
   public partial class Charity
    {
        [XmlIgnore]
        public string Status
        {
            get
            {
                // Note: This may be overkill and we can just see if it's 'R' or 'RM'
                return "RM".Equals(OrganisationType) ? "removed" :
                    (IsCIODissolutionReceived || IsCIOAdminDissolution) ? "dissolved" :
                        IsInsolvencyActDocsReceived ? "insolvent" :
                            IsSuspended ? "suspended" :
                                "R".Equals(OrganisationType) ? "registered" : "unknown";
            }
        }

        [XmlIgnore]
        public string Type
        {
            get
            {
                return LastRegistrationEvent?.CharityEventTypeNameEnglish;
            }
        }

        [XmlIgnore]
        public DateTime? RegistrationDate
        {
            get
            {
                if (RegistrationHistory.Length > 0)
                {
                    var mostRecentRegistration = RegistrationHistory[RegistrationHistory.Length - 1];
                    return mostRecentRegistration?.RegistrationDateTime;
                }

                return null;
            }
        }

        [XmlIgnore]
        public DateTime? RegistrationRemovalDate
        {
            get
            {
                if (RegistrationHistory.Length > 0)
                {
                    var mostRecentRegistration = RegistrationHistory[RegistrationHistory.Length - 1];
                    return mostRecentRegistration?.RegistrationRemovalDateTime;
                }

                return null;
            }
        }

        [XmlIgnore]
        public string[] NatureOfBusiness
        {
            get
            {
                return Classification?.What;
            }
        }
    }
}
