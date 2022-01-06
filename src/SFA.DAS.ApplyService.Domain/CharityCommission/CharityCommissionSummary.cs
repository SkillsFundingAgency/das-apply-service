namespace SFA.DAS.ApplyService.Domain.CharityCommission
{
    using System;
    using System.Collections.Generic;

    public class CharityCommissionSummary
    {
        public string CharityNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? RemovalDate { get; set; }
        public List<Trustee> Trustees { get; set; }
        public bool TrusteeManualEntryRequired { get; set; }
    }

    public class Trustee
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
