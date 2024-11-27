﻿namespace SFA.DAS.ApplyService.Domain.Roatp
{
    public class OrganisationVerificationStatus
    {
        public bool VerifiedCompaniesHouse { get; set; }
        public bool CompaniesHouseManualEntry { get; set; }
        public bool VerifiedCharityCommission { get; set; }
        public bool CharityCommissionManualEntry { get; set; }
        public bool CompaniesHouseDataConfirmed { get; set; }
        public bool CharityCommissionDataConfirmed { get; set; }
        public bool CharityCommissionDataExempted { get; set; }
        public bool WhosInControlConfirmed { get; set; }
        public bool WhosInControlStarted { get; set; }
    }
}
