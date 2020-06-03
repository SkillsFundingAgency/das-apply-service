using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Sectors;

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
                case RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OnlyDeliveringApprenticeshipFrameworks_MainEmployer:
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

                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.OverallAccountability:
                    return "Overall accountability for apprenticeships";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ManagementHierarchy:
                    return "Management hierarchy for apprenticeships";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.QualityAndHighStandards:
                    return
                        "Management hierarchy's expectations for quality and high standards in apprenticeship training";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.QualityAndHighStandardsMonitoring:
                    return
                        "How expectations for quality and high standards in apprenticeship training are monitored and evaluated";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.QualityAndHighStandardsResponsible:
                    return
                        "Overall responsibility for maintaining expectations for quality and high standards in apprenticeship training";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.QualityAndHighStandardsExpectations:
                    return
                        "Expectations for quality and high standards in apprenticeship training communicated to employees";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.TeamResponsible:
                    return "Team responsible for developing and delivering training";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.PersonResponsible_SoleTrader:
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.PersonResponsible:
                    return "Someone responsible for developing and delivering training";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.DevelopAndDeliverTraining_Team:
                    return "Who the team worked with to develop and deliver training";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.OverallManager_MainEmployer:
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.OverallManager_Supporting:
                    return "Overall manager for the team responsible for developing and delivering training";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.HowTeamWorked_Organisations:
                    return "How the team worked with other organisations to develop and deliver training";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.HowTeamWorked_OrganisationsAndEmployers:
                    return "How the team worked with other organisations and employers to develop and deliver training";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.HowTeamWorked_Employers:
                    return "How the team worked with employers to develop and deliver training";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.DevelopAndDeliverTraining_Person:
                    return "Who has this person has worked with to develop and deliver training?";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.HowPersonWorked_Organisations:
                    return "How has this person worked with other organisations to develop and deliver training";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.HowPersonWorked_OrganisationsAndEmployers:
                    return
                        "How has this person worked with other organisations and employers to develop and deliver training";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.HowPersonWorked_Employers:
                    return "How has this person worked with employers to develop and deliver training";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ChooseYourOrganisationsSectors:
                    return "Sectors and employee experience";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.AgricultureEnvironmentalAndAnimalCare
                    .MostExperiencedEmployee:
                    return "Employee experience in 'Agriculture, environmental and animal care' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.BusinessAndAdministration
                    .MostExperiencedEmployee:
                    return "Employee experience in 'Business and administration' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.CareServices.MostExperiencedEmployee:
                    return "Employee experience in 'Care Services' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.CateringAndHospitality
                    .MostExperiencedEmployee:
                    return "Employee experience in 'Catering and hospitality' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.Construction.MostExperiencedEmployee:
                    return "Employee experience in 'Construction' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.CreativeAndDesign.MostExperiencedEmployee:
                    return "Employee experience in 'Creative and design' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.Digital.MostExperiencedEmployee:
                    return "Employee experience in 'Digital' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.EducationAndChildcare
                    .MostExperiencedEmployee:
                    return "Employee experience in 'Education and childcare' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.EngineeringAndManufacturing
                    .MostExperiencedEmployee:
                    return "Employee experience in 'Engineering and manufacturing' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.HairAndBeauty.MostExperiencedEmployee:
                    return "Employee experience in 'Hair and Beauty' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.HealthAndScience.MostExperiencedEmployee:
                    return "Employee experience in 'Health and Science' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.LegalFinanceAndAccounting
                    .MostExperiencedEmployee:
                    return "Employee experience in 'Legal, finance and accounting' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ProtectiveServices.MostExperiencedEmployee:
                    return "Employee experience in 'Protective services' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.SalesMarketingAndProcurement
                    .MostExperiencedEmployee:
                    return "Employee experience in 'Sales, marketing and procurement' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.TransportAndLogistics
                    .MostExperiencedEmployee:
                    return "Employee experience in 'Transport and logistics' sector";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ProfessionalDevelopmentPolicy:
                    return "Policy for professional development of employees";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ImproveEmployeeSectorExpertise:
                    return "An example of how the policy is used to improve employee sector expertise";
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ImproveEmployeeKnowledge:
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
                public string GetLabelForQuestion(string questionId)
        {
            switch (questionId)
            {
                case RoatpPlanningApprenticeshipTrainingQuestionIdConstants.ApplicationFrameworks_MainEmployer:
                case RoatpPlanningApprenticeshipTrainingQuestionIdConstants.ApplicationFrameworks_Supporting:
                    return "Does your organisation have a plan to transition from apprenticeship frameworks to apprenticeship standards?";
                default:
                    return null;
            }
        }

        public string GetSectorNameForPage(string pageId)
        {
            switch (questionId)
            {
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.AgricultureEnvironmentalAndAnimalCare.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.AgricultureEnvironmentalAndAnimalCare.Name;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.BusinessAndAdministration.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.BusinessAndAdministration.Name;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.CareServices.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.CareServices.Name;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.CateringAndHospitality.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.CateringAndHospitality.Name;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.Construction.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.Construction.Name;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.CreativeAndDesign.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.CreativeAndDesign.Name;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.Digital.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.Digital.Name;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.EducationAndChildcare.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.EducationAndChildcare.Name;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.EngineeringAndManufacturing.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.EngineeringAndManufacturing.Name;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.HairAndBeauty.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.HairAndBeauty.Name;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.HealthAndScience.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.HealthAndScience.Name;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.LegalFinanceAndAccounting.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.LegalFinanceAndAccounting.Name;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ProtectiveServices.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ProtectiveServices.Name;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.SalesMarketingAndProcurement.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.SalesMarketingAndProcurement.Name;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.TransportAndLogistics.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.TransportAndLogistics.Name;
                default:
                    return null;
            }
        }

        public SectorQuestionIds GetSectorQuestionIdsForSectorPageId(string pageId)
        {
             switch (pageId)
            {
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.AgricultureEnvironmentalAndAnimalCare.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.AgricultureEnvironmentalAndAnimalCare.SectorQuestionIds;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.BusinessAndAdministration.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.BusinessAndAdministration.SectorQuestionIds;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.CareServices.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.CareServices.SectorQuestionIds;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.CateringAndHospitality.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.CateringAndHospitality.SectorQuestionIds;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.Construction.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.Construction.SectorQuestionIds;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.CreativeAndDesign.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.CreativeAndDesign.SectorQuestionIds;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.Digital.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.Digital.SectorQuestionIds;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.EducationAndChildcare.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.EducationAndChildcare.SectorQuestionIds;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.EngineeringAndManufacturing.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.EngineeringAndManufacturing.SectorQuestionIds;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.HairAndBeauty.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.HairAndBeauty.SectorQuestionIds;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.HealthAndScience.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.HealthAndScience.SectorQuestionIds;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.LegalFinanceAndAccounting.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.LegalFinanceAndAccounting.SectorQuestionIds;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ProtectiveServices.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.ProtectiveServices.SectorQuestionIds;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.SalesMarketingAndProcurement.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.SalesMarketingAndProcurement.SectorQuestionIds;
                case RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.TransportAndLogistics.MostExperiencedEmployee:
                    return RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.TransportAndLogistics.SectorQuestionIds;
                default:
                    return null;
            }
        }
    }
}