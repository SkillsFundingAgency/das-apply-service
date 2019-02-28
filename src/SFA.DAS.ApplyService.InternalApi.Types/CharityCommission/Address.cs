namespace SFA.DAS.ApplyService.InternalApi.Types.CharityCommission
{
    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; } // address_line_3 ??
        public string County { get; set; } // address_line_4 ??
        public string Country { get; set; } // // address_line_5 ??
        public string PostalCode { get; set; }
    }
}
