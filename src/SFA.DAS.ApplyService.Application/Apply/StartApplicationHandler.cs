using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Organisations;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply
{
    public class StartApplicationHandler : IRequestHandler<StartApplicationRequest>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IOrganisationRepository _organisationRepository;

        public StartApplicationHandler(IApplyRepository applyRepository, IOrganisationRepository organisationRepository)
        {
            _applyRepository = applyRepository;
            _organisationRepository = organisationRepository;
        }

        public async Task<Unit> Handle(StartApplicationRequest request, CancellationToken cancellationToken)
        {
            var assets = await _applyRepository.GetAssets();

            var org = await _organisationRepository.GetUserOrganisation(request.UserId);         

            var workflowId = await _applyRepository.GetLatestWorkflow("EPAO");
            var applicationId =
                await _applyRepository.CreateApplication("EPAO", org.Id, request.UserId, workflowId);

            var sections =
                await _applyRepository.CopyWorkflowToApplication(applicationId, workflowId, org.OrganisationType);

            foreach (var applicationSection in sections)
            {
                string QnADataJson = JsonConvert.SerializeObject(applicationSection.QnAData);
                foreach (var asset in assets)
                {
                    QnADataJson = QnADataJson.Replace(asset.Reference, HttpUtility.JavaScriptStringEncode(asset.Text));
                }

                applicationSection.QnAData = JsonConvert.DeserializeObject<QnAData>(QnADataJson);
            }

            var sequences = await _applyRepository.GetSequences(applicationId);
            
            DisableSequencesAndSectionsAsAppropriate(org, sequences, sections);

            try
            {
                await _applyRepository.UpdateSections(sections);
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }

            await _applyRepository.UpdateSequences(sequences);
            
            return Unit.Value;
        }

        private void DisableSequencesAndSectionsAsAppropriate(Organisation org, List<ApplicationSequence> sequences, List<ApplicationSection> sections)
        {
            // IF IsEPAOApproved = true;
            if (OrganisationIsOnEPAORegister(org))
            {
                RemoveSectionsOneAndTwo(sections);
                
                if (FinancialAssessmentNotRequired(org.OrganisationDetails.FHADetails))
                {
                    RemoveSectionThree(sections);
                    RemoveSequenceOne(sequences);
                }   
            }
        }

        private static bool OrganisationIsOnEPAORegister(Organisation org)
        {
            return org.RoEPAOApproved;
        }

        private void RemoveSequenceOne(List<ApplicationSequence> sequences)
        {
            var stage1 = sequences.Single(seq => seq.SequenceId == SequenceId.Stage1);
            stage1.IsActive = false;
            stage1.NotRequired = true;

            sequences.Single(seq => seq.SequenceId == SequenceId.Stage2).IsActive = true;
        }

        private void RemoveSectionThree(List<ApplicationSection> sections)
        {
            sections.Where(s => s.SectionId == 3).ToList().ForEach(s => s.NotRequired = true);
        }

        private void RemoveSectionsOneAndTwo(List<ApplicationSection> sections)
        {
            sections.Where(s => s.SectionId == 1 || s.SectionId == 2).ToList().ForEach(s => s.NotRequired = true);
        }

        private static bool FinancialAssessmentNotRequired(FHADetails financials)
        {
            return (financials.FinancialDueDate.HasValue && financials.FinancialDueDate.Value > DateTime.Today) 
                   || (financials.FinancialExempt.HasValue && financials.FinancialExempt.Value);
        }

    }
}