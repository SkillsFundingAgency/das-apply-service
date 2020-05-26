using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Sectors;
using static SFA.DAS.ApplyService.Application.Apply.Roatp.RoatpWorkflowPageIds.DeliveringApprenticeshipTraining;

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
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining
                    .OnlyDeliveringApprenticeshipFrameworks_MainEmployer:
                    return "Delivering apprenticeship frameworks only";
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining
                    .OnlyDeliveringApprenticeshipFrameworks_Supporting:
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

                case OverallAccountability:
                    return "Overall accountability for apprenticeships";
                case ManagementHierarchy:
                    return "Management hierarchy for apprenticeships";
                case QualityAndHighStandards:
                    return
                        "Management hierarchy's expectations for quality and high standards in apprenticeship training";
                case QualityAndHighStandardsMonitoring:
                    return
                        "How expectations for quality and high standards in apprenticeship training are monitored and evaluated";
                case QualityAndHighStandardsResponsible:
                    return
                        "Overall responsibility for maintaining expectations for quality and high standards in apprenticeship training";
                case QualityAndHighStandardsExpectations:
                    return
                        "Expectations for quality and high standards in apprenticeship training communicated to employees";
                case TeamResponsible:
                    return "Team responsible for developing and delivering training";
                case PersonResponsible_SoleTrader:
                case PersonResponsible:
                    return "Someone responsible for developing and delivering training";
                case DevelopAndDeliverTraining_Team:
                    return "Who the team worked with to develop and deliver training";
                case OverallManager_MainEmployer:
                case OverallManager_Supporting:
                    return "Overall manager for the team responsible for developing and delivering training";
                case HowTeamWorked_Organisations:
                    return "How the team worked with other organisations to develop and deliver training";
                case HowTeamWorked_OrganisationsAndEmployers:
                    return "How the team worked with other organisations and employers to develop and deliver training";
                case HowTeamWorked_Employers:
                    return "How the team worked with employers to develop and deliver training";
                case DevelopAndDeliverTraining_Person:
                    return "Who has this person has worked with to develop and deliver training?";
                case HowPersonWorked_Organisations:
                    return "How has this person worked with other organisations to develop and deliver training";
                case HowPersonWorked_OrganisationsAndEmployers:
                    return
                        "How has this person worked with other organisations and employers to develop and deliver training";
                case HowPersonWorked_Employers:
                    return "How has this person worked with employers to develop and deliver training";
                case ChooseYourOrganisationsSectors:
                    return "Sectors and employee experience";
                case AgricultureEnvironmentalAndAnimalCare
                    .MostExperiencedEmployee:
                    return "Employee experience in 'Agriculture, environmental and animal care' sector";
                case BusinessAndAdministration
                    .MostExperiencedEmployee:
                    return "Employee experience in 'Business and administration' sector";
                case CareServices.MostExperiencedEmployee:
                    return "Employee experience in 'Care Services' sector";
                case CateringAndHospitality
                    .MostExperiencedEmployee:
                    return "Employee experience in 'Catering and hospitality' sector";
                case Construction.MostExperiencedEmployee:
                    return "Employee experience in 'Construction' sector";
                case CreativeAndDesign.MostExperiencedEmployee:
                    return "Employee experience in 'Creative and design' sector";
                case Digital.MostExperiencedEmployee:
                    return "Employee experience in 'Digital' sector";
                case EducationAndChildcare
                    .MostExperiencedEmployee:
                    return "Employee experience in 'Education and childcare' sector";
                case EngineeringAndManufacturing
                    .MostExperiencedEmployee:
                    return "Employee experience in 'Engineering and manufacturing' sector";
                case HairAndBeauty.MostExperiencedEmployee:
                    return "Employee experience in 'Hair and Beauty' sector";
                case HealthAndScience.MostExperiencedEmployee:
                    return "Employee experience in 'Health and Science' sector";
                case LegalFinanceAndAccounting
                    .MostExperiencedEmployee:
                    return "Employee experience in 'Legal, finance and accounting' sector";
                case ProtectiveServices.MostExperiencedEmployee:
                    return "Employee experience in 'Protective services' sector";
                case SalesMarketingAndProcurement
                    .MostExperiencedEmployee:
                    return "Employee experience in 'Sales, marketing and procurement' sector";
                case TransportAndLogistics
                    .MostExperiencedEmployee:
                    return "Employee experience in 'Transport and logistics' sector";
                case ProfessionalDevelopmentPolicy:
                    return "Policy for professional development of employees";
                case ImproveEmployeeSectorExpertise:
                    return "An example of how the policy is used to improve employee sector expertise";
                case ImproveEmployeeKnowledge:
                    return "An example of how the policy is used to maintain employee teaching and training knowledge";

                case RoatpWorkflowPageIds.EvaluatingApprenticeshipTraining.QualityProcessEvaluating:
                    return "Process for evaluating the quality of training delivered";
                case RoatpWorkflowPageIds.EvaluatingApprenticeshipTraining.QualityProcessImprovements:
                    return "Improvements made using process for evaluating the quality of training delivered";
                case RoatpWorkflowPageIds.EvaluatingApprenticeshipTraining.QualityProcessIncludesApprenticeshipTraining:
                    return "Process for evaluating the quality of training delivered include apprenticeship training";
                case RoatpWorkflowPageIds.EvaluatingApprenticeshipTraining.QualityProcessQuality_MainEmployer:
                    return "Evaluate the quality of apprenticeship training";
                case RoatpWorkflowPageIds.EvaluatingApprenticeshipTraining.QualityProcessQuality_Supporting:
                    return "Evaluate the quality of apprenticeship training";
                case RoatpWorkflowPageIds.EvaluatingApprenticeshipTraining.QualityProcessReviewing:
                    return "Review process for evaluating the quality of training delivered";
                case RoatpWorkflowPageIds.EvaluatingApprenticeshipTraining.CollectApprenticeshipData:
                    return "Systems and processes to collect apprenticeship data";
                case RoatpWorkflowPageIds.EvaluatingApprenticeshipTraining.IndividualisedLearnerRecordData:
                    return "Individualised Learner Record (ILR) data";

                default:
                    return null;
            }
        }

        public SectorQuestionIds GetSectorQuestionIdsForSectorPageId(string pageId)
        {
             switch (pageId)
            {
                case AgricultureEnvironmentalAndAnimalCare.MostExperiencedEmployee:
                    return AgricultureEnvironmentalAndAnimalCare.SectorQuestionIds;
                case BusinessAndAdministration.MostExperiencedEmployee:
                    return BusinessAndAdministration.SectorQuestionIds;
                case CareServices.MostExperiencedEmployee:
                    return CareServices.SectorQuestionIds;
                case CateringAndHospitality.MostExperiencedEmployee:
                    return CateringAndHospitality.SectorQuestionIds;
                case Construction.MostExperiencedEmployee:
                    return Construction.SectorQuestionIds;
                case CreativeAndDesign.MostExperiencedEmployee:
                    return CreativeAndDesign.SectorQuestionIds;
                case Digital.MostExperiencedEmployee:
                    return Digital.SectorQuestionIds;
                case EducationAndChildcare.MostExperiencedEmployee:
                    return EducationAndChildcare.SectorQuestionIds;
                case EngineeringAndManufacturing.MostExperiencedEmployee:
                    return EngineeringAndManufacturing.SectorQuestionIds;
                case HairAndBeauty.MostExperiencedEmployee:
                    return HairAndBeauty.SectorQuestionIds;
                case HealthAndScience.MostExperiencedEmployee:
                    return HealthAndScience.SectorQuestionIds;
                case LegalFinanceAndAccounting.MostExperiencedEmployee:
                    return LegalFinanceAndAccounting.SectorQuestionIds;
                case ProtectiveServices.MostExperiencedEmployee:
                    return ProtectiveServices.SectorQuestionIds;
                case SalesMarketingAndProcurement.MostExperiencedEmployee:
                    return SalesMarketingAndProcurement.SectorQuestionIds;
                case TransportAndLogistics.MostExperiencedEmployee:
                    return TransportAndLogistics.SectorQuestionIds;
                default:
                    return null;
            }
        }
    }
}