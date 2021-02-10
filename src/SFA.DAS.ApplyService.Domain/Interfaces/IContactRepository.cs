using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact> CreateContact(string email, string givenName, string familyName, string signInType);
        Task<Contact> GetContact(string email);
        Task<Contact> GetContactBySignInId(Guid signInId);
        Task UpdateSignInId(Guid contactId, Guid? signInId);
        Task UpdateApplyOrganisationId(Guid contactId, Guid applyOrganisationId);
        Task<bool> UpdateIsApproved(Guid contactId, bool isApproved);

        Task<List<Contact>> GetOrganisationContacts(Guid organisationId);

        Task<Contact> GetContact(Guid userId);
        Task<List<Contact>> GetUsersToMigrate();
        Task UpdateMigratedContact(Guid contactId, Guid signInId);
        Task<Contact> GetContactByEmail(string email);
        Task UpdateContactIdAndSignInId(Guid contactId, Guid signInId, string email, string updatedBy);
        Task<bool> UpdateContactOrgId(Guid contactId, Guid orgId);
        Task<Contact> CreateContact(Contact contact, Guid? organisationId);
        Task RemoveContactFromOrganisation(Guid contactId);
    }
}