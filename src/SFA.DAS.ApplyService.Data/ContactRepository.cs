using System;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Infrastructure.Database;

namespace SFA.DAS.ApplyService.Data
{
    public class ContactRepository : IContactRepository
    {
        private readonly IDbConnectionHelper _dbConnectionHelper;

        public ContactRepository(IDbConnectionHelper dbConnectionHelper)
        {
            _dbConnectionHelper = dbConnectionHelper;
        }
        
        public async Task<Contact> CreateContact(string email, string givenName, string familyName, string govUkIdentifier = null, Guid? userId = null)
        {
            var loginType = string.IsNullOrEmpty(govUkIdentifier) ? "ASLogin" : "GovLogin";
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                await connection.ExecuteAsync(@"INSERT INTO Contacts (Id, Email, GivenNames, FamilyName, SignInType, CreatedAt, CreatedBy, Status, GovUkidentifier) 
                                                     VALUES (@userId, @email, @givenName, @familyName, @loginType, @createdAt, @email, 'New', @GovUkidentifier)",
                    new {userId, email, givenName, familyName,loginType, createdAt = DateTime.UtcNow, govUkIdentifier });

                return await GetContactByEmail(email);
            }
        }

        public async Task<Contact> GetContact(Guid userId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Contact>(@"SELECT * FROM Contacts WHERE Id = @userId",
                    new { userId });
            }
        }

        public async Task<Contact> GetContactByEmail(string email)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Contact>("SELECT * FROM Contacts WHERE Email = @email",
                    new {email});
            }
        }

        public async Task<Contact> GetContactBySignInId(Guid signInId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Contact>("SELECT * FROM Contacts WHERE SignInId = @signInId",
                    new {signInId});
            }
        }

        public async Task UpdateSignInId(Guid contactId, Guid? signInId, string govIdentifier)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                await connection.ExecuteAsync(
                    @"UPDATE Contacts SET SignInId = @signInId, UpdatedAt = GETUTCDATE(), GovUkidentifier=@govIdentifier, UpdatedBy = 'ASLogin', Status = 'Live' WHERE Id = @contactId",
                    new {contactId, signInId, govIdentifier});
            }
        }
    }
}