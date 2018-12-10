using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply.GetSection;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class UpdateGradeHandler : IRequestHandler<UpdateGradeRequest>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IMediator _mediator;

        public UpdateGradeHandler(IApplyRepository applyRepository, IMediator mediator)
        {
            _applyRepository = applyRepository;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateGradeRequest request, CancellationToken cancellationToken)
        {
            var section = await _mediator.Send(new GetSectionRequest(request.ApplicationId, null, 1, 3));

            var qnADataObject = section.QnADataObject;

            qnADataObject.FinancialApplicationGrade = request.UpdatedGrade;
            qnADataObject.FinancialApplicationGrade.GradedDateTime = DateTime.UtcNow;            
            
            section.QnADataObject = qnADataObject;
            section.Status = SectionStatus.Graded;
            
            await _applyRepository.SaveSection(section);

            return Unit.Value;
        }
    }
}