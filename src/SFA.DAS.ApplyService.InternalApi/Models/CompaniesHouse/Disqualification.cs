using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Models.CompaniesHouse
{
    public class DisqualificationAddress
    {
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string country { get; set; }
        public string locality { get; set; }
        public string postal_code { get; set; }
        public string premises { get; set; }
        public string region { get; set; }
    }

    public class LastVariation
    {
        public string case_identifier { get; set; }
        public string court_name { get; set; }
        public DateTime? varied_on { get; set; }
    }

    public class Reason
    {
        public string act { get; set; }
        public string article { get; set; }
        public string description_identifier { get; set; }
        public string section { get; set; }
    }

    public class Disqualification
    {
        public DisqualificationAddress address { get; set; }
        public string case_identifier { get; set; }
        public List<string> company_names { get; set; }
        public string court_name { get; set; }
        public string disqualification_type { get; set; }
        public DateTime disqualified_from { get; set; }
        public DateTime disqualified_until { get; set; }
        public DateTime? heard_on { get; set; }
        public LastVariation last_variation { get; set; }
        public Reason reason { get; set; }
        public DateTime? undertaken_on { get; set; }
    }

    public class DisqualificationListLinks
    {
        public string self { get; set; }
    }

    public class PermissionsToAct
    {
        public List<string> company_names { get; set; }
        public string court_name { get; set; }
        public DateTime expires_on { get; set; }
        public DateTime granted_on { get; set; }
    }

    public class DisqualificationList
    {
        public List<Disqualification> disqualifications { get; set; }
        public string etag { get; set; }
        public string kind { get; set; }
        public DisqualificationListLinks links { get; set; }
        public List<PermissionsToAct> permissions_to_act { get; set; }

        public string company_number { get; set; } // Corporate Only
        public string country_of_registration { get; set; } // Corporate Only
        public string name { get; set; } // Corporate Only
        
        public string title { get; set; } // Natural Only
        public string forename { get; set; } // Natural Only
        public string other_forenames { get; set; } // Natural Only
        public string surname { get; set; } // Natural Only
        public DateTime? date_of_birth { get; set; } // Natural Only
        public string nationality { get; set; } // Natural Only
        public string honours { get; set; } // Natural Only
    }
}
