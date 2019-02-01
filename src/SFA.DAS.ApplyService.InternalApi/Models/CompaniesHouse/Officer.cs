using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Models.CompaniesHouse
{
    public class Address
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

    public class DateOfBirth
    {
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
    }

    public class FormerName
    {
        public string forenames { get; set; }
        public string surname { get; set; }
    }

    public class Identification
    {
        public string identification_type { get; set; }
        public string legal_authority { get; set; }
        public string legal_form { get; set; }
        public string place_registered { get; set; }
        public string registration_number { get; set; }
    }

    public class OfficerAppointments
    {
        public string appointments { get; set; }
    }

    public class OfficerLinks
    {
        public OfficerAppointments officer { get; set; }
    }

    public class Officer
    {
        public Address address { get; set; }
        public DateTime appointed_on { get; set; }
        public string country_of_residence { get; set; }
        public DateOfBirth date_of_birth { get; set; }
        public List<FormerName> former_names { get; set; }
        public Identification identification { get; set; }
        public OfficerLinks links { get; set; }
        public string name { get; set; }
        public string nationality { get; set; }
        public string occupation { get; set; }
        public string officer_role { get; set; }
        public DateTime resigned_on { get; set; }
    }


    public class OfficerList
    {
        public int active_count { get; set; }
        public string etag { get; set; }
        public int inactive_count { get; set; }
        public List<Officer> items { get; set; }
        public int items_per_page { get; set; }
        public string kind { get; set; }
        public int resigned_count { get; set; }
        public int start_index { get; set; }
        public int total_results { get; set; }
    }
}
