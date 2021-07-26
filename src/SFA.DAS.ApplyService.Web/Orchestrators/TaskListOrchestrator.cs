using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Orchestrators
{
    public class TaskListOrchestrator : ITaskListOrchestrator
    {
        private readonly IApplicationApiClient _apiClient;
        private readonly IQnaApiClient _qnaApiClient;
        private readonly IRoatpOrganisationVerificationService _organisationVerificationService;
        private readonly IRoatpTaskListWorkflowService _roatpTaskListWorkflowService;
        private readonly INotRequiredOverridesService _notRequiredOverridesService;

        public TaskListOrchestrator(IApplicationApiClient apiClient,
            IQnaApiClient qnaApiClient,
            IRoatpOrganisationVerificationService organisationVerificationService,
            IRoatpTaskListWorkflowService roatpTaskListWorkflowService,
            INotRequiredOverridesService notRequiredOverridesService)
        {
            _apiClient = apiClient;
            _qnaApiClient = qnaApiClient;
            _organisationVerificationService = organisationVerificationService;
            _roatpTaskListWorkflowService = roatpTaskListWorkflowService;
            _notRequiredOverridesService = notRequiredOverridesService;
        }

        public async Task<TaskListViewModel> GetTaskListViewModel(Guid applicationId, Guid userId)
        {
            var organisationDetailsTask = _apiClient.GetOrganisationByUserId(userId);
            var providerRouteTask = _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute);
            var organisationVerificationStatusTask = _organisationVerificationService.GetOrganisationVerificationStatus(applicationId);
            var refreshNotRequiredOverridesTask = _notRequiredOverridesService.RefreshNotRequiredOverridesAsync(applicationId);
            var sequencesTask = _roatpTaskListWorkflowService.GetApplicationSequences(applicationId);

            await Task.WhenAll(organisationDetailsTask, providerRouteTask, organisationVerificationStatusTask, refreshNotRequiredOverridesTask, sequencesTask);

            var organisationDetails = await organisationDetailsTask;
            var providerRoute = await providerRouteTask;
            var organisationVerificationStatus = await organisationVerificationStatusTask;
            var sequences = (await sequencesTask).ToList();

            var yourOrganisationSequenceCompleted = await IsYourOrganisationSequenceCompleted(applicationId, sequences, organisationVerificationStatus);
            var applicationSequencesCompleted = await ApplicationSequencesCompleted(applicationId, sequences, organisationVerificationStatus);
            var lastFinishSectionCompleted = await _roatpTaskListWorkflowService.PreviousSectionCompleted(applicationId, RoatpWorkflowSequenceIds.Finish, RoatpWorkflowSectionIds.Finish.SubmitApplication, sequences, organisationVerificationStatus);

            var result = new TaskListViewModel
            {
                ApplicationId = applicationId,
                ApplicationRouteId = providerRoute.Value,
                ApplicationSummaryViewModel = new ApplicationSummaryViewModel
                {
                    ApplicationId = applicationId,
                    UKPRN = organisationDetails.OrganisationUkprn?.ToString(),
                    OrganisationName = organisationDetails.Name,
                    TradingName = organisationDetails.OrganisationDetails?.TradingName,
                    ApplicationRouteId = providerRoute.Value,
                },
                AllowSubmission = applicationSequencesCompleted && lastFinishSectionCompleted
            };

            foreach (var sequence in sequences)
            {
                var sequenceVm = new TaskListViewModel.Sequence
                {
                    Id = sequence.SequenceId,
                    Description = sequence.Description
                };

                result.Sequences.Add(sequenceVm);

                foreach (var section in sequence.Sections.OrderBy(x => x.SectionId))
                {
                    sequenceVm.Sections.Add(new TaskListViewModel.Section
                    {
                        Id = section.SectionId,
                        Title = section.Title,
                        IsNotRequired = await _roatpTaskListWorkflowService.SectionNotRequired(applicationId, sequence.SequenceId, section.SectionId),
                        Status = sequence.SequenceId == RoatpWorkflowSequenceIds.Finish
                                ? await _roatpTaskListWorkflowService.FinishSectionStatus(applicationId, section.SectionId, sequences, applicationSequencesCompleted)
                                : await _roatpTaskListWorkflowService.SectionStatusAsync(applicationId, sequence.SequenceId, section.SectionId, sequences, organisationVerificationStatus),
                        IsLocked = await IsSectionLocked(applicationId, sequence.SequenceId, section.SectionId, sequences, organisationVerificationStatus, yourOrganisationSequenceCompleted, applicationSequencesCompleted)
                    });
                }
            }

            return result;
        }

        private async Task<bool> IsSectionLocked(Guid applicationId, int sequenceId, int sectionId, List<ApplicationSequence> sequences, OrganisationVerificationStatus organisationVerificationStatus, bool yourOrganisationSequenceCompleted, bool applicationSequencesCompleted)
        {
            // Disable the other sequences if YourOrganisation sequence isn't complete
            if (sequenceId != RoatpWorkflowSequenceIds.YourOrganisation && !yourOrganisationSequenceCompleted)
            {
                return true;
            }

            //Within Your Organisation, sections are locked until the previous one is completed
            if (sequenceId == RoatpWorkflowSequenceIds.YourOrganisation && sectionId != 1)
            {
                return ! await _roatpTaskListWorkflowService.PreviousSectionCompleted(applicationId, sequenceId, sectionId, sequences, organisationVerificationStatus);
            }

            //Entire Finish section is locked until all app sequences are completed, and sections are locked until previous one is completed
            if (sequenceId == RoatpWorkflowSequenceIds.Finish)
            {
                return !applicationSequencesCompleted 
                    || ! await _roatpTaskListWorkflowService.PreviousSectionCompleted(applicationId, sequenceId, sectionId, sequences, organisationVerificationStatus);
            }

            // CriminalComplianceChecks has two intro pages...
            if (sequenceId == RoatpWorkflowSequenceIds.CriminalComplianceChecks)
            {
                var SecondCriminialIntroductionSectionId = 3;
                if (sectionId > SecondCriminialIntroductionSectionId)
                {
                    var statusOfSecondIntroductionPage = _roatpTaskListWorkflowService.SectionQuestionsStatus(applicationId, RoatpWorkflowSequenceIds.CriminalComplianceChecks, SecondCriminialIntroductionSectionId, sequences);
                    if (statusOfSecondIntroductionPage != TaskListSectionStatus.Completed)
                    {
                        return true;
                    }
                }
            }

            var statusOfIntroductionPage = _roatpTaskListWorkflowService.SectionQuestionsStatus(applicationId, sequenceId, 1, sequences);
            if (sequenceId > 1 && sectionId != 1 && statusOfIntroductionPage != TaskListSectionStatus.Completed)
            {
                return true;
            }

            return false;
        }

        private async Task<bool> IsYourOrganisationSequenceCompleted(Guid applicationId, IEnumerable<ApplicationSequence> sequences, OrganisationVerificationStatus organisationVerificationStatus)
        {
            var yourOrganisationSequenceCompleted = true;

            var yourOrganisationSequence = sequences.First(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);

            foreach (var section in yourOrganisationSequence.Sections)
            {
                var sectionStatus = await _roatpTaskListWorkflowService.SectionStatusAsync(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, section.SectionId, sequences, organisationVerificationStatus);
                if (sectionStatus != TaskListSectionStatus.NotRequired &&  sectionStatus != TaskListSectionStatus.Completed)
                {
                    yourOrganisationSequenceCompleted = false;
                    break;
                }
            }

            return yourOrganisationSequenceCompleted;
        }

        private async Task<bool> ApplicationSequencesCompleted(Guid applicationId, IEnumerable<ApplicationSequence> sequences, OrganisationVerificationStatus organisationVerificationStatus)
        {
            var nonFinishSequences = sequences.Where(seq => seq.SequenceId != RoatpWorkflowSequenceIds.Finish).ToList();
            foreach (var sequence in nonFinishSequences)
            {
                foreach (var section in sequence.Sections)
                {
                    var sectionStatus = await _roatpTaskListWorkflowService.SectionStatusAsync(applicationId, sequence.SequenceId, section.SectionId, sequences, organisationVerificationStatus);
                    if (sectionStatus != TaskListSectionStatus.NotRequired && sectionStatus != TaskListSectionStatus.Completed)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
