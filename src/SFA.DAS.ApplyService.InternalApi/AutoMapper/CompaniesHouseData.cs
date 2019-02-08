using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.AutoMapper.CompaniesHouse
{
    public class Company
    {
        public string CompanyNumber { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public Address RegisteredOfficeAddress { get; set; }
        public IEnumerable<string> NatureOfBusiness { get; set; } // sic codes
        public DateTime? IncorporatedOn { get; set; } // date_of_creation
        public DateTime? DissolvedOn { get; set; } // date_of_cessation
        public IEnumerable<string> PreviousNames { get; set; }

        public Accounts Accounts { get; set; }
        public IEnumerable<Officer> Officers { get; set; }
        public IEnumerable<PersonWithSignificantControl> PeopleWithSignificantControl { get; set; }
    }

    public class Address
    {
        public string AddressLine1 { get; set; } // po_box + premises + address_line_1
        public string AddressLine2 { get; set; }
        public string City { get; set; } // locality
        public string County { get; set; } // region
        public string Country { get; set; }
        public string PostalCode { get; set; }      
    }

    public class Accounts
    {
        public DateTime? LastAccountsDate { get; set; }
        public DateTime? LastConfirmationStatementDate { get; set; }
    }

    public class Officer
    {
        public string Id { get; set; } // TODO: Rather this isn't here but let's see how easy it is to do that
        public string Name { get; set; }
        public string Role { get; set; }
        public Address Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime AppointedOn { get; set; }
        public DateTime? ResignedOn { get; set; }

        public IEnumerable<OfficerDisqualification> Disqualifications { get; set; }
    }

    public class OfficerDisqualification
    {
        public DateTime DisqualifiedFrom { get; set; }
        public DateTime DisqualifiedUntil { get; set; }
        public string CaseIdentifier { get; set; }
        public string Reason { get; set; }
        public string ReasonDescription { get; set; }
    }

    public class PersonWithSignificantControl
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public IEnumerable<string> NaturesOfControl { get; set; }
        public DateTime NotifiedOn { get; set; }
        public DateTime? CeasedOn { get; set; }
    }
}
