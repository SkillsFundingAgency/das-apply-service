using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Models.CompaniesHouse
{
    public class Classification
    {
        public string description { get; set; }
        public string type { get; set; }
    }

    public class InsolvencyCaseLinks
    {
        public string @case { get; set; }
    }

    public class InsolvencyCase
    {
        public int? case_number { get; set; }
        public InsolvencyCaseLinks links { get; set; }
        public int transaction_id { get; set; }
    }

    public class ChargeLinks
    {
        public string self { get; set; }
    }

    public class Particulars
    {
        public bool? chargor_acting_as_bare_trustee { get; set; }
        public bool? contains_fixed_charge { get; set; }
        public bool? contains_floating_charge { get; set; }
        public bool? contains_negative_pledge { get; set; }
        public string description { get; set; }
        public bool? floating_charge_covers_all { get; set; }
        public string type { get; set; }
    }

    public class PersonsEntitled
    {
        public string name { get; set; }
    }

    public class ScottishAlterations
    {
        public bool? has_alterations_to_order { get; set; }
        public bool? has_alterations_to_prohibitions { get; set; }
        public bool? has_restricting_provisions { get; set; }
    }

    public class SecuredDetails
    {
        public string description { get; set; }
        public string type { get; set; }
    }

    public class TransactionLinks
    {
        public string filing { get; set; }
        public string insolvency_case { get; set; }
    }

    public class Transaction
    {
        public DateTime? delivered_on { get; set; }
        public string filing_type { get; set; }
        public int? insolvency_case_number { get; set; }
        public TransactionLinks links { get; set; }
        public int? transaction_id { get; set; }
    }

    public class Charge
    {
        public DateTime? acquired_on { get; set; }
        public string assets_ceased_released { get; set; }
        public string charge_code { get; set; } // Only applied to charges submitted post April 2013 
        public int? charge_number { get; set; } // Only applied to charges submitted pre April 2013 
        public Classification classification { get; set; }
        public DateTime? covering_instrument_date { get; set; }
        public DateTime? created_on { get; set; }
        public DateTime? delivered_on { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public List<InsolvencyCase> insolvency_cases { get; set; }
        public ChargeLinks links { get; set; }
        public bool? more_than_four_persons_entitled { get; set; } // Only applied to charges submitted post April 2013 
        public Particulars particulars { get; set; }
        public List<PersonsEntitled> persons_entitled { get; set; }
        public DateTime? resolved_on { get; set; }
        public DateTime? satisfied_on { get; set; }
        public ScottishAlterations scottish_alterations { get; set; }
        public SecuredDetails secured_details { get; set; }
        public string status { get; set; }
        public List<Transaction> transactions { get; set; }
    }

    public class ChargeList
    {
        public string etag { get; set; }
        public List<Charge> items { get; set; }
        public int? part_satisfied_count { get; set; }
        public int? satisfied_count { get; set; }
        public int? total_count { get; set; }
        public int? unfiletered_count { get; set; }
    }
}
