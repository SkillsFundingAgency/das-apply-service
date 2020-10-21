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
            var result = new TaskList2ViewModel();

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
            var sequences = _roatpTaskListWorkflowService.GetApplicationSequences(applicationId);

            var yourOrganisationSequence = sequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);

            var yourOrganisationSequenceCompleted = CheckYourOrganisationSequenceComplete(applicationId, sequences, organisationVerificationStatus, yourOrganisationSequence);
            var applicationSequencesCompleted = ApplicationSequencesCompleted(applicationId, sequences, organisationVerificationStatus);





            return result;
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
