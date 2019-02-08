using System;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class FinancialApplicationGrade
    {
        public string SelectedGrade { get; set; }
        public string InadequateMoreInformation { get; set; }
        public string GradedBy { get; set; }
        public DateTime GradedDateTime { get; set; }
        public DateTime? FinancialDueDate { get; set; }
    }

    public class FinancialApplicationSelectedGrade
    {
        public const string Excellent = "Excellent";
        public const string Good = "Good";
        public const string Satisfactory = "Satisfactory";
        public const string Inadequate = "Inadequate";
        public const string Exempt = "Exempt";
    }
}