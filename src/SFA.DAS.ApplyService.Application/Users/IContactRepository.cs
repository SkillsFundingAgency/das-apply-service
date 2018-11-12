using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Users
{
    public interface IContactRepository
    {
        Task<Contact> CreateContact(string email, string givenName, string familyName, string signInType);
        Task<Contact> GetContact(string email);
        Task<Contact> GetContactBySignInId(Guid signInId);
        Task UpdateSignInId(Guid contactId, Guid signInId);
        Task UpdateApplyOrganisationId(Guid contactId, Guid applyOrganisationId);
        Task<bool> UpdateIsApproved(Guid contactId, bool isApproved);
    }
}