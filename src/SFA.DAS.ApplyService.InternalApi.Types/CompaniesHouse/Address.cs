namespace SFA.DAS.ApplyService.InternalApi.Types.CompaniesHouse
{
    public class Address
    {
        public string AddressLine1 { get; set; } // po_box + premises + address_line_1
        public string AddressLine2 { get; set; }
        public string City { get; set; } // locality
        public string County { get; set; } // region
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}
