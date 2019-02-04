using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Models.CompaniesHouse
{
    public class Date
    {
        public DateTime date { get; set; }
        public string type { get; set; }
    }

    public class CaseLinks
    {
        public string charge { get; set; }
    }

    public class PractitionerAddress
    {
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string country { get; set; }
        public string locality { get; set; }
        public string postal_code { get; set; }
        public string region { get; set; }
    }
    public class Practitioner
    {
        public PractitionerAddress address { get; set; }
        public DateTime? appointed_on { get; set; }
        public DateTime? ceased_to_act_on { get; set; }
        public string name { get; set; }
        public string role { get; set; }
    }

    public class Case
    {
        public List<Date> dates { get; set; }
        public CaseLinks links { get; set; }
        public List<string> notes { get; set; }
        public int? number { get; set; }
        public List<Practitioner> practitioners { get; set; }
        public string type { get; set; }
    }

    public class InsolvencyDetails
    {
        public List<Case> cases { get; set; }
        public string etag { get; set; }
        public List<string> status { get; set; }
    }
}
