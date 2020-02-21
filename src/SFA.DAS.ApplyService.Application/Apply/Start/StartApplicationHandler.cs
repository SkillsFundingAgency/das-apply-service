using MediatR;
using SFA.DAS.ApplyService.Application.Organisations;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.Start
{
    public class StartApplicationHandler : IRequestHandler<StartApplicationRequest, Guid>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IContactRepository _contactRepository;

        public StartApplicationHandler(IApplyRepository applyRepository, IOrganisationRepository organisationRepository, IContactRepository contactRepository)
        {
            _applyRepository = applyRepository;
            _organisationRepository = organisationRepository;
            _contactRepository = contactRepository;
        }

        public async Task<Guid> Handle(StartApplicationRequest request, CancellationToken cancellationToken)
        {
            var applicationId = Guid.Empty;

            var creatingContact = await _contactRepository.GetContact(request.CreatingContactId);
            var org = await _organisationRepository.GetOrganisationByUserId(request.CreatingContactId);

            if (org != null && creatingContact != null)
            {
                var sequences = request.ApplySequences;
                //MakeSequencesSequentialAsAppropriate(request.ProviderRoute, sequences);
                //DisableSequencesAndSectionsAsAppropriate(request.ProviderRoute, sequences);

                var applyData = new ApplyData
                {
                    Sequences = sequences,
                    ApplyDetails = new ApplyDetails
                    {
                        ReferenceNumber = await _applyRepository.GetNextRoatpApplicationReference(),
                        UKPRN = org.OrganisationUkprn?.ToString(),
                        OrganisationName = org.Name,
                        TradingName = org.OrganisationDetails?.TradingName,
                        ProviderRoute = request.ProviderRoute,
                        ProviderRouteName = request.ProviderRouteName
                    }
                };
                foreach (var sequence in applyData.Sequences)
                {
                    sequence.IsActive = true;
                }

                applicationId = await _applyRepository.StartApplication(request.ApplicationId, applyData, org.Id, creatingContact.Id);
            }

            return applicationId;
        }
    }
}