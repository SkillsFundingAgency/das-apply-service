using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Organisations;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public class StartApplicationHandler : IRequestHandler<StartApplicationRequest, StartApplicationResponse>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IOrganisationRepository _organisationRepository;
        public StartApplicationHandler(IApplyRepository applyRepository, IOrganisationRepository organisationRepository)
        {
            _applyRepository = applyRepository;
            _organisationRepository = organisationRepository;
        }

        public async Task<StartApplicationResponse> Handle(StartApplicationRequest request, CancellationToken cancellationToken)
        {
            var assets = await _applyRepository.GetAssets();

            var org = await _organisationRepository.GetUserOrganisation(request.UserId);

            var workflowId = await _applyRepository.GetLatestWorkflow(request.ApplicationType);
            var applicationId =
                await _applyRepository.CreateApplication(request.ApplicationId, request.ApplicationType, org.Id, request.UserId, workflowId);

            return new StartApplicationResponse() {ApplicationId = applicationId};
        }

        private void DisableSequencesAndSectionsAsAppropriate(Organisation org, List<ApplicationSequence> sequences, List<ApplicationSection> sections)
        {
            bool isEpao = IsOrganisationOnEPAORegister(org);
            if (isEpao)
            {
                RemoveSectionsOneAndTwo(sections);
            }

            bool isFinancialExempt = IsFinancialExempt(org.OrganisationDetails?.FHADetails);
            if (isFinancialExempt)
            {
                RemoveSectionThree(sections);
            }

            if (isEpao && isFinancialExempt)
            {
                RemoveSequenceOne(sequences);
            }
        }

        private static bool IsOrganisationOnEPAORegister(Organisation org)
        {
            if (org is null) return false;

            return org.RoEPAOApproved;
        }

        private static bool IsFinancialExempt(FHADetails financials)
        {
            if (financials is null) return false;

            bool financialExempt = financials.FinancialExempt ?? false;
            bool financialIsNotDue = (financials.FinancialDueDate?.Date ?? DateTime.MinValue) > DateTime.Today;

            return financialExempt || financialIsNotDue;
        }

        private void RemoveSequenceOne(List<ApplicationSequence> sequences)
        {
            var stage1 = sequences.Single(seq => seq.SequenceNo == SequenceId.Stage1);
            stage1.IsActive = false;
            stage1.NotRequired = true;
            stage1.Status = ApplicationSequenceStatus.Approved;

            SetSubmissionData(stage1.ApplicationId, stage1.SequenceNo).GetAwaiter().GetResult();

            sequences.Single(seq => seq.SequenceNo == SequenceId.Stage2).IsActive = true;
        }

        private async Task SetSubmissionData(Guid applicationId, int sequenceId)
        {
            var application = await _applyRepository.GetApplication(applicationId);

            if (application != null)
            {
                if(application.ApplicationData == null)
                {
                    application.ApplicationData = new ApplicationData();
                }

                if (sequenceId == SequenceId.Stage1)
                {
                    application.ApplicationData.LatestInitSubmissionDate = DateTime.UtcNow;
                    application.ApplicationData.InitSubmissionClosedDate = DateTime.UtcNow;
                }
                else if (sequenceId == SequenceId.Stage2)
                {
                    application.ApplicationData.LatestStandardSubmissionDate = DateTime.UtcNow;
                    application.ApplicationData.StandardSubmissionClosedDate = DateTime.UtcNow;
                }

                await _applyRepository.UpdateApplicationData(application.Id, application.ApplicationData);
            }
        }

        private void RemoveSectionThree(List<ApplicationSection> sections)
        {
            foreach(var sec in sections.Where(s => s.SectionNo == 3))
            {
                sec.NotRequired = true;
                sec.Status = ApplicationSectionStatus.Evaluated;

                if (sec.QnAData.FinancialApplicationGrade is null)
                {
                    sec.QnAData.FinancialApplicationGrade = new FinancialApplicationGrade();
                }

                sec.QnAData.FinancialApplicationGrade.SelectedGrade = FinancialApplicationSelectedGrade.Exempt;
                sec.QnAData.FinancialApplicationGrade.GradedDateTime = DateTime.UtcNow;
            }
        }

        private void RemoveSectionsOneAndTwo(List<ApplicationSection> sections)
        {
            foreach (var sec in sections.Where(s => s.SectionNo == 1 || s.SectionNo == 2))
            {
                sec.NotRequired = true;
                sec.Status = ApplicationSectionStatus.Evaluated;
            }
        }
    }
}