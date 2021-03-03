using System;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Sectors;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Services.Assessor
{
    public class AssessorSectorDetailsService: IAssessorSectorDetailsService
    {
        private readonly IAssessorLookupService _lookupService;
        private readonly IAssessorPageService _pageService;
        private readonly IExtractAnswerValueService _extractAnswerValueService;

        public AssessorSectorDetailsService(IAssessorLookupService assessorLookupService, IAssessorPageService assessorPageService, IExtractAnswerValueService extractAnswerValueService)
        {
            _lookupService = assessorLookupService;
            _pageService = assessorPageService;
            _extractAnswerValueService = extractAnswerValueService;
        }

        public async Task<AssessorSectorDetails> GetSectorDetails(Guid applicationId, string pageId)
        {
            var sectorDetails = new AssessorSectorDetails
            {
                SectorName = _lookupService.GetSectorNameForPage(pageId)
            };

            var sectorPageIds = _lookupService.GetSectorQuestionIdsForSectorPageId(pageId);

            if (sectorPageIds == null)
                return null;

            var sequenceNumber = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining;
            var sectionNumber = RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees;
            var page4ExperienceQualificationsMemberships = new AssessorPage();
            var page5TypeOfTraining = new AssessorPage();

            var page1HowManyStarts = await _pageService.GetPage(applicationId, sequenceNumber, sectionNumber, pageId);
            if (page1HowManyStarts == null)
                return null;

            HydrateSectorDetailsWithHowManyStarts(page1HowManyStarts,sectorDetails,sectorPageIds);

            var page2HowManyEmployees = await _pageService.GetPage(applicationId, sequenceNumber, sectionNumber, page1HowManyStarts.NextPageId);

            HydrateSectorDetailsWithHowManyEmployees(page2HowManyEmployees,sectorDetails,sectorPageIds);

            var page3NameRoleExperience = await _pageService.GetPage(applicationId, sequenceNumber, sectionNumber, page2HowManyEmployees.NextPageId);
            
            HydrateSectorDetailsWithFullNameJobRoleTimeInRole(page3NameRoleExperience, sectorDetails, sectorPageIds);

            if (!string.IsNullOrEmpty(page3NameRoleExperience.NextPageId))
            {
                page4ExperienceQualificationsMemberships = await _pageService.GetPage(applicationId, sequenceNumber,
                    sectionNumber, page3NameRoleExperience.NextPageId);

                HydrateSectorDetailsWithQualificationsAwardingBodiesAndTradeMemberships(sectorDetails, page4ExperienceQualificationsMemberships, sectorPageIds);
            }

            if (!string.IsNullOrEmpty(page4ExperienceQualificationsMemberships.NextPageId))
            {
                page5TypeOfTraining = await _pageService.GetPage(applicationId, sequenceNumber, sectionNumber,
                    page4ExperienceQualificationsMemberships.NextPageId);

                HydrateSectorDetailsWhatTypeOfTrainingDelivered(sectorDetails, page5TypeOfTraining, sectorPageIds);
            }

            if (!string.IsNullOrEmpty(page5TypeOfTraining.NextPageId))
            {
                var page4HowDeliveredAndDuration = await _pageService.GetPage(applicationId, sequenceNumber, sectionNumber,
                    page5TypeOfTraining.NextPageId);

                HydrateSectorDetailsWithHowTrainingIsDeliveredDetails(page4HowDeliveredAndDuration, sectorPageIds, sectorDetails);
            }

            return sectorDetails;
        }



        private void HydrateSectorDetailsWithHowManyStarts(AssessorPage page1HowManyStarts, AssessorSectorDetails sectorDetails, SectorQuestionIds sectorPageIds)
        {
            if (page1HowManyStarts?.Answers == null || !page1HowManyStarts.Answers.Any()) return; 
            sectorDetails.HowManyStarts = _extractAnswerValueService.ExtractAnswerValueFromQuestionId(page1HowManyStarts.Answers,
                sectorPageIds.HowManyStarts);
        }

        private void HydrateSectorDetailsWithHowManyEmployees(AssessorPage pageHowManyEmployees, AssessorSectorDetails sectorDetails, SectorQuestionIds sectorPageIds)
        {
            if (pageHowManyEmployees?.Answers == null || !pageHowManyEmployees.Answers.Any()) return;
            sectorDetails.HowManyEmployees = _extractAnswerValueService.ExtractAnswerValueFromQuestionId(pageHowManyEmployees.Answers,
                sectorPageIds.HowManyEmployees);
        }

        private void HydrateSectorDetailsWithFullNameJobRoleTimeInRole(AssessorPage page3NameRoleExperience,
         AssessorSectorDetails sectorDetails, SectorQuestionIds sectorPageIds)
        {
            if (page3NameRoleExperience?.Answers == null || !page3NameRoleExperience.Answers.Any()) return;
            sectorDetails.FullName =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(page3NameRoleExperience.Answers,
                    sectorPageIds.FullName);
            sectorDetails.JobRole =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(page3NameRoleExperience.Answers,
                    sectorPageIds.JobRole);
            sectorDetails.TimeInRole =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(page3NameRoleExperience.Answers,
                    sectorPageIds.TimeInRole);

            sectorDetails.IsPartOfAnyOtherOrganisations =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(page3NameRoleExperience.Answers,
                    sectorPageIds.IsPartOfAnyOtherOrganisations);

                sectorDetails.OtherOrganisations = _extractAnswerValueService.ExtractFurtherQuestionAnswerValueFromQuestionId(page3NameRoleExperience,
                sectorPageIds.IsPartOfAnyOtherOrganisations);

        }

        private void HydrateSectorDetailsWithQualificationsAwardingBodiesAndTradeMemberships(AssessorSectorDetails sectorDetails,
          AssessorPage page4ExperienceQualificationsMemberships, SectorQuestionIds sectorPageIds)
        {
            if (page4ExperienceQualificationsMemberships?.Answers == null || !page4ExperienceQualificationsMemberships.Answers.Any()) return;
            sectorDetails.ExperienceOfDelivering =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(
                    page4ExperienceQualificationsMemberships.Answers,
                    sectorPageIds.ExperienceOfDelivering);

            sectorDetails.WhereDidTheyGainThisExperience =
                _extractAnswerValueService.ExtractFurtherQuestionAnswerValueFromQuestionId(
                    page4ExperienceQualificationsMemberships,
                    sectorPageIds.ExperienceOfDelivering);

            sectorDetails.DoTheyHaveQualifications = _extractAnswerValueService.ExtractAnswerValueFromQuestionId(
                page4ExperienceQualificationsMemberships.Answers, sectorPageIds.DoTheyHaveQualifications);

            sectorDetails.WhatQualificationsDoTheyHave =
                _extractAnswerValueService.ExtractFurtherQuestionAnswerValueFromQuestionId(
                    page4ExperienceQualificationsMemberships,
                    sectorPageIds.DoTheyHaveQualifications);

            sectorDetails.ApprovedByAwardingBodies = _extractAnswerValueService.ExtractAnswerValueFromQuestionId(
                page4ExperienceQualificationsMemberships.Answers, sectorPageIds.AwardingBodies);

            sectorDetails.NamesOfAwardingBodies =
                _extractAnswerValueService.ExtractFurtherQuestionAnswerValueFromQuestionId(
                    page4ExperienceQualificationsMemberships,
                    sectorPageIds.AwardingBodies);


            sectorDetails.DoTheyHaveTradeBodyMemberships =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(
                    page4ExperienceQualificationsMemberships.Answers, sectorPageIds.TradeMemberships);

            sectorDetails.NamesOfTradeBodyMemberships =
                _extractAnswerValueService.ExtractFurtherQuestionAnswerValueFromQuestionId(
                    page4ExperienceQualificationsMemberships,
                    sectorPageIds.TradeMemberships);
        }

        private void HydrateSectorDetailsWhatTypeOfTrainingDelivered(AssessorSectorDetails sectorDetails,
            AssessorPage pageTypeOfTraining, SectorQuestionIds sectorPageIds)
        {
            if (pageTypeOfTraining?.Answers == null || !pageTypeOfTraining.Answers.Any()) return;
            sectorDetails.WhatTypeOfTrainingDelivered = _extractAnswerValueService.ExtractAnswerValueFromQuestionId(
                pageTypeOfTraining.Answers, sectorPageIds.WhatTypeOfTrainingDelivered);
        }

        private void HydrateSectorDetailsWithHowTrainingIsDeliveredDetails(AssessorPage pageHowDeliveredAndDuration,
            SectorQuestionIds sectorPageIds, AssessorSectorDetails sectorDetails)
        {
            if (pageHowDeliveredAndDuration?.Answers == null || !pageHowDeliveredAndDuration.Answers.Any()) return;
            var howHaveTheyDelivered =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(pageHowDeliveredAndDuration.Answers,
                    sectorPageIds.HowHaveTheyDeliveredTraining);

            var otherIsHowTheyDelivered =
                RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.DeliveringTrainingOther;

            if (howHaveTheyDelivered.Contains(otherIsHowTheyDelivered))
            {
                var otherWords =
                    _extractAnswerValueService.ExtractAnswerValueFromQuestionId(pageHowDeliveredAndDuration.Answers,
                        sectorPageIds.HowHaveTheyDeliveredTrainingOther);
                howHaveTheyDelivered = howHaveTheyDelivered.Replace(otherIsHowTheyDelivered, otherWords.Replace(",", "&#44;"));
            }

            sectorDetails.HowHaveTheyDeliveredTraining = howHaveTheyDelivered;
            
            sectorDetails.ExperienceOfDeliveringTraining =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(
                    pageHowDeliveredAndDuration.Answers, sectorPageIds.ExperienceOfDeliveringTraining);

            sectorDetails.TypicalDurationOfTraining = _extractAnswerValueService.ExtractAnswerValueFromQuestionId(
                pageHowDeliveredAndDuration.Answers, sectorPageIds.TypicalDurationOfTraining);
        }
    }
}

