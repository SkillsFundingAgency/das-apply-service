﻿using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Evaluate
{
    public class EvaluateHandler : IRequestHandler<EvaluateRequest>
    {
        private readonly IApplyRepository _applyRepository;

        public EvaluateHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<Unit> Handle(EvaluateRequest request, CancellationToken cancellationToken)
        {
            var section = await _applyRepository.GetSection(request.ApplicationId, request.SequenceId, request.SectionId, null);

            if (request.IsSectionComplete)
            {
                section.Status = ApplicationSectionStatus.Evaluated;
            }
            else if (request.SequenceId == 1 && request.SectionId == 3)
            {
                section.Status = ApplicationSectionStatus.Graded;
            }
            else
            {
                section.Status = ApplicationSectionStatus.InProgress;
            }

            await _applyRepository.UpdateSections(new List<ApplicationSection> { section });

            return Unit.Value;
        }
    }
}
