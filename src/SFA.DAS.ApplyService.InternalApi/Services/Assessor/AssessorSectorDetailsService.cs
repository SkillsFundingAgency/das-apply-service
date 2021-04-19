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
            var pageExperienceQualificationsMemberships = new AssessorPage();
            var pageTypeOfTraining = new AssessorPage();

            var pageWhatStandardsOffered = await _pageService.GetPage(applicationId, sequenceNumber, sectionNumber, pageId);
            if (pageWhatStandardsOffered == null)
                return null;

            HydrateSectorDetailsWithWhatStandardsOffered(pageWhatStandardsOffered, sectorDetails, sectorPageIds);
            
            var pageHowManyStarts= await _pageService.GetPage(applicationId, sequenceNumber, sectionNumber, pageWhatStandardsOffered.NextPageId);

            HydrateSectorDetailsWithHowManyStarts(pageHowManyStarts, sectorDetails,sectorPageIds);

            var pageHowManyEmployees = await _pageService.GetPage(applicationId, sequenceNumber, sectionNumber, pageHowManyStarts.NextPageId);

            HydrateSectorDetailsWithHowManyEmployees(pageHowManyEmployees,sectorDetails,sectorPageIds);

            var pageNameRoleExperience = await _pageService.GetPage(applicationId, sequenceNumber, sectionNumber, pageHowManyEmployees.NextPageId);
            
            HydrateSectorDetailsWithFullNameJobRoleTimeInRole(pageNameRoleExperience, sectorDetails, sectorPageIds);

            if (!string.IsNullOrEmpty(pageNameRoleExperience.NextPageId))
            {
                pageExperienceQualificationsMemberships = await _pageService.GetPage(applicationId, sequenceNumber,
                    sectionNumber, pageNameRoleExperience.NextPageId);

                HydrateSectorDetailsWithQualificationsAwardingBodiesAndTradeMemberships(sectorDetails, pageExperienceQualificationsMemberships, sectorPageIds);
            }

            if (!string.IsNullOrEmpty(pageExperienceQualificationsMemberships.NextPageId))
            {
                pageTypeOfTraining = await _pageService.GetPage(applicationId, sequenceNumber, sectionNumber,
                    pageExperienceQualificationsMemberships.NextPageId);

                HydrateSectorDetailsWhatTypeOfTrainingDelivered(sectorDetails, pageTypeOfTraining, sectorPageIds);
            }

            if (!string.IsNullOrEmpty(pageTypeOfTraining.NextPageId))
            {
                var pageHowDeliveredAndDuration = await _pageService.GetPage(applicationId, sequenceNumber, sectionNumber,
                    pageTypeOfTraining.NextPageId);

                HydrateSectorDetailsWithHowTrainingIsDeliveredDetails(pageHowDeliveredAndDuration, sectorPageIds, sectorDetails);
            }

            return sectorDetails;
        }

        private void HydrateSectorDetailsWithWhatStandardsOffered(AssessorPage pageWhatStandardsOffered, AssessorSectorDetails sectorDetails, SectorQuestionIds sectorPageIds)
        {
            if (pageWhatStandardsOffered?.Answers == null || !pageWhatStandardsOffered.Answers.Any()) return;
            sectorDetails.WhatStandardsOffered = _extractAnswerValueService.ExtractAnswerValueFromQuestionId(pageWhatStandardsOffered.Answers,
                sectorPageIds.WhatStandardsOffered);
        }

        private void HydrateSectorDetailsWithHowManyStarts(AssessorPage pageHowManyStarts, AssessorSectorDetails sectorDetails, SectorQuestionIds sectorPageIds)
        {
            if (pageHowManyStarts?.Answers == null || !pageHowManyStarts.Answers.Any()) return; 
            sectorDetails.HowManyStarts = _extractAnswerValueService.ExtractAnswerValueFromQuestionId(pageHowManyStarts.Answers,
                sectorPageIds.HowManyStarts);
        }

        private void HydrateSectorDetailsWithHowManyEmployees(AssessorPage pageHowManyEmployees, AssessorSectorDetails sectorDetails, SectorQuestionIds sectorPageIds)
        {
            if (pageHowManyEmployees?.Answers == null || !pageHowManyEmployees.Answers.Any()) return;
            sectorDetails.HowManyEmployees = _extractAnswerValueService.ExtractAnswerValueFromQuestionId(pageHowManyEmployees.Answers,
                sectorPageIds.HowManyEmployees);
        }

        private void HydrateSectorDetailsWithFullNameJobRoleTimeInRole(AssessorPage page4NameRoleExperience,
         AssessorSectorDetails sectorDetails, SectorQuestionIds sectorPageIds)
        {
            if (page4NameRoleExperience?.Answers == null || !page4NameRoleExperience.Answers.Any()) return;
            sectorDetails.FullName =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(page4NameRoleExperience.Answers,
                    sectorPageIds.FullName);
            sectorDetails.JobRole =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(page4NameRoleExperience.Answers,
                    sectorPageIds.JobRole);
            sectorDetails.TimeInRole =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(page4NameRoleExperience.Answers,
                    sectorPageIds.TimeInRole);

            sectorDetails.IsPartOfAnyOtherOrganisations =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(page4NameRoleExperience.Answers,
                    sectorPageIds.IsPartOfAnyOtherOrganisations);

                sectorDetails.OtherOrganisations = _extractAnswerValueService.ExtractFurtherQuestionAnswerValueFromQuestionId(page4NameRoleExperience,
                sectorPageIds.IsPartOfAnyOtherOrganisations);

        }

        private void HydrateSectorDetailsWithQualificationsAwardingBodiesAndTradeMemberships(AssessorSectorDetails sectorDetails,
          AssessorPage page5ExperienceQualificationsMemberships, SectorQuestionIds sectorPageIds)
        {
            if (page5ExperienceQualificationsMemberships?.Answers == null || !page5ExperienceQualificationsMemberships.Answers.Any()) return;
            sectorDetails.ExperienceOfDelivering =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(
                    page5ExperienceQualificationsMemberships.Answers,
                    sectorPageIds.ExperienceOfDelivering);

            sectorDetails.WhereDidTheyGainThisExperience =
                _extractAnswerValueService.ExtractFurtherQuestionAnswerValueFromQuestionId(
                    page5ExperienceQualificationsMemberships,
                    sectorPageIds.ExperienceOfDelivering);

            sectorDetails.DoTheyHaveQualifications = _extractAnswerValueService.ExtractAnswerValueFromQuestionId(
                page5ExperienceQualificationsMemberships.Answers, sectorPageIds.DoTheyHaveQualifications);

            sectorDetails.WhatQualificationsDoTheyHave =
                _extractAnswerValueService.ExtractFurtherQuestionAnswerValueFromQuestionId(
                    page5ExperienceQualificationsMemberships,
                    sectorPageIds.DoTheyHaveQualifications);

            sectorDetails.ApprovedByAwardingBodies = _extractAnswerValueService.ExtractAnswerValueFromQuestionId(
                page5ExperienceQualificationsMemberships.Answers, sectorPageIds.AwardingBodies);

            sectorDetails.NamesOfAwardingBodies =
                _extractAnswerValueService.ExtractFurtherQuestionAnswerValueFromQuestionId(
                    page5ExperienceQualificationsMemberships,
                    sectorPageIds.AwardingBodies);


            sectorDetails.DoTheyHaveTradeBodyMemberships =
                _extractAnswerValueService.ExtractAnswerValueFromQuestionId(
                    page5ExperienceQualificationsMemberships.Answers, sectorPageIds.TradeMemberships);

            sectorDetails.NamesOfTradeBodyMemberships =
                _extractAnswerValueService.ExtractFurtherQuestionAnswerValueFromQuestionId(
                    page5ExperienceQualificationsMemberships,
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

