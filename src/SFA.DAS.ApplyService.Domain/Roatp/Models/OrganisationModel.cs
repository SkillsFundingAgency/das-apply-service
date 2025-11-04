using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.Roatp.Models;

public class OrganisationModel
{
    public Guid OrganisationId { get; set; }
    public int Ukprn { get; set; }
    public string LegalName { get; set; }
    public string TradingName { get; set; }
    public string CompanyNumber { get; set; }
    public string CharityNumber { get; set; }
    public ProviderType ProviderType { get; set; }
    public int OrganisationTypeId { get; set; }
    public string OrganisationType { get; set; }
    public OrganisationStatus Status { get; set; }
    public DateTime? ApplicationDeterminedDate { get; set; }
    public int? RemovedReasonId { get; set; }
    public string RemovedReason { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    public IEnumerable<AllowedCourseType> AllowedCourseTypes { get; set; } = [];
}

public record AllowedCourseType(int CourseTypeId, string CourseTypeName, LearningType LearningType);

public enum LearningType
{
    Standard = 1,
    ShortCourse = 2
}

public enum ProviderType
{
    Main = 1,
    Employer = 2,
    Supporting = 3
}

public enum OrganisationStatus
{
    Removed = 0,
    Active = 1,
    ActiveNoStarts = 2,
    OnBoarding = 3
}

