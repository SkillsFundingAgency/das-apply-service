using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class TaskListService : ITaskListService
    {
        private readonly IApplicationApiClient _apiClient;
        private readonly IQnaApiClient _qnaApiClient;
        private readonly IRoatpOrganisationVerificationService _organisationVerificationService;
        private readonly IRoatpTaskListWorkflowService _roatpTaskListWorkflowService; //todo: consider removing this dependency, and copying the methods we need

        public TaskListService(IApplicationApiClient apiClient,
            IQnaApiClient qnaApiClient,
            IRoatpOrganisationVerificationService organisationVerificationService,
            IRoatpTaskListWorkflowService roatpTaskListWorkflowService)
        {
            _apiClient = apiClient;
            _qnaApiClient = qnaApiClient;
            _organisationVerificationService = organisationVerificationService;
            _roatpTaskListWorkflowService = roatpTaskListWorkflowService;
        }

        public async Task<TaskList2ViewModel> GetTaskList2ViewModel(Guid applicationId, Guid userId)
        {
            var result = new TaskList2ViewModel
            {
                ApplicationId = applicationId
            };

            var organisationDetails = await _apiClient.GetOrganisationByUserId(userId);
            var providerRoute = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ProviderRoute);

            result.ApplicationSummaryViewModel = new ApplicationSummaryViewModel
            {
                UKPRN = organisationDetails.OrganisationUkprn?.ToString(),
                OrganisationName = organisationDetails.Name,
                TradingName = organisationDetails.OrganisationDetails?.TradingName,
                ApplicationRouteId = providerRoute.Value,
            };

            var organisationVerificationStatus = await _organisationVerificationService.GetOrganisationVerificationStatus(applicationId);

            _roatpTaskListWorkflowService.RefreshNotRequiredOverrides(applicationId);
            var sequences = _roatpTaskListWorkflowService.GetApplicationSequences(applicationId).ToList(); //this method is not async

            var yourOrganisationSequence = sequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);

            var yourOrganisationSequenceCompleted = CheckYourOrganisationSequenceComplete(applicationId, sequences, organisationVerificationStatus, yourOrganisationSequence);
            var applicationSequencesCompleted = ApplicationSequencesCompleted(applicationId, sequences, organisationVerificationStatus);




            //new stuff below!


            foreach (var sequence in sequences)
            {
                var sequenceVm = new TaskList2ViewModel.Sequence
                {
                    Id = sequence.SequenceId,
                    Description = sequence.Description
                };

                result.Sequences.Add(sequenceVm);

                foreach (var section in sequence.Sections.OrderBy(x => x.SectionId))
                {
                    sequenceVm.Sections.Add(new TaskList2ViewModel.Section
                    {
                        Id = section.SectionId,
                        Title = section.Title,
                        IsNotRequired = _roatpTaskListWorkflowService.SectionNotRequired(applicationId,
                            sequence.SequenceId, section.SectionId),
                        Status = _roatpTaskListWorkflowService.SectionStatus(applicationId, sequence.SequenceId,
                            section.SectionId, sequences, organisationVerificationStatus),
                        IntroductionPageNextSectionUnavailable = IntroductionPageNextSectionUnavailable(sequence.SequenceId, section.SectionId, yourOrganisationSequenceCompleted, applicationId, sequences)
                    });
                }

            }


            return result;
        }


        private bool IntroductionPageNextSectionUnavailable(int sequenceId, int sectionId, bool yourOrganisationSequenceCompleted, Guid applicationId, List<ApplicationSequence> sequences)
        {
            // Disable the other sequences if YourOrganisation sequence isn't complete
            if (sequenceId != RoatpWorkflowSequenceIds.YourOrganisation && !yourOrganisationSequenceCompleted)
            {
                return true;
            }

            // CriminalComplianceChecks has two intro pages...
            if (sequenceId == RoatpWorkflowSequenceIds.CriminalComplianceChecks)
            {
                var SecondCriminialIntroductionSectionId = 3;
                if (sectionId > SecondCriminialIntroductionSectionId)
                {
                    var statusOfSecondIntroductionPage = _roatpTaskListWorkflowService.SectionQuestionsStatus(applicationId, sequenceId, sectionId, sequences);
                    if (statusOfSecondIntroductionPage != TaskListSectionStatus.Completed)
                    {
                        return true;
                    }
                }
            }

            var statusOfIntroductionPage = _roatpTaskListWorkflowService.SectionQuestionsStatus(applicationId, sequenceId, 1, sequences);
            if (sequenceId > 1 && sectionId != 1 && statusOfIntroductionPage != TaskListSectionStatus.Completed)
                return true;

            return false;
        }



        private bool CheckYourOrganisationSequenceComplete(Guid applicationId, IEnumerable<ApplicationSequence> sequences, OrganisationVerificationStatus organisationVerificationStatus, ApplicationSequence yourOrganisationSequence)
        {
            var yourOrganisationSequenceComplete = true;
            foreach (var section in yourOrganisationSequence.Sections)
            {
                var sectionStatus = _roatpTaskListWorkflowService.SectionStatus(applicationId, RoatpWorkflowSequenceIds.YourOrganisation,
                    section.SectionId, sequences.ToList(), organisationVerificationStatus);
                if (sectionStatus != TaskListSectionStatus.Completed)
                {
                    yourOrganisationSequenceComplete = false;
                    break;
                }
            }

            return yourOrganisationSequenceComplete;
        }

        private bool ApplicationSequencesCompleted(Guid applicationId, IEnumerable<ApplicationSequence> applicationSequences, OrganisationVerificationStatus organisationVerificationStatus)
        {
            var nonFinishSequences = applicationSequences.Where(seq => seq.SequenceId != RoatpWorkflowSequenceIds.Finish).ToList();
            foreach (var sequence in nonFinishSequences)
            {
                foreach (var section in sequence.Sections)
                {
                    var sectionStatus = _roatpTaskListWorkflowService.SectionStatus(applicationId, sequence.SequenceId, section.SectionId, applicationSequences, organisationVerificationStatus);
                    if (sectionStatus != TaskListSectionStatus.NotRequired && sectionStatus != TaskListSectionStatus.Completed)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

    }

    public interface ITaskListService
    {
        Task<TaskList2ViewModel> GetTaskList2ViewModel(Guid applicationId, Guid userId);
    }
}
