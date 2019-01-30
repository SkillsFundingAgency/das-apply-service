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

        public async Task<Organisation> CreateOrganisation(Organisation organisation, Guid userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                connection.Execute(
                    "INSERT INTO [Organisations] ([Id],[Name],[OrganisationType],[OrganisationUKPRN], " +
                    "[OrganisationDetails],[Status],[CreatedAt],[CreatedBy],[RoEPAOApproved],[RoATPApproved]) " +
                    "VALUES (NEWID(), @Name, REPLACE(@OrganisationType, ' ', ''), @OrganisationUkprn, @OrganisationDetails, 'New', GETUTCDATE(), @CreatedBy, @RoEPAOApproved, @RoATPApproved)",
                    new { organisation.Name, organisation.OrganisationType, organisation.OrganisationUkprn, organisation.OrganisationDetails, organisation.CreatedBy, organisation.RoEPAOApproved, organisation.RoATPApproved });

                var org = await GetOrganisationByName(organisation.Name);

                if (org != null)
                {
                    connection.Execute(
                                "UPDATE [Contacts] " +
                                "SET ApplyOrganisationID = @Id " +
                                "WHERE Id = @userId",
                                new { org.Id, userId });
                }

                return org;
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

        public async Task<Organisation> UpdateOrganisation(Organisation organisation, Guid userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                connection.Execute(
                    "UPDATE [Organisations] " +
                    "SET [UpdatedAt] = GETUTCDATE(), [UpdatedBy] = @UpdatedBy, [Name] = @Name, " +
                    "[OrganisationType] = @OrganisationType, [OrganisationUKPRN] = @OrganisationUkprn, " +
                    "[OrganisationDetails] = @OrganisationDetails, [RoEPAOApproved] = @RoEPAOApproved, [RoATPApproved] = @RoATPApproved " +
                    "WHERE [Id] = @Id",
                    new { organisation.Id, organisation.Name, organisation.OrganisationType, organisation.OrganisationUkprn, organisation.OrganisationDetails, organisation.UpdatedBy, organisation.RoEPAOApproved, organisation.RoATPApproved });

                var org = await GetOrganisationByName(organisation.Name);

                if (org != null)
                {
                    connection.Execute(
                                "UPDATE [Contacts] " +
                                "SET ApplyOrganisationID = @Id " +
                                "WHERE Id = @userId",
                                new { org.Id, userId });
                }

                return org;
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
                    "WHERE Name LIKE @name";

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
    }
}