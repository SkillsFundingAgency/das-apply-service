namespace SFA.DAS.ApplyService.Domain.CharityCommission
{
    using System;
    using System.Collections.Generic;

    public class CharityCommissionSummary
    {
        public string CharityNumber { get; set; }
        public DateTime? IncorporatedOn { get; set; }
        public List<Trustee> Trustees { get; set; }
    }

    public class Trustee
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
