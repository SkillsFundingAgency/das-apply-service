using System;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Sectors;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public class SectorDetailsOrchestratorService: ISectorDetailsOrchestratorService
    {
        private readonly IAssessorLookupService _assessorLookupService;
        private readonly IGetAssessorPageService _getAssessorPageService;
        private readonly IExtractAnswerValueService _extractAnswerValueService;

        public SectorDetailsOrchestratorService(IAssessorLookupService assessorLookupService, IGetAssessorPageService getAssessorPageService, IExtractAnswerValueService extractAnswerValueService)
        {
            _assessorLookupService = assessorLookupService;
            _getAssessorPageService = getAssessorPageService;
            _extractAnswerValueService = extractAnswerValueService;
        }

        public async Task<AssessorSectorDetails> GetSectorDetails(Guid applicationId, string pageId)
        {
            var sectorDetails = new AssessorSectorDetails();

            sectorDetails.SectorName = _assessorLookupService.GetSectorNameForPage(pageId);
            var sectorPageIds = _assessorLookupService.GetSectorQuestionIdsForSectorPageId(pageId);

            if (sectorPageIds == null)
                return null;

            var sequenceNumber = RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining;
            var sectionNumber = RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.YourSectorsAndEmployees;
            var page2ExperienceQualificationsMemberships = new AssessorPage();
            var page3TypeOfTraining = new AssessorPage();

            var page1NameRoleExperience = await _getAssessorPageService.GetPage(applicationId, sequenceNumber, sectionNumber, pageId);

            if (page1NameRoleExperience == null)
                return null;

            HydrateSectorDetailsWithFullNameJobRoleTimeInRole(page1NameRoleExperience, sectorDetails, sectorPageIds);

            if (!string.IsNullOrEmpty(page1NameRoleExperience.NextPageId))
            {
                page2ExperienceQualificationsMemberships = await _getAssessorPageService.GetPage(applicationId, sequenceNumber,
                    sectionNumber, page1NameRoleExperience.NextPageId);

                HydrateSectorDetailsWithQualificationsAwardingBodiesAndTradeMemberships(sectorDetails, page2ExperienceQualificationsMemberships, sectorPageIds);
            }

            if (!string.IsNullOrEmpty(page2ExperienceQualificationsMemberships.NextPageId))
            {
                page3TypeOfTraining = await _getAssessorPageService.GetPage(applicationId, sequenceNumber, sectionNumber,
                    page2ExperienceQualificationsMemberships.NextPageId);

                HydrateSectorDetailsWhatTypeOfTrainingDelivered(sectorDetails, page3TypeOfTraining, sectorPageIds);
            }

            if (!string.IsNullOrEmpty(page3TypeOfTraining.NextPageId))
            {
                var page4HowDeliveredAndDuration = await _getAssessorPageService.GetPage(applicationId, sequenceNumber, sectionNumber,
                    page3TypeOfTraining.NextPageId);

                HydrateSectorDetailsWithHowTrainingIsDeliveredDetails(page4HowDeliveredAndDuration, sectorPageIds, sectorDetails);
            }

            return sectorDetails;
        }

        private void HydrateSectorDetailsWithFullNameJobRoleTimeInRole(AssessorPage page1NameRoleExperience,
         AssessorSectorDetails sectorDetails, SectorQuestionIds sectorPageIds)
        {
            if (page1NameRoleExperience?.Answers == null || !page1NameRoleExperience.Answers.Any()) return;
            sectorDetails.FullName =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(page1NameRoleExperience.Answers,
                    sectorPageIds.FullName);
            sectorDetails.JobRole =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(page1NameRoleExperience.Answers,
                    sectorPageIds.JobRole);
            sectorDetails.TimeInRole =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(page1NameRoleExperience.Answers,
                    sectorPageIds.TimeInRole);
        }

        private void HydrateSectorDetailsWithQualificationsAwardingBodiesAndTradeMemberships(AssessorSectorDetails sectorDetails,
          AssessorPage page2ExperienceQualificationsMemberships, SectorQuestionIds sectorPageIds)
        {
            if (page2ExperienceQualificationsMemberships?.Answers == null || !page2ExperienceQualificationsMemberships.Answers.Any()) return;
            sectorDetails.ExperienceOfDelivering =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(
                    page2ExperienceQualificationsMemberships.Answers,
                    sectorPageIds.ExperienceOfDelivering);

            sectorDetails.WhereDidTheyGainThisExperience =
                _extractAnswerValueService.ExtractFurtherQuestionAnswerValueFromQuestionId(
                    page2ExperienceQualificationsMemberships,
                    sectorPageIds.ExperienceOfDelivering);

            sectorDetails.DoTheyHaveQualifications = _extractAnswerValueService.ExtractAnswerValueFromQuestionId(
                page2ExperienceQualificationsMemberships.Answers, sectorPageIds.DoTheyHaveQualifications);

            sectorDetails.WhatQualificationsDoTheyHave =
                _extractAnswerValueService.ExtractFurtherQuestionAnswerValueFromQuestionId(
                    page2ExperienceQualificationsMemberships,
                    sectorPageIds.DoTheyHaveQualifications);

            sectorDetails.ApprovedByAwardingBodies = _extractAnswerValueService.ExtractAnswerValueFromQuestionId(
                page2ExperienceQualificationsMemberships.Answers, sectorPageIds.AwardingBodies);

            sectorDetails.NamesOfAwardingBodies =
                _extractAnswerValueService.ExtractFurtherQuestionAnswerValueFromQuestionId(
                    page2ExperienceQualificationsMemberships,
                    sectorPageIds.AwardingBodies);


            sectorDetails.DoTheyHaveTradeBodyMemberships =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(
                    page2ExperienceQualificationsMemberships.Answers, sectorPageIds.TradeMemberships);

            sectorDetails.NamesOfTradeBodyMemberships =
                _extractAnswerValueService.ExtractFurtherQuestionAnswerValueFromQuestionId(
                    page2ExperienceQualificationsMemberships,
                    sectorPageIds.TradeMemberships);
        }

        private void HydrateSectorDetailsWhatTypeOfTrainingDelivered(AssessorSectorDetails sectorDetails,
            AssessorPage page3TypeOfTraining, SectorQuestionIds sectorPageIds)
        {
            if (page3TypeOfTraining?.Answers == null || !page3TypeOfTraining.Answers.Any()) return;
            sectorDetails.WhatTypeOfTrainingDelivered = _extractAnswerValueService.ExtractAnswerValueFromQuestionId(
                page3TypeOfTraining.Answers, sectorPageIds.WhatTypeOfTrainingDelivered);
        }

        private void HydrateSectorDetailsWithHowTrainingIsDeliveredDetails(AssessorPage page4HowDeliveredAndDuration,
            SectorQuestionIds sectorPageIds, AssessorSectorDetails sectorDetails)
        {
            if (page4HowDeliveredAndDuration?.Answers == null || !page4HowDeliveredAndDuration.Answers.Any()) return;
            var howHaveTheyDelivered =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(page4HowDeliveredAndDuration.Answers,
                    sectorPageIds.HowHaveTheyDeliveredTraining);

            var otherIsHowTheyDelivered =
                RoatpWorkflowPageIds.DeliveringApprenticeshipTraining.DeliveringTrainingOther;

            if (howHaveTheyDelivered.Contains(otherIsHowTheyDelivered))
            {
                var otherWords =
                    _extractAnswerValueService.ExtractAnswerValueFromQuestionId(page4HowDeliveredAndDuration.Answers,
                        sectorPageIds.HowHaveTheyDeliveredTrainingOther);
                howHaveTheyDelivered = howHaveTheyDelivered.Replace(otherIsHowTheyDelivered, otherWords.Replace(",", "&#44;"));
            }

            sectorDetails.HowHaveTheyDeliveredTraining = howHaveTheyDelivered;
            
            sectorDetails.ExperienceOfDeliveringTraining =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(
                    page4HowDeliveredAndDuration.Answers, sectorPageIds.ExperienceOfDeliveringTraining);

            sectorDetails.TypicalDurationOfTraining = _extractAnswerValueService.ExtractAnswerValueFromQuestionId(
                page4HowDeliveredAndDuration.Answers, sectorPageIds.TypicalDurationOfTraining);
        }
    }
}

