using System;
using System.Collections.Generic;
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
            
            await _applyRepository.UpdateSections(sections);
            await _applyRepository.UpdateSequences(sequences);
            
            return Unit.Value;
        }

        private void DisableSequencesAndSectionsAsAppropriate(Organisation org, List<ApplicationSequence> sequences, List<ApplicationSection> sections)
        {
            // IF IsEPAOApproved = true;
            if (org.RoEPAOApproved)
            {
                var financials = org.OrganisationDetails.FHADetails;

                if (financials.FinancialExempt.HasValue)
                {
                    if (OrgIsExemptFromFinancialAssessment(financials))
                    {
                        RemoveFinancialAssessmentSection(sections);
                    }
                    else
                    {
                        if (FinancialAssessmentIsDue(financials))
                        {
                            // remove all sequence 1 sections but 3
                        }
                        else
                        {
                            // remove all sequence 1 sections
                            // remove sequence
                        }
                    }    
                }
                
                
                if (financials.FinancialDueDate.HasValue && financials.FinancialDueDate.Value > DateTime.Today 
                 && financials.FinancialExempt.HasValue && financials.FinancialExempt.Value == false)
                {
                    //if !exempt && due date > today,
                    //set Sections 1, 2, 3 as NotRequired, Sequence 1 as IsActive false & status = NotRequired, Sequence 2 as IsActive true;
                    sections.Where(s => s.SectionId != 4).ToList().ForEach(s => s.Status = ApplicationSectionStatus.NotRequired);
                    var stage1 = sequences.Single(seq => seq.SequenceId == SequenceId.Stage1);
                    stage1.IsActive = false;
                    stage1.Status = ApplicationSequenceStatus.NotRequired;

                    sequences.Single(seq => seq.SequenceId == SequenceId.Stage2).IsActive = true;
                }
                
                // check org Financial Due Date
                // if exempt, set Section 3 to NotRequired
                // if !exempt && due date > today, set Sections 1, 2, 3 as NotRequired, Sequence 1 as IsActive false & status = NotRequired, Sequence 2 as IsActive true;   
            }
        }

        private static bool FinancialAssessmentIsDue(FHADetails financials)
        {
            return financials.FinancialDueDate.HasValue && financials.FinancialDueDate.Value < DateTime.Today;
        }

        private static bool OrgIsExemptFromFinancialAssessment(FHADetails financials)
        {
            return financials.FinancialExempt.Value;
        }
    }
}