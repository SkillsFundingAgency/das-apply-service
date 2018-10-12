using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Users
{
    public interface IContactRepository
    {
        Task<Contact> CreateContact(string email, string givenName, string familyName);
        Task<Contact> GetContact(string email);
    }
}