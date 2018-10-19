using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply;

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
    }
}