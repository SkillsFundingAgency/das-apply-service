using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Organisations;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Users.CreateNewContact
{
    public class CreateNewContactHandler : IRequestHandler<CreateNewContactRequest>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IOrganisationRepository _organisationRepository;

        public CreateNewContactHandler(IContactRepository contactRepository, IOrganisationRepository organisationRepository)
        {
            _contactRepository = contactRepository;
            _organisationRepository = organisationRepository;
        }
        
        public async Task<Unit> Handle(CreateNewContactRequest request, CancellationToken cancellationToken)
        {
            var existingOrganisation = await _organisationRepository.GetOrganisation(request.OrganisationId);

            if (existingOrganisation != null)
            {
                await _contactRepository.CreateContact(new Contact
                {
                    Id = request.Id,
                    CreatedAt = DateTime.UtcNow, 
                    CreatedBy = "Invitation", 
                    Email = request.Email,
                    FamilyName = request.FamilyName, 
                    GivenNames = request.GivenNames,
                    IsApproved = true,
                    SigninType = request.SignInType, 
                    Status = request.Status
                }, request.OrganisationId);
            }
            
            return Unit.Value;
        }
    }
}