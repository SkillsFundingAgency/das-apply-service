using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Organisations.GetOrganisation
{
    public class GetOrganisationByUkprnHandler : IRequestHandler<GetOrganisationByUkprnRequest, Organisation>
    {
        private readonly IOrganisationRepository _organisationRepository;

        public GetOrganisationByUkprnHandler(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }

        public async Task<Organisation> Handle(GetOrganisationByUkprnRequest request, CancellationToken cancellationToken)
        {
            return await _organisationRepository.GetOrganisationByUkprn(request.Ukprn);
        }
    }
}
