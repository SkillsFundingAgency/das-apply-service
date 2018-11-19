using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

//        public async Task<Workflow> GetCurrentWorkflow(string requestApplicationType, Guid requestApplyingOrganisationId)
//        {
//            using (var connection = new SqlConnection(_config.SqlConnectionString))
//            {
//                var dataJson = await connection.QuerySingleAsync<string>(
//                    "SELECT TOP(1) Data FROM QnAs WHERE Type = @ApplicationType AND Status = 'Live'", new {ApplicationType = requestApplicationType});
//
//                var workflow = JsonConvert.DeserializeObject<Workflow>(dataJson);
//                
//                return workflow;
//            }
//        }
//
//        public async Task SetOrganisationApplication(Workflow workflow, Guid applyingOrganisationId, string username)
//        {
//            using (var connection = new SqlConnection(_config.SqlConnectionString))
//            {
//                var workflowJson = JsonConvert.SerializeObject(workflow);
//                
//                await connection.ExecuteAsync(
//                    @"INSERT INTO Entities (ApplyingOrganisationId, QnAData, ApplicationStatus, CreatedAt, CreatedBy) 
//                                                                VALUES (@ApplyingOrganisationId, @QnAData, 'Active', GETUTCDATE(), @CreatedBy)",
//                    new {ApplyingOrganisationId = applyingOrganisationId, QnAData = workflowJson, CreatedBy = username});
//            }
//        }

        public Task<Domain.Entities.Application> GetEntity(Guid applicationId, Guid userId)
        {
            throw new NotImplementedException();
        }

