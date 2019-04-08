using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Organisations;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Users.UpdateContactOrgId
{
   
    public class MigrateContactOrganisationHandler : IRequestHandler<MigrateContactOrganisationRequest>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IOrganisationRepository _organisationRepository;

        public MigrateContactOrganisationHandler(IContactRepository contactRepository,
            IOrganisationRepository organisationRepository)
        {
            _contactRepository = contactRepository;
            _organisationRepository = organisationRepository;
        }

        public async Task<Unit> Handle(MigrateContactOrganisationRequest request, CancellationToken cancellationToken)
        {
            var newOrganisationCreated = false;
            var requestContact = request.Contact;
            var requestOrganisation = request.Organisation;
            if (requestContact != null )
            {
                var organisationId = Guid.Empty;
                if (requestOrganisation != null)
                {
                    var organisation = await _organisationRepository.GetOrganisationByName(requestOrganisation.Name) ??
                                       (await _organisationRepository.GetOrganisationByName(requestOrganisation.OrganisationDetails.LegalName) ??
                                        await _organisationRepository.GetOrganisationByName(requestOrganisation.OrganisationDetails.ProviderName));

                    if (organisation != null)
                    {
                        //Update organisation details
                        organisation.OrganisationDetails = requestOrganisation.OrganisationDetails;
                        organisation = await _organisationRepository.UpdateOrganisation(organisation);
                        organisationId = organisation.Id;
                    }
                    else
                    {
                        //Create new organisation
                        organisation = await _organisationRepository.CreateOrganisation(requestOrganisation);
                        organisationId = organisation.Id;
                        newOrganisationCreated = true;
                    }
                }
                var signinId = requestContact.SigninId ?? Guid.Empty;
                var contact = await _contactRepository.GetContactBySignInId(signinId);
                if (contact != null)
                {
                    //Update organisation association
                    if(contact.ApplyOrganisationId == null && organisationId != Guid.Empty)
                        await _contactRepository.UpdateApplyOrganisationId(requestContact.Id, organisationId);
                }
                else
                {
                    //Create new contact 
                    Contact newContact = null;
                    if (organisationId == Guid.Empty)
                        newContact = await _contactRepository.CreateContact(requestContact,null);
                    else
                        newContact = await _contactRepository.CreateContact(requestContact, organisationId);

                    if (newOrganisationCreated)
                        await _organisationRepository.UpdateOrganisation(organisationId, newContact.Id);
                }
            }

            return Unit.Value;
        }


    }

}
