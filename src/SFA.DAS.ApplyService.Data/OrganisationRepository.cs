using Dapper;
using SFA.DAS.ApplyService.Data.DapperTypeHandlers;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Infrastructure.Database;

namespace SFA.DAS.ApplyService.Data
{
    public class OrganisationRepository : IOrganisationRepository
    {
        private readonly IDbConnectionHelper _dbConnectionHelper;

        public OrganisationRepository(IDbConnectionHelper dbConnectionHelper)
        {
            _dbConnectionHelper = dbConnectionHelper;
            SqlMapper.AddTypeHandler(typeof(OrganisationDetails), new OrganisationDetailsHandler());
        }

        public async Task<Guid> CreateOrganisation(Organisation organisation, Guid userId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var organisationId = await connection.QuerySingleAsync<Guid>(
                    "INSERT INTO [Organisations] ([Id],[Name],[TradingName],[OrganisationType],[OrganisationUKPRN], " +
                    "[CompanyRegistrationNumber],[CharityRegistrationNumber],[OrganisationDetails],[Status],[CreatedAt],[CreatedBy],[RoATPApproved]) " +
                    "OUTPUT INSERTED.[Id] " +
                    "VALUES (NEWID(), @Name, @TradingName, REPLACE(@OrganisationType, ' ', ''), @OrganisationUkprn, @CompanyNumber, @CharityNumber, @OrganisationDetails, 'New', GETUTCDATE(), @CreatedBy, @RoATPApproved)",
                    new { organisation.Name, organisation.OrganisationDetails.TradingName, organisation.OrganisationType, organisation.OrganisationUkprn, 
                        organisation.OrganisationDetails.CompanyNumber, organisation.OrganisationDetails.CharityNumber,
                        organisation.OrganisationDetails, organisation.CreatedBy, organisation.RoATPApproved });

                    connection.Execute(
                                "UPDATE [Contacts] " +
                                "SET ApplyOrganisationID = @Id " +
                                "WHERE Id = @userId",
                                new { Id = organisationId, userId });
                return organisationId;
            }
        }

        public async Task<Guid> CreateOrganisation(Organisation organisation)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var organisationId = await connection.QuerySingleAsync<Guid>(
                    "INSERT INTO [Organisations] ([Id],[Name],[TradingName],[OrganisationType],[OrganisationUKPRN], " +
                    "[CompanyRegistrationNumber],[CharityRegistrationNumber],[OrganisationDetails],[Status],[CreatedAt],[CreatedBy],[RoATPApproved]) " +
                    "OUTPUT INSERTED.[Id] " +
                    "VALUES (NEWID(), @Name, @TradingName, REPLACE(@OrganisationType, ' ', ''), @OrganisationUkprn, @CompanyNumber, @CharityNumber, @OrganisationDetails, 'New', GETUTCDATE(), @CreatedBy, @RoATPApproved)",
                    new { organisation.Name, organisation.OrganisationDetails.TradingName, organisation.OrganisationType, organisation.OrganisationUkprn,
                        organisation.OrganisationDetails.CompanyNumber,organisation.OrganisationDetails.CharityNumber,
                        organisation.OrganisationDetails, organisation.CreatedBy, organisation.RoATPApproved });
                
                return organisationId;
            }
        }

        public async Task<Organisation> GetOrganisationByApplicationId(Guid applicationId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var sql =
                    @"SELECT Organisations.*
                    FROM Organisations 
                    INNER JOIN Apply ON Apply.OrganisationId = Organisations.Id
                    WHERE Apply.ApplicationId = @applicationId";

                var org = await connection.QuerySingleAsync<Organisation>(sql, new { applicationId });
                
                return org;
            }
        }

        public async Task UpdateOrganisation(Organisation organisation)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                await UpdateOrganisation(organisation, connection);
            }
        }

        private Task UpdateOrganisation(Organisation organisation, IDbConnection connection)
        {
            return connection.ExecuteAsync(
                "UPDATE [Organisations] " +
                "SET [UpdatedAt] = GETUTCDATE(), [UpdatedBy] = @UpdatedBy, [Name] = @Name, " + "[TradingName] = @TradingName," +
                "[OrganisationType] = @OrganisationType, [OrganisationUKPRN] = @OrganisationUkprn, " +
                "[CompanyRegistrationNumber]= @CompanyNumber, [CharityRegistrationNumber] =  @CharityNumber," +
                "[OrganisationDetails] = @OrganisationDetails, [RoATPApproved] = @RoATPApproved " +
                "WHERE [Id] = @Id",
                new {organisation.Id, organisation.Name, organisation.OrganisationDetails.TradingName, organisation.OrganisationType, organisation.OrganisationUkprn,
                    organisation.OrganisationDetails.CompanyNumber,organisation.OrganisationDetails.CharityNumber,
                    organisation.OrganisationDetails, organisation.UpdatedBy, organisation.RoATPApproved});
        }

        public async Task UpdateOrganisation(Organisation organisation, Guid userId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                await UpdateOrganisation(organisation, connection);

                    connection.Execute(
                                "UPDATE [Contacts] " +
                                "SET ApplyOrganisationID = @Id " +
                                "WHERE Id = @userId",
                                new { organisation.Id, userId });
            }
        }

        public async Task UpdateOrganisation(Guid organisationId, Guid userId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                connection.Execute(
                    "UPDATE [Organisations] " +
                    "SET CreatedBy = @userId " +
                    "WHERE [Id] = @organisationId",
                    new { userId, organisationId });
            }
        }

        public async Task<Organisation> GetOrganisation(Guid organisationId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return await connection.QueryFirstAsync<Organisation>("SELECT * FROM Organisations WHERE Id = @organisationId", new {organisationId});
            }
        }

        public async Task<Organisation> GetOrganisationByName(string name)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var sql =
                    "SELECT * " +
                    "FROM [Organisations] " +
                    "WHERE Name = @name";

                var orgs = await connection.QueryAsync<Organisation>(sql, new { name });
                var org = orgs.FirstOrDefault();
                return org;
            }
        }

        public async Task<Organisation> GetOrganisationByContactEmail(string email)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var sql =
                    "SELECT org.* " +
                    "FROM [Organisations] org " +
                    "INNER JOIN [Contacts] con on org.Id = con.ApplyOrganisationId " +
                    "WHERE con.Email = @email";

                var orgs = await connection.QueryAsync<Organisation>(sql, new { email });
                var org = orgs.FirstOrDefault();
                return org;
            }
        }
        
        public async Task<Organisation> GetUserOrganisation(Guid userId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return await connection.QuerySingleAsync<Organisation>(@"SELECT o.* 
                                                            FROM Contacts c 
                                                            INNER JOIN Organisations o ON o.Id = c.ApplyOrganisationID
                                                            WHERE c.Id = @UserId", new {userId});
            }
        }

        public async Task<Organisation> GetOrganisationByUserId(Guid userId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var sql =
                    "SELECT org.* " +
                    "FROM [Organisations] org " +
                    "INNER JOIN [Contacts] con on org.Id = con.ApplyOrganisationId " +
                    "WHERE con.Id = @userId";

                var org = await connection.QueryFirstOrDefaultAsync<Organisation>(sql, new { userId });
                return org;
            }
        }

        public async Task<Organisation> GetOrganisationByUkprn(string ukprn)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var sql =
                    "SELECT * " +
                    "FROM Organisations " +
                    "WHERE OrganisationUKPRN = @ukprn";

                var org = await connection.QueryFirstOrDefaultAsync<Organisation>(sql, new { ukprn });
                return org;
            }
        }
    }
}