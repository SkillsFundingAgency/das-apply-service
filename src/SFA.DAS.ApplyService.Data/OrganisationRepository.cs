using Dapper;
using SFA.DAS.ApplyService.Application.Organisations;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Data.DapperTypeHandlers;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Data
{
    public class OrganisationRepository : IOrganisationRepository
    {
        private readonly IApplyConfig _config;

        public OrganisationRepository(IConfigurationService configurationService)
        {
            _config = configurationService.GetConfig().Result;
            SqlMapper.AddTypeHandler(typeof(OrganisationDetails), new OrganisationDetailsHandler());
        }

        public async Task<Guid> CreateOrganisation(Organisation organisation, Guid userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                var organisationId = await connection.QuerySingleAsync<Guid>(
                    "INSERT INTO [Organisations] ([Id],[Name],[OrganisationType],[OrganisationUKPRN], " +
                    "[OrganisationDetails],[Status],[CreatedAt],[CreatedBy],[RoEPAOApproved],[RoATPApproved]) " +
                    "OUTPUT INSERTED.[Id] " +
                    "VALUES (NEWID(), @Name, REPLACE(@OrganisationType, ' ', ''), @OrganisationUkprn, @OrganisationDetails, 'New', GETUTCDATE(), @CreatedBy, @RoEPAOApproved, @RoATPApproved)",
                    new { organisation.Name, organisation.OrganisationType, organisation.OrganisationUkprn, organisation.OrganisationDetails, organisation.CreatedBy, organisation.RoEPAOApproved, organisation.RoATPApproved });

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
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                var organisationId = await connection.QuerySingleAsync<Guid>(
                    "INSERT INTO [Organisations] ([Id],[Name],[OrganisationType],[OrganisationUKPRN], " +
                    "[OrganisationDetails],[Status],[CreatedAt],[CreatedBy],[RoEPAOApproved],[RoATPApproved]) " +
                    "OUTPUT INSERTED.[Id] " +
                    "VALUES (NEWID(), @Name, REPLACE(@OrganisationType, ' ', ''), @OrganisationUkprn, @OrganisationDetails, 'New', GETUTCDATE(), @CreatedBy, @RoEPAOApproved, @RoATPApproved)",
                    new { organisation.Name, organisation.OrganisationType, organisation.OrganisationUkprn, organisation.OrganisationDetails, organisation.CreatedBy, organisation.RoEPAOApproved, organisation.RoATPApproved });
                
                return organisationId;
            }
        }



        public async Task<Organisation> GetOrganisationByApplicationId(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                var sql =
                    @"SELECT Organisations.*
                    FROM Organisations 
                    INNER JOIN Applications ON Applications.ApplyingOrganisationId = Organisations.Id
                    WHERE Applications.Id = @applicationId";

                var org = await connection.QuerySingleAsync<Organisation>(sql, new { applicationId });
                
                return org;
            }
        }

        public async Task UpdateOrganisation(Organisation organisation)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                await UpdateOrganisation(organisation, connection);
            }
        }

        private Task UpdateOrganisation(Organisation organisation, SqlConnection connection)
        {
            return connection.ExecuteAsync(
                "UPDATE [Organisations] " +
                "SET [UpdatedAt] = GETUTCDATE(), [UpdatedBy] = @UpdatedBy, [Name] = @Name, " +
                "[OrganisationType] = @OrganisationType, [OrganisationUKPRN] = @OrganisationUkprn, " +
                "[OrganisationDetails] = @OrganisationDetails, [RoEPAOApproved] = @RoEPAOApproved, [RoATPApproved] = @RoATPApproved " +
                "WHERE [Id] = @Id",
                new {organisation.Id, organisation.Name, organisation.OrganisationType, organisation.OrganisationUkprn, organisation.OrganisationDetails, organisation.UpdatedBy, organisation.RoEPAOApproved, organisation.RoATPApproved});
        }

        public async Task UpdateOrganisation(Organisation organisation, Guid userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

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
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();
                
                connection.Execute(
                    "UPDATE [Organisations] " +
                    "SET CreatedBy = @userId " +
                    "WHERE [Id] = @organisationId",
                    new { userId, organisationId });
            }
        }

        public async Task<Organisation> GetOrganisation(Guid organisationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                return await connection.QueryFirstAsync<Organisation>("SELECT * FROM Organisations WHERE Id = @organisationId", new {organisationId});
            }
        }

        public async Task<Organisation> GetOrganisationByName(string name)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

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
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

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
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return await connection.QuerySingleAsync<Organisation>(@"SELECT o.* 
                                                            FROM Contacts c 
                                                            INNER JOIN Organisations o ON o.Id = c.ApplyOrganisationID
                                                            WHERE c.Id = @UserId", new {userId});
            }
        }

        public async Task<Organisation> GetOrganisationByUserId(Guid userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

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
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

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