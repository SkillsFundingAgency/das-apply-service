using SFA.DAS.ApplyService.Application.Apply.Roatp;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public class AssessorLookupService : IAssessorLookupService
    {
        public string GetTitleForSequence(int sequenceId)
        {
            switch (sequenceId)
            {
                case RoatpWorkflowSequenceIds.ProtectingYourApprentices:
                    return "Protecting your apprentices checks";
                case RoatpWorkflowSequenceIds.ReadinessToEngage:
                    return "Readiness to engage checks";
                case RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining:
                    return "Planning apprenticeship training checks";
                case RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining:
                    return "Delivering apprenticeship training checks";
                case RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining:
                    return "Evaluating apprenticeship training checks";
                default:
                    return null;
            }
        }

        public string GetTitleForPage(string pageId)
        {
            switch (pageId)
            {
                case RoatpWorkflowPageIds.ProtectingYourApprentices.ContinuityPlan:
                    return "Continuity plan for apprenticeship training";
                case RoatpWorkflowPageIds.ProtectingYourApprentices.EqualityAndDiversityPolicy:
                    return "Equality and diversity policy";
                case RoatpWorkflowPageIds.ProtectingYourApprentices.SafeguardingPolicy:
                    return "Safeguarding policy";
                case RoatpWorkflowPageIds.ProtectingYourApprentices.SafeguardingOverallResponsibility:
                    return "Overall responsibility for safeguarding";
                case RoatpWorkflowPageIds.ProtectingYourApprentices.SafeguardingPolicyIncludesPreventDutyPolicy:
                    return "Safeguarding policy include Prevent duty policy";
                case RoatpWorkflowPageIds.ProtectingYourApprentices.PreventDutyPolicy:
                    return "Prevent duty policy";
                case RoatpWorkflowPageIds.ProtectingYourApprentices.HealthAndSafetyPolicy:
                    return "Health and safety policy";
                case RoatpWorkflowPageIds.ProtectingYourApprentices.HealthAndSafetyOverallResponsibility:
                    return "Overall responsibility for health and safety";
                case RoatpWorkflowPageIds.ProtectingYourApprentices.ActingAsASubcontractor:
                    return "Acting as a subcontractor";

                case RoatpWorkflowPageIds.ReadinessToEngage.EngagedWithEmployers:
                    return "Engaging with employers to deliver apprenticeship training to employees";
                case RoatpWorkflowPageIds.ReadinessToEngage.RelationshipWithEmployers:
                    return "Managing relationship with employers";
                case RoatpWorkflowPageIds.ReadinessToEngage.RelationshipWithEmployersResponsible:
                    return "Overall responsibility for managing relationships with employers";
                case RoatpWorkflowPageIds.ReadinessToEngage.PromoteApprenticeshipsToEmployers:
                    return "Promote apprenticeships to employers";
                case RoatpWorkflowPageIds.ReadinessToEngage.ComplaintsPolicy:
                    return "Complaints policy";
                case RoatpWorkflowPageIds.ReadinessToEngage.ComplaintsPolicyWebsite:
                    return "Website link for the complaints policy";
                case RoatpWorkflowPageIds.ReadinessToEngage.ContractForServicesTemplate:
                    return "Contract for services template with employers";
                case RoatpWorkflowPageIds.ReadinessToEngage.CommitmentStatementTemplate:
                    return "Commitment statement template";
                case RoatpWorkflowPageIds.ReadinessToEngage.PriorLearningAssessment:
                    return "Process for initial assessments to recognise prior learning";
                case RoatpWorkflowPageIds.ReadinessToEngage.PriorLearningQualifications:
                    return "Process to assess English and maths";
                case RoatpWorkflowPageIds.ReadinessToEngage.SubcontractorsUse:
                    return "Using subcontractors in the first 12 months of joining the RoATP";
                case RoatpWorkflowPageIds.ReadinessToEngage.SubcontractorsDueDiligence:
                    return "Due diligence on subcontractors";

                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Main:
                    return "Type of apprenticeship training";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Employer:
                    return "Type of apprenticeship training";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Supporting:
                    return "Type of apprenticeship training";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.ApplicationStandards:
                    return "Delivering training in apprenticeship standards";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.ApplicationFrameworks_MainEmployer:
                    return "Offering apprenticeship frameworks";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.ApplicationFrameworks_Supporting:
                    return "Offering apprenticeship frameworks";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OrganisationTransition_MainEmployer:
                    return "Transitioning from apprenticeship frameworks to apprenticeship standards";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OrganisationTransition_Supporting:
                    return "Transitioning from apprenticeship frameworks to apprenticeship standards";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OnlyDeliveringApprenticeshipFrameworks_MainEmployer:
                    return "Delivering apprenticeship frameworks only";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OnlyDeliveringApprenticeshipFrameworks_Supporting:
                    return "Delivering apprenticeship frameworks only";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.ReadyToDeliverTraining:
                    return "Delivering training in apprenticeship standards";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.ReadyToDeliverTrainingResponsible:
                    return "Course directory";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.EngagingWithAwardingBodies:
                    return "Engaging and work with awarding bodies";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.EngagingWithAssessmentOrganisations:
                    return "Engaging with end-point assessment organisations (EPAO's)";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.EnsureApprenticesAreSupported:
                    return "Supporting apprentices during apprenticeship training";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.EnsureApprenticesAreSupportedHow:
                    return "Ways of supporting apprentices";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.EnsureApprenticesAreSupportedOtherWays:
                    return "Other ways of supporting apprentices";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.ForecastingStarts:
                    return "Forecasting starts in the first 12 months of joining the RoATP";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.ReadyToDeliverAgainstStarts:
                    return "Ready to deliver training against forecast";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.RecruitNewStaff:
                    return "Recruit new staff to deliver training against forecast";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.RatioOfStaffToApprentices:
                    return "Typical ratio of the staff delivering training to the apprentices";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OnTheJobTrainingTeachingMethods:
                    return "Methods used to deliver 20% off the job training";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OnTheJobTrainingTeachingRelevance:
                    return "Off the job training relevant to apprenticeship being delivered";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.AddressWhereApprenticesWillBeTrained:
                    return "Where apprentices will be trained";

                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy:
                    return "Management hierarchy for apprenticeships";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ChooseYourOrganisationsSectors:
                    return "Sectors and employee experience";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.AgricultureEnvironmentalAndAnimalCare.MostExperiencedEmployee:
                    return "Employee experience in 'Agriculture, environmental and animal care' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.BusinessAndAdministration.MostExperiencedEmployee:
                    return "Employee experience in 'Business and administration' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.CareServices.MostExperiencedEmployee:
                    return "Employee experience in 'Care Services' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.CateringAndHospitality.MostExperiencedEmployee:
                    return "Employee experience in 'Catering and hospitality' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.Construction.MostExperiencedEmployee:
                    return "Employee experience in 'Construction' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.CreativeAndDesign.MostExperiencedEmployee:
                    return "Employee experience in 'Creative and design' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.Digital.MostExperiencedEmployee:
                    return "Employee experience in 'Digital' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.EducationAndChildcare.MostExperiencedEmployee:
                    return "Employee experience in 'Education and childcare' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.EngineeringAndManufacturing.MostExperiencedEmployee:
                    return "Employee experience in 'Engineering and manufacturing' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.HairAndBeauty.MostExperiencedEmployee:
                    return "Employee experience in 'Hair and Beauty' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.HealthAndScience.MostExperiencedEmployee:
                    return "Employee experience in 'Health and Science' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.LegalFinanceAndAccounting.MostExperiencedEmployee:
                    return "Employee experience in 'Legal, finance and accounting' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ProtectiveServices.MostExperiencedEmployee:
                    return "Employee experience in 'Protective services' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.SalesMarketingAndProcurement.MostExperiencedEmployee:
                    return "Employee experience in 'Sales, marketing and procurement' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.TransportAndLogistics.MostExperiencedEmployee:
                    return "Employee experience in 'Transport and logistics' sector";
                default:
                    return null;
            }
        }
    }
}
