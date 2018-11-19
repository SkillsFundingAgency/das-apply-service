using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Data
{
    public class OrganisationRepository : IOrganisationRepository
    {
        private readonly IApplyConfig _config;
        
        public OrganisationRepository(IConfigurationService configurationService)
        {
            _config = configurationService.GetConfig().Result;
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