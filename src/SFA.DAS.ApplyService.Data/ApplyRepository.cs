using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Data
{
    public class ApplyRepository : IApplyRepository
    {
        private readonly IApplyConfig _config;
        
        public ApplyRepository(IConfigurationService configurationService)
        {
            _config = configurationService.GetConfig().Result;
        }
        
        public async Task<Workflow> GetCurrentWorkflow(string requestApplicationType, Guid requestApplyingOrganisationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var dataJson = await connection.QuerySingleAsync<string>(
                    "SELECT TOP(1) Data FROM QnAs WHERE Type = @ApplicationType AND Status = 'Live'", new {ApplicationType = requestApplicationType});

                var workflow = JsonConvert.DeserializeObject<Workflow>(dataJson);
                
                return workflow;
            }
        }

        public async Task SetOrganisationApplication(Workflow workflow, Guid applyingOrganisationId, string username)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var workflowJson = JsonConvert.SerializeObject(workflow);
                
                await connection.ExecuteAsync(
                    @"INSERT INTO Entities (ApplyingOrganisationId, QnAData, ApplicationStatus, CreatedAt, CreatedBy) 
                                                                VALUES (@ApplyingOrganisationId, @QnAData, 'Active', GETUTCDATE(), @CreatedBy)",
                    new {ApplyingOrganisationId = applyingOrganisationId, QnAData = workflowJson, CreatedBy = username});
            }
        }

        public async Task<Entity> GetEntity(Guid applicationId, Guid userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var application =
                    await connection.QueryFirstOrDefaultAsync<Entity>($@"SELECT e.* 
                                        FROM Entities e
                                            INNER JOIN Contacts c ON c.ApplyOrganisationID = e.ApplyingOrganisationId
                                        WHERE e.Id = @applicationId AND c.Id = @userId",
                        new {applicationId, userId});

                return application;
            }
        }

        public async Task SaveEntity(Entity entity, Guid applicationId, Guid userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync("UPDATE Entities SET QnAData = @qnaData, UpdatedAt = GETUTCDATE(), UpdatedBy = @userId WHERE Id = @applicationId",new
                {
                    qnaData = entity.QnAData, 
                    applicationId,
                    userId
                });
            }
        }

        public async Task<List<Entity>> GetApplications(Guid userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<Entity>(@"SELECT e.* FROM Contacts c
                                                    INNER JOIN Entities e ON e.ApplyingOrganisationId = c.ApplyOrganisationID
                                                    WHERE c.Id = '5280BF71-DC9B-4C99-9044-4EA5550D7FE3'", new {userId})).ToList();
            }
        }
    }
}