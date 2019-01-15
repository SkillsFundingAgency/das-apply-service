using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Data.DapperTypeHandlers;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Data
{
    public class ApplyRepository : IApplyRepository
    {
        private readonly IApplyConfig _config;
        private readonly ILogger<ApplyRepository> _logger;

        public ApplyRepository(IConfigurationService configurationService, ILogger<ApplyRepository> logger)
        {
            _logger = logger;
            _config = configurationService.GetConfig().Result;
            SqlMapper.AddTypeHandler(typeof(OrganisationDetails), new OrganisationDetailsHandler());
            SqlMapper.AddTypeHandler(typeof(QnAData), new QnADataHandler());
            SqlMapper.AddTypeHandler(typeof(ApplicationData), new ApplicationDataHandler());

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

        public async Task<ApplicationSection> GetSection(Guid applicationId, int sequenceId, int sectionId, Guid? userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                if (userId == null)
                {
                    return (await connection.QuerySingleAsync<ApplicationSection>(@"SELECT asec.* 
                                                                FROM ApplicationSections asec
                                                                INNER JOIN Applications a ON a.Id = asec.ApplicationId
                                                                WHERE asec.ApplicationId = @applicationId AND asec.SectionId =@sectionId AND asec.SequenceId = @sequenceId",
                        new {applicationId, sequenceId, sectionId}));   
                }

                return (await connection.QuerySingleAsync<ApplicationSection>(@"SELECT asec.* 
                                                                FROM ApplicationSections asec
                                                                INNER JOIN Applications a ON a.Id = asec.ApplicationId
                                                                INNER JOIN Contacts c ON c.ApplyOrganisationID = a.ApplyingOrganisationId
                                                                WHERE asec.ApplicationId = @applicationId AND asec.SectionId =@sectionId AND asec.SequenceId = @sequenceId AND c.Id = @userId",
                    new {applicationId, sequenceId, sectionId, userId}));
            }
        }

        public async Task<ApplicationSequence> GetActiveSequence(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var sequence = await connection.QueryFirstAsync<ApplicationSequence>(@"SELECT seq.* 
                            FROM ApplicationSequences seq
                            INNER JOIN Applications a ON a.Id = seq.ApplicationId
                            WHERE seq.ApplicationId = @applicationId 
                            AND seq.IsActive = 1", new {applicationId});

                try
                {
                    var sections = (await connection.QueryAsync<ApplicationSection>(@"SELECT * FROM ApplicationSections 
                            WHERE ApplicationId = @ApplicationId 
                            AND SequenceId = @SequenceId",
                        sequence)).ToList();

                    sequence.Sections = sections;
                }
                catch (Exception e)
                {
                    _logger.LogError(e,"There has been an error trying to map ApplicationSections - this is most likely caused by to invalid JSON in the QnAData of ApplicationSections and WorkflowSections");
                }
                
                
                return sequence;
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
                                        VALUES (@ApplyingOrganisationId, @applicationStatus, GETUTCDATE(), @userId, @workflowId)",
                    new {applyingOrganisationId, userId, workflowId, applicationStatus = ApplicationStatus.InProgress});
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

        public async Task<List<ApplicationSection>> CopyWorkflowToApplication(Guid applicationId, Guid workflowId, string organisationType)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<ApplicationSection>(@"
                                INSERT INTO ApplicationSequences
                                    (ApplicationId, SequenceId, Status, IsActive)
                                SELECT        @applicationId AS ApplicationId, SequenceId, Status, IsActive
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
                    await connection.ExecuteAsync(@"UPDATE ApplicationSections SET QnAData = @qnadata, Status = @Status WHERE Id = @Id", applicationSection);    
                }
            }
        }

        public async Task SaveSection(ApplicationSection section, Guid? userId = null)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE ApplicationSections SET QnAData = @qnadata, Status = @Status WHERE Id = @Id", section);       
            }
        }

        public async Task<Guid> CreateNewWorkflow(string workflowType)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return await connection.QuerySingleAsync<Guid>(@"
                                                    UPDATE Workflows SET Status = 'Deleted' WHERE Type = @workflowType;

                                                    INSERT INTO Workflows 
                                                            (Description, Version, Type, Status, CreatedAt, CreatedBy, ReferenceFormat) 
                                                    OUTPUT INSERTED.[Id]
                                                    VALUES  ('EPAO Workflow','1.0',@workflowType, 'Live', GETUTCDATE(), 'SpreadsheetImport', 'AAD'); ",
                    new {workflowType});
            }
        }

        public async Task CreateSequence(Guid workflowId, double sequenceId, bool isActive)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(
                    "INSERT INTO WorkflowSequences (WorkflowId, SequenceId, Status, IsActive) VALUES (@workflowId, @sequenceId, 'Draft', @isActive)",
                    new {workflowId, sequenceId, isActive});
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

        public async Task SubmitApplicationSequence(ApplicationSubmitRequest request, ApplicationData applicationdata)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE ApplicationSequences
                                                SET    Status = 'Submitted'
                                                FROM   ApplicationSequences INNER JOIN
                                                         Applications ON ApplicationSequences.ApplicationId = Applications.Id INNER JOIN
                                                         Contacts ON Applications.ApplyingOrganisationId = Contacts.ApplyOrganisationID
                                                WHERE  (ApplicationSequences.ApplicationId = @ApplicationId) AND (ApplicationSequences.SequenceId = @SequenceId) AND Contacts.Id = @UserId;
                            
                                                UPDATE ApplicationSections
                                                SET    Status = (CASE WHEN JSON_VALUE(ApplicationSections.QnAData, '$.FinancialApplicationGrade.SelectedGrade') IN ('Excellent','Good','Satisfactory') THEN 'Graded' ELSE 'Submitted' END)
                                                FROM   ApplicationSections INNER JOIN
                                                            Applications ON ApplicationSections.ApplicationId = Applications.Id INNER JOIN
                                                            Contacts ON Applications.ApplyingOrganisationId = Contacts.ApplyOrganisationID
                                                WHERE  (ApplicationSections.ApplicationId = @ApplicationId) AND (ApplicationSections.SequenceId = @SequenceId) AND Contacts.Id = @UserId;

                                                UPDATE       Applications
                                                SET                ApplicationStatus = 'Submitted', ApplicationData = @applicationdata
                                                FROM            Applications INNER JOIN
                                                                Contacts ON Applications.ApplyingOrganisationId = Contacts.ApplyOrganisationID
                                                WHERE  (Applications.Id = @ApplicationId) AND Contacts.Id = @UserId	",
                    new {request.ApplicationId, request.UserId, request.SequenceId, applicationdata });
            }
            
        }

        public async Task UpdateSequenceStatus(Guid applicationId, int sequenceId, string status, string applicationStatus)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE ApplicationSequences
                                                SET    Status = @status
                                                FROM   ApplicationSequences INNER JOIN
                                                         Applications ON ApplicationSequences.ApplicationId = Applications.Id INNER JOIN
                                                         Contacts ON Applications.ApplyingOrganisationId = Contacts.ApplyOrganisationID
                                                WHERE  (ApplicationSequences.ApplicationId = @ApplicationId) AND (ApplicationSequences.SequenceId = @SequenceId);
                            
                                                UPDATE       Applications
                                                SET                ApplicationStatus = @applicationStatus
                                                FROM            Applications INNER JOIN
                                                                Contacts ON Applications.ApplyingOrganisationId = Contacts.ApplyOrganisationID
                                                WHERE  (Applications.Id = @ApplicationId)",
                    new {applicationId, sequenceId, status, applicationStatus});
            }
        }

        public async Task CloseSequence(Guid applicationId, int sequenceId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE ApplicationSequences
                                                SET    IsActive = 0
                                                FROM   ApplicationSequences INNER JOIN
                                                         Applications ON ApplicationSequences.ApplicationId = Applications.Id INNER JOIN
                                                         Contacts ON Applications.ApplyingOrganisationId = Contacts.ApplyOrganisationID
                                                WHERE  (ApplicationSequences.ApplicationId = @ApplicationId) AND (ApplicationSequences.SequenceId = @SequenceId);",
                    new {applicationId, sequenceId});
            }
        }

        public async Task<List<ApplicationSequence>> GetSequences(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
               return (await connection.QueryAsync<ApplicationSequence>(@"SELECT * FROM ApplicationSequences WHERE ApplicationId = @applicationId",
                    new {applicationId})).ToList();
            }
        }

        public async Task OpenSequence(Guid applicationId, int nextSequenceId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE ApplicationSequences
                                                SET    IsActive = 1
                                                FROM   ApplicationSequences INNER JOIN
                                                         Applications ON ApplicationSequences.ApplicationId = Applications.Id INNER JOIN
                                                         Contacts ON Applications.ApplyingOrganisationId = Contacts.ApplyOrganisationID
                                                WHERE  (ApplicationSequences.ApplicationId = @ApplicationId) AND (ApplicationSequences.SequenceId = @nextSequenceId);",
                    new {applicationId, nextSequenceId});
            }
        }

        public async Task UpdateApplicationData(Guid applicationId, ApplicationData applicationData)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE Applications
                                                SET    ApplicationData = @applicationData
                                                WHERE  Applications.Id = @applicationId",
                    new {applicationId, applicationData});
            }
        }

        public async Task<Domain.Entities.Application> GetApplication(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return await connection.QuerySingleAsync<Domain.Entities.Application>(@"SELECT * FROM Applications WHERE Id = @applicationId", new {applicationId});
            }
        }

        public async Task UpdateApplicationStatus(Guid applicationId, string status)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE       Applications
                                                SET                ApplicationStatus = @status
                                                FROM            Applications INNER JOIN
                                                Contacts ON Applications.ApplyingOrganisationId = Contacts.ApplyOrganisationID
                                                WHERE  (Applications.Id = @ApplicationId)", new {applicationId, status});
            }
        }

        public async Task<List<ApplicationSection>> GetSections(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<ApplicationSection>(@"SELECT * FROM ApplicationSections WHERE ApplicationId = @applicationId",
                    new {applicationId})).ToList();
            }
        }

        public async Task<List<dynamic>> GetNewApplications(int sequenceId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .QueryAsync(
                        @"SELECT OrganisationName, ApplicationId, SequenceId, sec3Status AS FinanceStatus,  
	                        CASE WHEN (SequenceId = 1 AND (sec1Status = 'Evaluated' AND sec2Status = 'Evaluated')) OR (SequenceId = 2 AND sec4Status = 'Evaluated') THEN @sectionStatusEvaluated
		                         WHEN (SequenceId = 1 AND (sec1Status = 'Submitted' AND sec2Status = 'Submitted')) OR (SequenceId = 2 AND sec4Status = 'Submitted') THEN @sectionStatusSubmitted
		                         ELSE @sectionStatusInProgress
	                        END As SequenceStatus
                        FROM (
	                        SELECT seq.SequenceId,
	                        MAX(CASE WHEN SectionId = 1 THEN sec1.[Status] ELSE NULL END) Sec1Status,
	                        MAX(CASE WHEN SectionId = 2 THEN sec1.[Status] ELSE NULL END) Sec2Status,
	                        MAX(CASE WHEN SectionId = 3 THEN sec1.[Status] ELSE NULL END) Sec3Status,
	                        MAX(CASE WHEN SectionId = 4 THEN sec1.[Status] ELSE NULL END) Sec4Status,
	                        appl.ApplyingOrganisationId, appl.id ApplicationId, org.Name OrganisationName
	                        FROM Applications appl
	                        INNER JOIN ApplicationSequences seq ON seq.ApplicationId = appl.Id AND seq.SequenceId = @sequenceId
	                        INNER JOIN ApplicationSections sec1 ON sec1.ApplicationId = appl.Id 
	                        INNER JOIN Organisations org ON org.Id = appl.ApplyingOrganisationId
	                        WHERE appl.ApplicationStatus = @applicationStatusSubmitted AND seq.Status = @sequenceStatusSubmitted
	                        GROUP BY seq.SequenceId, appl.ApplyingOrganisationId, appl.id, org.Name
                        ) ab",
                        new
                        {
                            sequenceId, // 1 for new applications, until accepted; 2 when reviewing Capacity & Capability
                            applicationStatusSubmitted = ApplicationStatus.Submitted,
                            sequenceStatusInProgress = ApplicationSectionStatus.InProgress,
                            sequenceStatusSubmitted = ApplicationSectionStatus.Submitted,
                            sectionStatusEvaluated = ApplicationSectionStatus.Evaluated,
                            sectionStatusSubmitted = ApplicationSectionStatus.Submitted,
                            sectionStatusInProgress = ApplicationSectionStatus.InProgress
                        })).ToList();
            }
        }

        public async Task<List<dynamic>> GetNewFinancialApplications()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync(@"SELECT org.Name, sec.Status, appl.Id
                                FROM Applications appl
                            INNER JOIN Organisations org ON org.Id = appl.ApplyingOrganisationId
                            INNER JOIN ApplicationSections sec ON sec.ApplicationId = appl.Id
                            WHERE appl.ApplicationStatus = @applicationStatusSubmitted 
                            AND sec.SectionId = 3 
                            AND (sec.Status = @financialStatusInProgress OR sec.Status = @financialStatusSubmitted)",
                    new
                    {
                        applicationStatusSubmitted = ApplicationStatus.Submitted, 
                        financialStatusInProgress = ApplicationSectionStatus.InProgress, 
                        financialStatusSubmitted = ApplicationSectionStatus.Submitted
                    })).ToList();
            }
        }
        //
        //        public async Task UpdateFinancialGrade(Guid applicationId, FinancialApplicationGrade updatedGrade)
        //        {
        //            using (var connection = new SqlConnection(_config.SqlConnectionString))
        //            {
        //                await connection.ExecuteAsync(@"UPDATE Applications
        //                                                SET    ApplicationData = @serialisedData
        //                                                WHERE  Applications.Id = @applicationId",
        //                    new {applicationId, updatedGrade});
        //            }
        //        }

        public async Task StartApplicationReview(Guid applicationId, int sectionId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE ApplicationSections 
                                                SET Status = 'In Progress'
                                                WHERE ApplicationId = @applicationId AND SectionId = @sectionId  AND SequenceId = 1",
                    new { applicationId, sectionId });
            }
        }

        public async Task StartFinancialReview(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE ApplicationSections 
                                                SET Status = 'In Progress'
                                                WHERE ApplicationId = @applicationId AND SectionId = 3 AND SequenceId = 1",
                    new {applicationId});
            }
        }

        public async Task<Organisation> GetOrganisationForApplication(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return await connection.QuerySingleAsync<Organisation>(@"SELECT org.* FROM Organisations org 
                                                                        INNER JOIN Applications appl ON appl.ApplyingOrganisationId = org.Id
                                                                        WHERE appl.Id = @ApplicationId",
                    new {applicationId});
            }
        }

        public async Task<string> CheckOrganisationStandardStatus(Guid applicationId, int standardId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
               var applicationStatuses= await connection.QueryAsync<string>(@"select top 1 A.applicationStatus from Applications A
                                                                    where JSON_VALUE(ApplicationData,'$.StandardCode')= @standardId
                                                                    and ApplyingOrganisationId in 
                                                                        (select ApplyingOrganisationId from Applications where Id = @applicationId)
",
                    new { applicationId, standardId });

                return !applicationStatuses.Any() ? string.Empty : applicationStatuses.FirstOrDefault();
            }
        }

        public async Task<List<dynamic>> GetPreviousFinancialApplications()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync(@"SELECT org.Name, sec.Status, appl.Id, 
                                                JSON_VALUE(sec.QnAData, '$.FinancialApplicationGrade.GradedBy') AS GradedBy, 
	                                            JSON_VALUE(sec.QnAData, '$.FinancialApplicationGrade.GradedDateTime') AS GradedDateTime,
                                                JSON_VALUE(sec.QnAData, '$.FinancialApplicationGrade.SelectedGrade') AS Grade
                                FROM Applications appl
                            INNER JOIN Organisations org ON org.Id = appl.ApplyingOrganisationId
                            INNER JOIN ApplicationSections sec ON sec.ApplicationId = appl.Id
                            WHERE appl.ApplicationStatus = @applicationStatusSubmitted
                            AND sec.SectionId = 3 
                            AND (sec.Status = @financialStatusGraded)",
                    new
                    {
                        applicationStatusSubmitted = ApplicationStatus.Submitted, 
                        financialStatusGraded = ApplicationSectionStatus.Graded
                    })).ToList();
            }
        }

        public async Task ClearAssets()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"DELETE FROM Assets");
            }
        }

        public async Task<List<ApplicationSection>> GetApplicationSections()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var applicationSections = (await connection.QueryAsync<ApplicationSection>(@"SELECT * FROM ApplicationSections WHERE DisplayType != 'Backup'")).ToList();

                if (applicationSections.Count() != 4)
                {
                    throw new Exception("Expecting only four Application Sections");
                }
                
                return applicationSections;
            }
        }

        public async Task<List<WorkflowSection>> GetWorkflowSections()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var applicationSections = (await connection.QueryAsync<WorkflowSection>(@"SELECT * FROM WorkflowSections")).ToList();
                
                return applicationSections;
            }
        }
        public async Task<int> GetNextAppReferenceSequence()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<int>(@"SELECT NEXT VALUE FOR AppRefSequence")).FirstOrDefault();

            }
        }

        public async Task<string> GetWorkflowReferenceFormat(Guid requestApplicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<string>(@"SELECT wf.ReferenceFormat
                                FROM Applications app inner join Workflows wf ON wf.Id = app.CreatedFromWorkflowId
                                AND app.id = @requestApplicationId",
                    new
                    {
                        requestApplicationId
                    })).FirstOrDefault();
            }
        }
        
    }
}