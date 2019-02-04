using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Models.CompaniesHouse
{
    public class AccountingReferenceDate
    {
        public int day { get; set; }
        public int month { get; set; }
    }

    public class LastAccounts
    {
        public DateTime made_up_to { get; set; }
        public DateTime? period_end_on { get; set; }
        public DateTime? period_start_on { get; set; }
        public string type { get; set; }
    }

    public class NextAccounts
    {
        public DateTime? due_on { get; set; }
        public bool? overdue { get; set; }
        public DateTime? period_end_on { get; set; }
        public DateTime? period_start_on { get; set; }
    }

    public class Accounts
    {
        public AccountingReferenceDate accounting_reference_date { get; set; }
        public LastAccounts last_accounts { get; set; }
        public NextAccounts next_accounts { get; set; }
        public DateTime? next_due { get; set; }
        public DateTime next_made_up_to { get; set; }
        public bool overdue { get; set; }
    }

    public class AnnualReturn
    {
        public DateTime? last_made_up_to { get; set; }
        public DateTime? next_due { get; set; }
        public DateTime? next_made_up_to { get; set; }
        public bool? overdue { get; set; }
    }

    public class BranchCompanyDetails
    {
        public string business_activity { get; set; }
        public string parent_company_name { get; set; }
        public string parent_company_number { get; set; }
    }

    public class ConfirmationStatement
    {
        public DateTime? last_made_up_to { get; set; }
        public DateTime next_due { get; set; }
        public DateTime next_made_up_to { get; set; }
        public bool? overdue { get; set; }
    }

    public class AccountingRequirement
    {
        public string foreign_account_type { get; set; }
        public string terms_of_account_publication { get; set; }
    }

    public class AccountPeriodFrom
    {
        public int? day { get; set; }
        public int? month { get; set; }
    }

    public class AccountPeriodTo
    {
        public int? day { get; set; }
        public int? month { get; set; }
    }

    public class MustFileWithin
    {
        public int? months { get; set; }
    }

    public class ForeignAccounts
    {
        public AccountPeriodFrom account_period_from { get; set; }
        public AccountPeriodTo account_period_to { get; set; }
        public MustFileWithin must_file_within { get; set; }
    }

    public class OriginatingRegistry
    {
        public string country { get; set; }
        public string name { get; set; }
    }

    public class ForeignCompanyDetails
    {
        public AccountingRequirement accounting_requirement { get; set; }
        public ForeignAccounts accounts { get; set; }
        public string business_activity { get; set; }
        public string company_type { get; set; }
        public string governed_by { get; set; }
        public bool? is_a_credit_finance_institution { get; set; }
        public OriginatingRegistry originating_registry { get; set; }
        public string registration_number { get; set; }
    }

    public class CompanyDetailsLinks
    {
        public string charges { get; set; }
        public string filing_history { get; set; }
        public string insolvency { get; set; }
        public string officers { get; set; }
        public string persons_with_significant_control { get; set; }
        public string persons_with_significant_control_statements { get; set; }
        public string registers { get; set; }
        public string self { get; set; }
    }

    public class PreviousCompanyName
    {
        public DateTime ceased_on { get; set; }
        public DateTime effective_from { get; set; }
        public string name { get; set; }
    }

    public class RegisteredOfficeAddress
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

    public class CompanyDetails
    {
        public Accounts accounts { get; set; }
        public AnnualReturn annual_return { get; set; }
        public BranchCompanyDetails branch_company_details { get; set; }
        public bool can_file { get; set; }
        public string company_name { get; set; }
        public string company_number { get; set; }
        public string company_status { get; set; }
        public string company_status_detail { get; set; }
        public ConfirmationStatement confirmation_statement { get; set; }
        public DateTime? date_of_cessation { get; set; }
        public DateTime? date_of_creation { get; set; }
        public string etag { get; set; }
        public string external_registration_number { get; set; }
        public ForeignCompanyDetails foreign_company_details { get; set; }
        public bool? has_been_liquidated { get; set; }
        public bool? has_charges { get; set; } // Deprecated. Please use links.charges 
        public bool? has_insolvency_history { get; set; } // Deprecated. Please use links.insolvency
        public bool? is_community_interest_company { get; set; } // Deprecated. Please use subtype
        public string jurisdiction { get; set; }
        public DateTime last_full_members_list_date { get; set; }
        public CompanyDetailsLinks links { get; set; }
        public string partial_data_available { get; set; }
        public List<PreviousCompanyName> previous_company_names { get; set; }
        public RegisteredOfficeAddress registered_office_address { get; set; }
        public bool registered_office_is_in_dispute { get; set; }
        public List<string> sic_codes { get; set; }
        public string subtype { get; set; }
        public string type { get; set; }
        public bool? undeliverable_registered_office_address { get; set; }
    }
}
