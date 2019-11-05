using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class SoleTradeDobViewModel
    {
        public const string DobFieldPrefix = "SoleTraderDob";

        public Guid ApplicationId { get; set; }
        public string SoleTraderName { get; set; }
        public string SoleTraderDobMonth { get; set; }
        public string SoleTraderDobYear { get; set; }
        public List<ValidationErrorDetail> ErrorMessages { get; set; }
    }
}
