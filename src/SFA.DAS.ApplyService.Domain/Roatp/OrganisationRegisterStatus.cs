namespace SFA.DAS.ApplyService.Domain.Roatp
{
    using System;

    public class OrganisationRegisterStatus
    {
        public bool UkprnOnRegister { get; set; }
        public Guid? OrganisationId { get; set; }
        public int? ProviderTypeId { get; set; }
        public int? StatusId { get; set; }
        public int? RemovedReasonId { get; set; }
        public DateTime? StatusDate { get; set; }
    }

    public class OrganisationStatus
    {
        public const int Removed = 0;
        public const int Active = 1;
        public const int ActiveNotTakingOnApprentices = 2;
        public const int Onboarding = 3;
    }

    public class RemovedReason
    {
        public const int Breach = 1;
        public const int ChangeOfTradingStatus = 2;
        public const int HighRiskPolicy = 3;
        public const int InadequateFinancialHealth = 4;
        public const int InadequateOfstedGrade = 5;
        public const int InternalError = 6;
        public const int Merger = 7;
        public const int MinimumStandardsNotMet = 8;
        public const int NonDirectDeliveryInTwelveMonthPeriod = 9;
        public const int ProviderError = 10;
        public const int ProviderRequest = 11;
        public const int Other = 12;
    }
}
