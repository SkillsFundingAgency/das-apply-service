using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Data
{
    public class ContactRepository : IContactRepository
    {
        private readonly IApplyConfig _config;
        
        public ContactRepository(IConfigurationService configurationService)
        {
            _config = configurationService.GetConfig().Result;
        }
        
        public async Task<Contact> CreateContact(string email, string givenName, string familyName, string signInType)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"INSERT INTO Contacts (Email, GivenNames, FamilyName, SignInType, CreatedAt, CreatedBy, Status) 
                                                     VALUES (@email, @givenName, @familyName, @signInType, @createdAt, @email, 'Live')",
                    new {email, givenName, familyName, signInType, createdAt = DateTime.UtcNow});

                return await GetContact(email);   
            }
        }

        public async Task<Contact> GetContact(string email)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<Contact>("SELECT * FROM Contacts WHERE Email = @email",
                    new {email});
            }
        }

        public async Task<Contact> GetContactBySignInId(Guid signInId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<Contact>("SELECT * FROM Contacts WHERE SignInId = @signInId",
                    new {signInId});
            }
        }
    }
}