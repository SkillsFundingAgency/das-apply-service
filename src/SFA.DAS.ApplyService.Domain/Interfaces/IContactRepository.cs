using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Domain.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact> CreateContact(string email, string givenName, string familyName, string govUkIdentifier = null);
        Task<Contact> GetContact(Guid userId);
        Task<Contact> GetContactByEmail(string email);
        Task<Contact> GetContactBySignInId(Guid signInId);
        Task UpdateSignInId(Guid contactId, Guid? signInId, string govIdentifier);
    }
}