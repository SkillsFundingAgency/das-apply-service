﻿using System;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals
{
    public class GroundsOfAppealViewModel
    {
        public Guid ApplicationId { get; set; }

        public bool AppealOnPolicyOrProcesses { get; set; }
        public bool AppealOnEvidenceSubmitted { get; set; }
    }
}
