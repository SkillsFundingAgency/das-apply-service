using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply.GetSection;
using SFA.DAS.ApplyService.Application.Organisations;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class UpdateGradeHandler : IRequestHandler<UpdateGradeRequest>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IMediator _mediator;
        private readonly IOrganisationRepository _organisationRepository;

        public UpdateGradeHandler(IApplyRepository applyRepository, IMediator mediator, IOrganisationRepository organisationRepository)
        {
            _applyRepository = applyRepository;
            _mediator = mediator;
            _organisationRepository = organisationRepository;
        }

        public async Task<Unit> Handle(UpdateGradeRequest request, CancellationToken cancellationToken)
        {
            var section = await _mediator.Send(new GetSectionRequest(request.ApplicationId, null, 1, 3));

            section.QnAData.FinancialApplicationGrade = request.UpdatedGrade;
            section.QnAData.FinancialApplicationGrade.GradedDateTime = DateTime.UtcNow;                    
            section.Status = ApplicationSectionStatus.Graded;
            
            await _applyRepository.SaveSection(section);

            if (request.UpdatedGrade.FinancialDueDate.HasValue)
            {
                var org = await _organisationRepository.GetOrganisationByApplicationId(request.ApplicationId);
                org.OrganisationDetails.FinancialDueDate = request.UpdatedGrade.FinancialDueDate.Value;

                await _organisationRepository.UpdateOrganisation(org, Guid.NewGuid());
            }
                        
            return Unit.Value;
        }
    }
}