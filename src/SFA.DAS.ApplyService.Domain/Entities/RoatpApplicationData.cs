using System;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class RoatpApplicationData
    {
        public Guid ApplicationId { get; set; }
        public string ReferenceNumber { get; set; }
        public string UKPRN { get; set; }
        public string OrganisationName { get; set; }
        public string TradingName { get; set; }
        public string ApplicationRouteId { get; set; }
        public DateTime? ApplicationSubmittedOn { get; set; }
        public Guid ApplicationSubmittedBy { get; set; }
    }
}
