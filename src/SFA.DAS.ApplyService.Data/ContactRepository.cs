using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
                await connection.ExecuteAsync(@"INSERT INTO Contacts (Email, GivenNames, FamilyName, SignInType, CreatedAt, CreatedBy, Status, IsApproved) 
                                                     VALUES (@email, @givenName, @familyName, @signInType, @createdAt, @email, 'New', 0)",
                    new { email, givenName, familyName, signInType, createdAt = DateTime.UtcNow });


                return await GetContact(email);
            }
        }

        public async Task<Contact> CreateContact(Contact contact, Guid? organisationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                contact.CreatedBy = contact.CreatedBy ?? contact.Email;
                await connection.ExecuteAsync(
                    @"INSERT INTO Contacts (Id, Email, GivenNames, FamilyName, SigninId, SignInType, ApplyOrganisationID, 
                                                Status, IsApproved, CreatedAt, CreatedBy) 
                                        VALUES (@Id, @Email, @GivenNames, @FamilyName,@SigninId, @SignInType,@organisationId,@Status,@IsApproved, @CreatedAt, @CreatedBy)",
                    new
                    {
                        contact.Id, contact.Email, contact.GivenNames, contact.FamilyName, contact.SigninId,
                        contact.SigninType, organisationId, contact.Status, contact.IsApproved, contact.CreatedAt,
                        contact.CreatedBy
                    });

                return await GetContactBySignInId(contact.Id);
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

        public async Task UpdateSignInId(Guid contactId, Guid? signInId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(
                    @"UPDATE Contacts SET SignInId = @signInId, UpdatedAt = GETUTCDATE(), UpdatedBy = 'dfeSignIn', Status = 'Live' WHERE Id = @contactId",
                    new {contactId, signInId});
            }
        }

        public async Task UpdateContactIdAndSignInId(Guid contactId, Guid signInId, string email, string updatedBy)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(
                    @"UPDATE Contacts SET Id = @contactId, SignInId = @signInId, UpdatedAt = GETUTCDATE(), UpdatedBy = @updatedBy, Status = 'Live' WHERE Email = @email",
                    new { contactId, signInId, email, updatedBy });
            }
        }

        public async Task UpdateApplyOrganisationId(Guid contactId, Guid applyOrganisationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(
                    @"UPDATE Contacts SET ApplyOrganisationId = @applyOrganisationId, UpdatedAt = GETUTCDATE(), UpdatedBy = 'System' WHERE Id = @contactId",
                    new { contactId, applyOrganisationId });
            }
        }

        public async Task<bool> UpdateIsApproved(Guid contactId, bool isApproved)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                int rowsAffected = await connection.ExecuteAsync(
                    @"UPDATE Contacts SET IsApproved = @isApproved, UpdatedAt = GETUTCDATE(), UpdatedBy = 'System' WHERE Id = @contactId",
                    new { contactId, isApproved });

                return rowsAffected > 0;
            }
        }

        public async Task<Contact> GetContact(Guid userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<Contact>(@"SELECT * FROM Contacts 
                                                    WHERE Id = @userId", new { userId })).FirstOrDefault();
            }
        }

        public async Task<Contact> GetContactByEmail(string email)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<Contact>(@"SELECT * FROM Contacts 
                                                    WHERE Email = @email", new { email })).FirstOrDefault();
            }
        }

        public async Task<List<Contact>> GetOrganisationContacts(Guid organisationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<Contact>(@"SELECT * FROM Contacts con
                                                    INNER JOIN Organisations org on con.ApplyOrganisationID = org.Id
                                                    WHERE con.ApplyOrganisationID = @organisationId", new { organisationId })).ToList();
            }
        }

        public async Task<bool> UpdateContactOrgId(Guid contactId, Guid orgId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                int rowsAffected = await connection.ExecuteAsync(
                    @"UPDATE Contacts SET ApplyOrganisationID = @orgId, UpdatedAt = GETUTCDATE(), UpdatedBy = 'System' WHERE Id = @contactId",
                    new { orgId, contactId });

                return rowsAffected > 0;
            }
        }

        public async Task<List<Contact>> GetUsersToMigrate()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<Contact>(@"SELECT * FROM Contacts 
                                                    WHERE SigninType = 'DfESignIn'")).ToList();
            }
        }

        public async Task UpdateMigratedContact(Guid contactId, Guid signInId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(
                    @"UPDATE Contacts SET SigninId = @signinId, SigninType = 'ASLogin', UpdatedAt = GETUTCDATE(), UpdatedBy = 'Migrate' WHERE Id = @contactId",
                    new { contactId, signInId });
            }
        }
    }
}