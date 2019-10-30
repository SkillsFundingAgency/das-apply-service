using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class AddPartnerIndividualViewModel
    {
        public Guid ApplicationId { get; set; }
        public List<ValidationErrorDetail> ErrorMessages { get; set; }        
        public string PartnerName { get; set; }
        public string PartnerDobMonth { get; set; }
        public string PartnerDobYear { get; set; }
    }
}
