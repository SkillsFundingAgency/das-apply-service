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
                var dates = RegistrationHistory?.Where(d => d.RegistrationDateTime.HasValue).Select(d => d.RegistrationDateTime);
                return dates?.Min();
            }
        }

        [XmlIgnore]
        public DateTime? RegistrationRemovalDate
        {
            get
            {
                var dates = RegistrationHistory?.Where(d => d.RegistrationRemovalDateTime.HasValue).Select(d => d.RegistrationRemovalDateTime);
                return dates?.Min();
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
