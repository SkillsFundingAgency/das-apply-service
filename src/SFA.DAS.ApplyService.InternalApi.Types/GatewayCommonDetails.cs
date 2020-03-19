using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApplyService.InternalApi.Types
{
    public class GatewayCommonDetails
    {
        public string Ukprn { get; set; }
        public DateTime? ApplicationSubmittedOn { get; set; }
        public DateTime? CheckedOn { get; set; }
        public string LegalName { get; set; }
        public string Status { get; set; }

        public string GatewayReviewStatus { get; set; }

        public string OptionPassText { get; set; }
        public string OptionFailText { get; set; }

        public string OptionInProgressText { get; set; }

    }
}
