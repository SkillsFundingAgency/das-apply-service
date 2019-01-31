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
    public class UpdateGradeHandler : IRequestHandler<UpdateGradeRequest, Organisation>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IMediator _mediator;
        private readonly IOrganisationRepository _organisationRepository;

        public UpdateGradeHandler(IApplyRepository applyRepository, IMediator mediator,
            IOrganisationRepository organisationRepository)
        {
            _applyRepository = applyRepository;
            _mediator = mediator;
            _organisationRepository = organisationRepository;
        }

        public async Task<Organisation> Handle(UpdateGradeRequest request, CancellationToken cancellationToken)
        {
            var section = await _mediator.Send(new GetSectionRequest(request.ApplicationId, null, 1, 3));

            section.QnAData.FinancialApplicationGrade = request.UpdatedGrade;
            section.QnAData.FinancialApplicationGrade.GradedDateTime = DateTime.UtcNow;
            section.Status = ApplicationSectionStatus.Graded;

            await _applyRepository.SaveSection(section);

            var org = await UpdateApplyOrganisation(request);

            return org;
        }

        private async Task<Organisation> UpdateApplyOrganisation(UpdateGradeRequest request)
        {
            var org = await _organisationRepository.GetOrganisationByApplicationId(request.ApplicationId);
            org.OrganisationDetails.FHADetails = new FHADetails()
            {
                FinancialDueDate = request.UpdatedGrade.FinancialDueDate,
                FinancialExempt = request.UpdatedGrade.SelectedGrade == FinancialApplicationSelectedGrade.Exempt
            };
            
            await _organisationRepository.UpdateOrganisation(org);

            return org;
        }
    }
}