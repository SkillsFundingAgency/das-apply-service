using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Services.Assessor
{
    public class AssessorSequenceService : IAssessorSequenceService
    {
        private static ReadOnlyCollection<int> AssessorSequenceNumbers => new List<int>
        {
            RoatpWorkflowSequenceIds.ProtectingYourApprentices,
            RoatpWorkflowSequenceIds.ReadinessToEngage,
            RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining,
            RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining,
            RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining
        }.AsReadOnly();

        private readonly IMediator _mediator;
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly IAssessorLookupService _lookupService;

        public AssessorSequenceService(IMediator mediator, IInternalQnaApiClient qnaApiClient, IAssessorLookupService assessorLookupService)
        {
            _mediator = mediator;
            _qnaApiClient = qnaApiClient;
            _lookupService = assessorLookupService;
        }

        public bool IsValidSequenceNumber(int sequenceNumber)
        {
            return AssessorSequenceNumbers.Contains(sequenceNumber);
        }

        public async Task<List<AssessorSequence>> GetSequences(Guid applicationId)
        {
            var sequences = new List<AssessorSequence>();

            var application = await _mediator.Send(new GetApplicationRequest(applicationId));
            var allQnaSections = await _qnaApiClient.GetAllApplicationSections(applicationId);

            if (allQnaSections != null && application?.ApplyData != null)
            {
                foreach (var sequenceNumber in AssessorSequenceNumbers)
                {
                    var applySequence = application.ApplyData.Sequences?.FirstOrDefault(seq => seq.SequenceNo == sequenceNumber);

                    var assessorSequence = GetSequence(sequenceNumber, allQnaSections, applySequence);

                    if (assessorSequence != null)
                    {
                        sequences.Add(assessorSequence);
                    }
                }
            }

            return sequences;
        }

        private AssessorSequence GetSequence(int sequenceNumber, IEnumerable<ApplicationSection> qnaSectionsForSequence, ApplySequence applySequence)
        {
            AssessorSequence sequence = null;

            if (IsValidSequenceNumber(sequenceNumber) && qnaSectionsForSequence != null)
            {
                var sectionsToExclude = GetWhatYouWillNeedSectionsForSequence(sequenceNumber);
                var qnaSections = qnaSectionsForSequence.Where(sec => sec.SequenceId == sequenceNumber && !sectionsToExclude.Contains(sec.SectionId));

                sequence = new AssessorSequence
                {
                    SequenceNumber = sequenceNumber,
                    SequenceTitle = _lookupService.GetTitleForSequence(sequenceNumber),
                    Sections = qnaSections.Select(sec =>
                                    {
                                        return new AssessorSection { SectionNumber = sec.SectionId, LinkTitle = sec.Title, Status = string.Empty };
                                    })
                                    .OrderBy(sec => sec.SectionNumber).ToList()
                };

                if (applySequence != null && applySequence.SequenceNo == sequenceNumber && applySequence.Sections != null)
                {
                    foreach (var section in sequence.Sections)
                    {
                        var applySection = applySequence.Sections.FirstOrDefault(sec => sec.SectionNo == section.SectionNumber);

                        if (applySequence.NotRequired || applySection?.NotRequired == true)
                        {
                            section.Status = AssessorReviewStatus.NotRequired;
                        }
                    }
                }
            }

            return sequence;
        }

        private static List<int> GetWhatYouWillNeedSectionsForSequence(int sequenceNumber)
        {
            var sections = new List<int>();

            switch (sequenceNumber)
            {
                case RoatpWorkflowSequenceIds.YourOrganisation:
                    sections.Add(RoatpWorkflowSectionIds.YourOrganisation.WhatYouWillNeed);
                    break;
                case RoatpWorkflowSequenceIds.FinancialEvidence:
                    sections.Add(RoatpWorkflowSectionIds.FinancialEvidence.WhatYouWillNeed);
                    break;
                case RoatpWorkflowSequenceIds.CriminalComplianceChecks:
                    sections.Add(RoatpWorkflowSectionIds.CriminalComplianceChecks.WhatYouWillNeed);
                    sections.Add(RoatpWorkflowSectionIds.CriminalComplianceChecks.WhatYouWillNeed_CheckOnWhosInControl);
                    break;
                case RoatpWorkflowSequenceIds.ProtectingYourApprentices:
                    sections.Add(RoatpWorkflowSectionIds.ProtectingYourApprentices.WhatYouWillNeed);
                    break;
                case RoatpWorkflowSequenceIds.ReadinessToEngage:
                    sections.Add(RoatpWorkflowSectionIds.ReadinessToEngage.WhatYouWillNeed);
                    break;
                case RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining:
                    sections.Add(RoatpWorkflowSectionIds.PlanningApprenticeshipTraining.WhatYouWillNeed);
                    break;
                case RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining:
                    sections.Add(RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.WhatYouWillNeed);
                    break;
                case RoatpWorkflowSequenceIds.EvaluatingApprenticeshipTraining:
                    sections.Add(RoatpWorkflowSectionIds.EvaluatingApprenticeshipTraining.WhatYouWillNeed);
                    break;
                default:
                    break;
            }

            return sections;
        }
    }
}
