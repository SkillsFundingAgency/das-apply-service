using MediatR;
using SFA.DAS.ApplyService.Application.Email.Consts;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Organisations;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Return
{
    public class ReturnRequestHandler : IRequestHandler<ReturnRequest>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IEmailService _emailServiceObject;
        private readonly IContactRepository _contactRepository;
        private readonly IConfigurationService _configurationService;
        private readonly IOrganisationRepository _organisationRepository;
        private readonly ILogger<ReturnRequestHandler> _logger;

        private const string SERVICE_NAME = "Apprenticeship assessment service";
        private const string SERVICE_TEAM = "Apprenticeship assessment service team";

        public ReturnRequestHandler(IApplyRepository applyRepository, IContactRepository contactRepository, 
            IEmailService emailServiceObject, IConfigurationService configurationService, IOrganisationRepository organisationRepository, ILogger<ReturnRequestHandler> logger)
        {
            _applyRepository = applyRepository;
            _emailServiceObject = emailServiceObject;
            _contactRepository = contactRepository;
            _configurationService = configurationService;
            _organisationRepository = organisationRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(ReturnRequest request, CancellationToken cancellationToken)
        {
            if (request.RequestReturnType == "ReturnWithFeedback")
            {
                await _applyRepository.UpdateSequenceStatus(request.ApplicationId, request.SequenceId,
                    ApplicationSequenceStatus.FeedbackAdded, ApplicationStatus.FeedbackAdded);
            }
            else if (request.RequestReturnType == "Approve" || request.RequestReturnType == "ApproveWithFeedback")
            {
                await _applyRepository.UpdateSequenceStatus(request.ApplicationId, request.SequenceId,
                    ApplicationSequenceStatus.Approved, ApplicationStatus.InProgress);

                await _applyRepository.CloseSequence(request.ApplicationId, request.SequenceId);

                var sequences = await _applyRepository.GetSequences(request.ApplicationId);
                var nextSequence = sequences.FirstOrDefault(seq => (int)seq.SequenceNo == request.SequenceId + 1);

                if (nextSequence != null)
                {
                    await _applyRepository.OpenSequence(request.ApplicationId, (int)nextSequence.SequenceNo);
                }
                else
                {
                    // This is the last sequence, so approve the whole application
                    await _applyRepository.UpdateApplicationStatus(request.ApplicationId, ApplicationStatus.Approved);

                    if(await OrganisationIsNotYetOnTheRegister(request.ApplicationId))
                    {
                        _logger.LogInformation($"Organisation attached to ApplicationId: {request.ApplicationId} is not yet on the register. Check to see if any related applications need to be removed.");
                        await _applyRepository.DeleteRelatedApplications(request.ApplicationId);
                    }
                }
            }
            else
            {
                await _applyRepository.UpdateSequenceStatus(request.ApplicationId, request.SequenceId,
                    ApplicationSequenceStatus.Rejected, ApplicationStatus.Rejected);
            }

            await NotifyContact(request.ApplicationId, request.SequenceId);

            return Unit.Value;
        }

        private async Task<bool> OrganisationIsNotYetOnTheRegister(Guid applicationId)
        {
            var org = await _organisationRepository.GetOrganisationByApplicationId(applicationId);
            return !org.RoEPAOApproved;
        }

        private async Task NotifyContact(Guid applicationId, int sequenceId)
        {
            var application = await _applyRepository.GetApplication(applicationId);
            var standard = application.ApplicationData?.StandardName ?? string.Empty;
            var reference = application.ApplicationData?.ReferenceNumber ?? string.Empty;
            var config = await _configurationService.GetConfig();
            var assessorSignInPage = $"{config.AssessorServiceBaseUrl}/Account/SignIn";

            if (sequenceId == 1)
            {
                var lastInitSubmission = application.ApplicationData?.InitSubmissions.OrderByDescending(sub => sub.SubmittedAt).FirstOrDefault();

                if (lastInitSubmission != null)
                {
                    var contactToNotify = await _contactRepository.GetContact(lastInitSubmission.SubmittedBy);
                    await _emailServiceObject.SendEmailToContact(EmailTemplateName.APPLY_EPAO_UPDATE, contactToNotify, 
                        new { ServiceName = SERVICE_NAME, ServiceTeam = SERVICE_TEAM, Contact = contactToNotify.GivenNames, LoginLink = assessorSignInPage });
                }
            }
            else if (sequenceId == 2)
            {
                var lastStandardSubmission = application.ApplicationData?.StandardSubmissions.OrderByDescending(sub => sub.SubmittedAt).FirstOrDefault();

                if (lastStandardSubmission != null)
                {
                    var contactToNotify = await _contactRepository.GetContact(lastStandardSubmission.SubmittedBy);
                    await _emailServiceObject.SendEmailToContact(EmailTemplateName.APPLY_EPAO_RESPONSE, contactToNotify, 
                        new {ServiceName=SERVICE_NAME,ServiceTeam=SERVICE_TEAM, Contact= contactToNotify.GivenNames, standard, LoginLink = assessorSignInPage });
                }
            }
        }
    }
}