//        public async Task<BitVector32.Section> GetSection(Guid applicationId, Guid userId, int sectionId)
//        {
//            using (var connection = new SqlConnection(_config.SqlConnectionString))
//            {
//                var application =
//                    await connection.QueryFirstOrDefaultAsync<Domain.Entities.Application>($@"SELECT e.* 
//                                        FROM Entities e
//                                            INNER JOIN Contacts c ON c.ApplyOrganisationID = e.ApplyingOrganisationId
//                                        WHERE e.Id = @applicationId AND c.Id = @userId",
//                        new {applicationId, userId});
//
//                return application;
//            }
//        }

        public async Task SaveEntity(Domain.Entities.Application application, Guid applicationId, Guid userId)
        {
//            using (var connection = new SqlConnection(_config.SqlConnectionString))
//            {
//                await connection.ExecuteAsync("UPDATE Entities SET QnAData = @qnaData, UpdatedAt = GETUTCDATE(), UpdatedBy = @userId WHERE Id = @applicationId",new
//                {
//                    qnaData = application.QnAData, 
//                    applicationId,
//                    userId
//                });
//            }
        }

        public async Task<List<Domain.Entities.Application>> GetApplications(Guid userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<Domain.Entities.Application>(@"SELECT a.* FROM Contacts c
                                                    INNER JOIN Applications a ON a.ApplyingOrganisationId = c.ApplyOrganisationID
                                                    WHERE c.Id = @userId", new {userId})).ToList();
            }
        }

        public async Task<ApplicationSection> GetSection(Guid applicationId, int sequenceId, int sectionId, Guid userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QuerySingleAsync<ApplicationSection>(@"SELECT asec.* 
                                                                FROM ApplicationSections asec
                                                                INNER JOIN Applications a ON a.Id = asec.ApplicationId
                                                                INNER JOIN Contacts c ON c.ApplyOrganisationID = a.ApplyingOrganisationId
                                                                WHERE asec.ApplicationId = @applicationId AND asec.SectionId =@sectionId AND asec.SequenceId = @sequenceId AND c.Id = @userId",
                    new {applicationId, sequenceId, sectionId, userId}));
            }
        }

        public async Task<List<ApplicationSection>> GetSections(Guid applicationId, int sequenceId, Guid userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<ApplicationSection>(@"SELECT asec.* 
                                                                FROM ApplicationSections asec
                                                                INNER JOIN Applications a ON a.Id = asec.ApplicationId
                                                                INNER JOIN Contacts c ON c.ApplyOrganisationID = a.ApplyingOrganisationId
                                                                WHERE asec.ApplicationId = @applicationId AND asec.SequenceId =@sequenceId AND c.Id = @userId",
                    new {applicationId, sequenceId, userId})).ToList();
            }
        }

        public async Task<List<Asset>> GetAssets()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<Asset>(@"SELECT * FROM Assets")).ToList();
            }
        }

        public async Task<Guid> CreateApplication(string applicationType, Guid applyingOrganisationId, Guid userId,
            Guid workflowId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return await connection.QuerySingleAsync<Guid>(
                    @"INSERT INTO Applications (ApplyingOrganisationId, ApplicationStatus, CreatedAt, CreatedBy, CreatedFromWorkflowId)
                                        OUTPUT INSERTED.[Id] 
                                        VALUES (@ApplyingOrganisationId, 'Draft', GETUTCDATE(), @userId, @workflowId)",
                    new {applyingOrganisationId, userId, workflowId});
            }
        }

        public async Task<Guid> GetLatestWorkflow(string applicationType)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QuerySingleAsync<Guid>(
                    @"SELECT Id FROM Workflows WHERE [Type] = @applicationType AND Status = 'Live'",
                    new {applicationType}));
            }
        }

        public async Task<List<ApplicationSection>> CopyWorkflowToApplication(Guid applicationId, Guid workflowId, int organisationType)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<ApplicationSection>(@"
                                INSERT INTO ApplicationSequences
                                    (ApplicationId, SequenceId, Status)
                                SELECT        @applicationId AS ApplicationId, SequenceId, Status
                                FROM            WorkflowSequences
                                WHERE        (WorkflowId = @workflowId);
                    
                                INSERT INTO ApplicationSections
                                    (ApplicationId, SequenceId, SectionId, QnAData, Title, LinkTitle, Status, DisplayType)
                                SELECT        @applicationId AS ApplicationId, SequenceId, SectionId, QnAData, Title, LinkTitle, Status, DisplayType
                                FROM            WorkflowSections
                                WHERE        (WorkflowId = @workflowId AND (DisallowedOrgTypes IS NULL OR DisallowedOrgTypes NOT LIKE @organisationType));

                                SELECT * FROM ApplicationSections WHERE ApplicationId = @applicationId;", new {applicationId, workflowId, organisationType = $"%|{organisationType}|%"})).ToList();
            }
        }

        public async Task UpdateSections(List<ApplicationSection> sections)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                foreach (var applicationSection in sections)
                {
                    await connection.ExecuteAsync(@"UPDATE ApplicationSections SET QnAData = @qnadata WHERE Id = @Id", applicationSection);    
                }
            }
        }

        public async Task SaveSection(ApplicationSection section, Guid userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE ApplicationSections SET QnAData = @qnadata WHERE Id = @Id", section);       
            }
        }

        public async Task<Guid> CreateNewWorkflow(string workflowType)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return await connection.QuerySingleAsync<Guid>(@"
                                                    UPDATE Workflows SET Status = 'Deleted' WHERE Type = @workflowType;

                                                    INSERT INTO Workflows 
                                                            (Description, Version, Type, Status, CreatedAt, CreatedBy) 
                                                    OUTPUT INSERTED.[Id]
                                                    VALUES  ('EPAO Workflow','1.0',@workflowType, 'Live', GETUTCDATE(), 'SpreadsheetImport'); ",
                    new {workflowType});
            }
        }

        public async Task CreateSequence(Guid workflowId, double sequenceId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(
                    "INSERT INTO WorkflowSequences (WorkflowId, SequenceId, Status) VALUES (@workflowId, @sequenceId, 'Draft')",
                    new {workflowId, sequenceId});
            }
        }

        public async Task CreateSection(WorkflowSection section)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(
                    @"INSERT INTO WorkflowSections (WorkflowId, SequenceId, SectionId, QnAData, Title, LinkTitle, Status, DisplayType) 
                                                            VALUES (@workflowId, @SequenceId, @SectionId, @QnAData, @Title, @LinkTitle, @Status, @DisplayType)",
                    section);
            }
        }

        public async Task AddAssets(Dictionary<string, string> assets)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                foreach (var asset in assets)
                {
                    var cleanText = asset.Value?.Replace("\n", "<br/>");
                    try
                    {
                        await connection.ExecuteAsync(
                            "INSERT INTO Assets (Reference, Type, Text, Format, Status, CreatedAt, CreatedBy) VALUES (@reference, '', @text, '', 'Live', GETUTCDATE(), 'SpreadsheetImport')"
                            , new {reference = asset.Key, text = cleanText});   
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                    
                }
            }
        }

        public async Task<List<Domain.Entities.Application>> GetApplicationsToReview()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .QueryAsync<Domain.Entities.Application, Organisation, Domain.Entities.Application>(
                        @"SELECT * FROM Applications a
                INNER JOIN Organisations o ON o.Id = a.ApplyingOrganisationId",
                        (application, organisation) =>
                        {
                            application.ApplyingOrganisation = organisation;
                            return application;
                        })).ToList();
            }
        }
    }
}