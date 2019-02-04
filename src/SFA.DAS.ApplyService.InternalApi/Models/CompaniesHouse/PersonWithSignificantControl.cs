using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Models.CompaniesHouse
{
    public class PersonWithSignificantControlAddress
    {
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string care_of { get; set; }
        public string country { get; set; }
        public string locality { get; set; }
        public string po_box { get; set; }
        public string postal_code { get; set; }
        public string premises { get; set; }
        public string region { get; set; }
    }

    public class PersonWithSignificantControlLinks
    {
        public string self { get; set; }
        public string statement { get; set; }
    }

    public class NameElements
    {
        public string forename { get; set; }
        public string other_forenames { get; set; }
        public string surname { get; set; }
        public string title { get; set; }
    }

    public class PersonWithSignificantControl
    {
        public PersonWithSignificantControlAddress address { get; set; }
        public DateTime? ceased_on { get; set; }
        public string country_of_residence { get; set; }
        public DateOfBirth date_of_birth { get; set; }
        public string etag { get; set; }
        public PersonWithSignificantControlLinks links { get; set; }
        public string name { get; set; }
        public NameElements name_elements { get; set; }
        public string nationality { get; set; }
        public List<string> natures_of_control { get; set; }
        public DateTime notified_on { get; set; }
    }

    public class PersonWithSignificantControlListLinks
    {
        public string persons_with_significant_control_statements_list { get; set; }
        public string self { get; set; }
    }

    public class PersonWithSignificantControlList
    {
        public int active_count { get; set; }
        public int ceased_count { get; set; }
        public string etag { get; set; }
        public List<PersonWithSignificantControl> items { get; set; }
        public string items_per_page { get; set; }
        public string kind { get; set; }
        public PersonWithSignificantControlListLinks links { get; set; }
        public string start_index { get; set; }
        public string total_results { get; set; }
    }
}
