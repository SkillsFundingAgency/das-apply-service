using Dapper;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Submit;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Data.DapperTypeHandlers;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

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

            SqlMapper.AddTypeHandler(typeof(ApplyData), new ApplyDataHandler());

            SqlMapper.AddTypeHandler(typeof(OrganisationDetails), new OrganisationDetailsHandler());
            SqlMapper.AddTypeHandler(typeof(QnAData), new QnADataHandler());
            SqlMapper.AddTypeHandler(typeof(ApplicationData), new ApplicationDataHandler());
            SqlMapper.AddTypeHandler(typeof(FinancialApplicationGrade), new FinancialApplicationGradeDataHandler());
        }



        public async Task<Guid> StartApplication(Guid applicationId, ApplyData applyData, Guid organisationId, Guid createdBy)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return await connection.QuerySingleAsync<Guid>(
                    @"INSERT INTO Apply (ApplicationId, OrganisationId, ApplicationStatus, ApplyData, ReviewStatus, CreatedBy, CreatedAt)
                                        OUTPUT INSERTED.[ApplicationId] 
                                        VALUES (@applicationId, @organisationId, @applicationStatus, @applyData, @reviewStatus, @createdBy, GETUTCDATE())",
                    new { applicationId, organisationId, applicationStatus = "In Progress", applyData, reviewStatus = "Draft", createdBy });
            }
        }

        public async Task<Domain.Entities.Apply> GetApplication(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var application = await connection.QuerySingleOrDefaultAsync<Domain.Entities.Apply>(@"SELECT * FROM apply WHERE ApplicationId = @applicationId", new { applicationId });

                //if (application != null)
                //{
                //    application.ApplyingOrganisation = await GetOrganisationForApplication(applicationId);
                //}

                return application;
            }
        }

        public async Task<List<Domain.Entities.Apply>> GetUserApplications(Guid userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<Domain.Entities.Apply>(@"SELECT a.* FROM Contacts c
                                                    INNER JOIN Apply a ON a.OrganisationId = c.ApplyOrganisationID
                                                    WHERE c.Id = @userId AND a.CreatedBy = @userId", new { userId })).ToList();
            }
        }

        public async Task<List<Domain.Entities.Apply>> GetOrganisationApplications(Guid userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<Domain.Entities.Apply>(@"SELECT a.* FROM Contacts c
                                                    INNER JOIN Apply a ON a.OrganisationId = c.ApplyOrganisationID
                                                    WHERE c.Id = @userId", new { userId })).ToList();
            }
        }

        public async Task<bool> CanSubmitApplication(Guid applicationId)
        {
            var canSubmit = false;

            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var application = await GetApplication(applicationId);
                var invalidApplicationStatuses = new List<string> { ApplicationStatus.Approved, ApplicationStatus.Rejected };

                // Application must exist and has not already been Approved or Rejected
                if (application != null && !invalidApplicationStatuses.Contains(application.ApplicationStatus))
                {
                    var otherAppsInProgress = await connection.QueryAsync<Domain.Entities.Apply>(@"
                                                        SELECT a.*
                                                        FROM Apply a
                                                        INNER JOIN Organisations o ON o.Id = a.OrganisationId
														INNER JOIN Contacts con ON a.OrganisationId = con.ApplyOrganisationID
                                                        WHERE a.OrganisationId = (SELECT OrganisationId FROM Apply WHERE ApplicationId = @applicationId)
														AND a.CreatedBy <> (SELECT CreatedBy FROM Apply WHERE ApplicationId = @applicationId)
                                                        AND a.ApplicationStatus NOT IN (@applicationStatusApproved, @applicationStatusApprovedRejected)",
                                                            new
                                                            {
                                                                applicationId,
                                                                applicationStatusApproved = ApplicationStatus.Approved,
                                                                applicationStatusApprovedRejected = ApplicationStatus.Rejected
                                                            });

                    canSubmit = !otherAppsInProgress.Any();
                }
            }

            return canSubmit;
        }

        public async Task SubmitApplication(Guid applicationId, ApplyData applyData, Guid submittedBy)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE Apply
                                                SET  ApplicationStatus = @ApplicationStatus, ApplyData = @applyData, ReviewStatus = @ReviewStatus, UpdatedBy = @submittedBy, UpdatedAt = GETUTCDATE() 
                                                WHERE  (Apply.ApplicationId = @applicationId)",
                                                new { applicationId, ApplicationStatus = ApplicationStatus.Submitted, applyData, ReviewStatus = "New", submittedBy });
            }
        }





        // NOTE: This is old stuff or things which are not migrated over yet
        public async Task<ApplicationSection> GetSection(Guid applicationId, int sequenceId, int sectionId, Guid? userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                if (userId == null)
                {
                    return (await connection.QuerySingleOrDefaultAsync<ApplicationSection>(@"SELECT asec.* 
                                                                FROM ApplicationSections asec
                                                                INNER JOIN Applications a ON a.Id = asec.ApplicationId
                                                                WHERE asec.ApplicationId = @applicationId AND asec.SectionId =@sectionId AND asec.SequenceId = @sequenceId",
                        new {applicationId, sequenceId, sectionId}));
                }

                return (await connection.QuerySingleOrDefaultAsync<ApplicationSection>(@"SELECT asec.* 
                                                                FROM ApplicationSections asec
                                                                INNER JOIN Applications a ON a.Id = asec.ApplicationId
                                                                INNER JOIN Contacts c ON c.ApplyOrganisationID = a.ApplyingOrganisationId
                                                                WHERE asec.ApplicationId = @applicationId AND asec.SectionId =@sectionId AND asec.SequenceId = @sequenceId AND c.Id = @userId",
                    new {applicationId, sequenceId, sectionId, userId}));
            }
        }

        public async Task<List<ApplicationSection>> GetSections(Guid applicationId, int sequenceId, Guid? userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                try
                {
                    if (userId == null)
                    {
                        return (await connection.QueryAsync<ApplicationSection>(@"SELECT asec.* 
                                                                FROM ApplicationSections asec
                                                                INNER JOIN Applications a ON a.Id = asec.ApplicationId
                                                                WHERE asec.ApplicationId = @applicationId AND asec.SequenceId = @sequenceId AND asec.NotRequired = 0",
                                                                    new { applicationId, sequenceId })).ToList();
                    }

                    return (await connection.QueryAsync<ApplicationSection>(@"SELECT asec.* 
                                                                FROM ApplicationSections asec
                                                                INNER JOIN Applications a ON a.Id = asec.ApplicationId
                                                                INNER JOIN Contacts c ON c.ApplyOrganisationID = a.ApplyingOrganisationId
                                                                WHERE asec.ApplicationId = @applicationId AND asec.SequenceId = @sequenceId AND c.Id = @userId AND asec.NotRequired = 0",
                                                                    new { applicationId, sequenceId, userId })).ToList();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "There has been an error trying to map ApplicationSections - this is most likely caused by to invalid JSON in the QnAData of ApplicationSections and WorkflowSections");
                    return null;
                }
            }
        }

        public async Task<List<ApplicationSection>> GetSections(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                try
                {

                        return (await connection.QueryAsync<ApplicationSection>(@"SELECT asec.* 
                                                                FROM ApplicationSections asec
                                                                INNER JOIN Applications a ON a.Id = asec.ApplicationId
                                                                WHERE asec.ApplicationId = @applicationId AND asec.NotRequired = 0",
                            new { applicationId })).ToList();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "There has been an error trying to map ApplicationSections for the Application - this is most likely caused by to invalid JSON in the QnAData of ApplicationSections and WorkflowSections");
                    return null;
                }
            }
        }


        public async Task<ApplicationSequence> GetSequence(Guid applicationId, int sequenceId, Guid? userId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                ApplicationSequence sequence = null;

                if (userId == null)
                {
                    sequence = await connection.QuerySingleOrDefaultAsync<ApplicationSequence>(@"SELECT seq.* 
                            FROM ApplicationSequences seq
                            INNER JOIN Applications a ON a.Id = seq.ApplicationId
                            WHERE seq.ApplicationId = @applicationId 
                            AND seq.SequenceId = @sequenceId", new { applicationId, sequenceId });
                }
                else
                {
                    sequence = await connection.QuerySingleOrDefaultAsync<ApplicationSequence>(@"SELECT seq.* 
                            FROM ApplicationSequences seq
                            INNER JOIN Applications a ON a.Id = seq.ApplicationId
                            INNER JOIN Contacts c ON c.ApplyOrganisationID = a.ApplyingOrganisationId
                            WHERE seq.ApplicationId = @applicationId 
                            AND seq.SequenceId = @sequenceId AND c.Id = @userId", new { applicationId, sequenceId, userId });
                }

                if(sequence != null)
                {
                    sequence.Sections = await GetSections(applicationId, sequenceId, userId);
                }

                return sequence;
            }
        }

        public async Task<ApplicationSequence> GetActiveSequence(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var sequence = await connection.QueryFirstOrDefaultAsync<ApplicationSequence>(@"SELECT seq.* 
                            FROM ApplicationSequences seq
                            INNER JOIN Applications a ON a.Id = seq.ApplicationId
                            WHERE seq.ApplicationId = @applicationId 
                            AND seq.IsActive = 1", new {applicationId});

                if (sequence != null)
                {
                    sequence.Sections = await GetSections(applicationId, (int)sequence.SequenceId, null);
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

        public async Task<Guid> CreateApplication(Guid applicationId, string applicationType, Guid applyingOrganisationId, Guid userId,
            Guid workflowId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(
                    @"INSERT INTO Applications (Id, ApplyingOrganisationId, ApplicationStatus, CreatedAt, CreatedBy, CreatedFromWorkflowId)                                       
                                        VALUES (@applicationId, @ApplyingOrganisationId, @applicationStatus, GETUTCDATE(), @userId, @workflowId)",
                    new {applicationId, applyingOrganisationId, userId, workflowId, applicationStatus = ApplicationStatus.InProgress});

                return await Task.FromResult(applicationId);
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
                                    (ApplicationId, SequenceId, Status, IsActive, Description)
                                SELECT        @applicationId AS ApplicationId, SequenceId, Status, IsActive, Description
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
                    await connection.ExecuteAsync(@"UPDATE ApplicationSections SET QnAData = @qnadata, Status = @Status, NotRequired = @NotRequired WHERE Id = @Id", applicationSection);    
                }
            }
        }
        
        public async Task UpdateSequences(List<ApplicationSequence> sequences)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                foreach (var applicationSequence in sequences)
                {
                    await connection.ExecuteAsync(@"UPDATE ApplicationSequences SET Status = @Status, IsActive = @IsActive, NotRequired = @NotRequired WHERE Id = @Id", applicationSequence);    
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

        public async Task UpdateSequenceStatus(Guid applicationId, int sequenceId, string sequenceStatus, string applicationStatus)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE ApplicationSequences
                                                SET    Status = @sequenceStatus
                                                FROM   ApplicationSequences INNER JOIN
                                                         Applications ON ApplicationSequences.ApplicationId = Applications.Id INNER JOIN
                                                         Contacts ON Applications.ApplyingOrganisationId = Contacts.ApplyOrganisationID
                                                WHERE  (ApplicationSequences.ApplicationId = @ApplicationId) AND (ApplicationSequences.SequenceId = @SequenceId);
                            
                                                UPDATE       Applications
                                                SET                ApplicationStatus = @applicationStatus
                                                FROM            Applications INNER JOIN
                                                                Contacts ON Applications.ApplyingOrganisationId = Contacts.ApplyOrganisationID
                                                WHERE  (Applications.Id = @ApplicationId)",
                    new {applicationId, sequenceId, sequenceStatus, applicationStatus});


                if (sequenceId == 1)
                {
                    switch (sequenceStatus)
                    {
                        case ApplicationSequenceStatus.FeedbackAdded:
                            await connection.ExecuteAsync(@"UPDATE Applications
                                                                SET ApplicationData = JSON_MODIFY(ApplicationData, '$.InitSubmissionFeedbackAddedDate', CONVERT(varchar(30), GETUTCDATE(), 126))
                                                                WHERE  (Applications.Id = @ApplicationId);",
                            new { applicationId });
                            break;
                        case ApplicationSequenceStatus.Rejected:
                        case ApplicationSequenceStatus.Approved:
                            await connection.ExecuteAsync(@"UPDATE Applications
                                                    SET ApplicationData = JSON_MODIFY(ApplicationData, '$.InitSubmissionClosedDate', CONVERT(varchar(30), GETUTCDATE(), 126))
                                                    WHERE  (Applications.Id = @ApplicationId);",
                            new { applicationId });
                            break;
                    }
                }
                else if (sequenceId == 2)
                {
                    switch (sequenceStatus)
                    {
                        case ApplicationSequenceStatus.FeedbackAdded:
                            await connection.ExecuteAsync(@"UPDATE Applications
                                                                SET ApplicationData = JSON_MODIFY(ApplicationData, '$.StandardSubmissionFeedbackAddedDate', CONVERT(varchar(30), GETUTCDATE(), 126))
                                                                WHERE  (Applications.Id = @ApplicationId);",
                            new { applicationId });
                            break;
                        case ApplicationSequenceStatus.Rejected:
                        case ApplicationSequenceStatus.Approved:
                            await connection.ExecuteAsync(@"UPDATE Applications
                                                    SET ApplicationData = JSON_MODIFY(ApplicationData, '$.StandardSubmissionClosedDate', CONVERT(varchar(30), GETUTCDATE(), 126))
                                                    WHERE  (Applications.Id = @ApplicationId);",
                            new { applicationId });
                            break;
                    }
                }
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


        public async Task UpdateApplicationStatus(Guid applicationId, string status)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"UPDATE Applications
                                                SET  ApplicationStatus = @status
                                                FROM Applications
                                                INNER JOIN Contacts ON Applications.ApplyingOrganisationId = Contacts.ApplyOrganisationID
                                                WHERE  (Applications.Id = @ApplicationId)", new {applicationId, status});
            }
        }

        public async Task DeleteRelatedApplications(Guid applicationId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var inProgressRelatedApplications = await connection.QueryAsync<Domain.Entities.Application>(@"SELECT a.* FROM Applications a
                                                                                                    INNER JOIN Contacts ON a.ApplyingOrganisationId = Contacts.ApplyOrganisationID
                                                                                                    WHERE a.ApplyingOrganisationId = (SELECT ApplyingOrganisationId FROM Applications WHERE Applications.Id = @applicationId)
                                                                                                    AND a.Id <> @applicationId
                                                                                                    AND a.ApplicationStatus NOT IN (@approvedStatus, @rejectedStatus)",
                                                                                            new { applicationId, approvedStatus = ApplicationStatus.Approved, rejectedStatus = ApplicationStatus.Rejected });

                // For now Reject them (and add deleted information)
                foreach (var app in inProgressRelatedApplications)
                {
                    _logger.LogInformation($"Deleting application {app.Id} due to Application {applicationId} being submitted and the Creating Org not being on the Register.");
                    await connection.ExecuteAsync(@"UPDATE ApplicationSequences
                                                        SET  IsActive = 0, Status = @rejectedStatus
                                                        WHERE  ApplicationSequences.ApplicationId = @applicationId;

                                                        UPDATE Applications
                                                        SET  ApplicationStatus = @rejectedSequenceStatus, DeletedAt = GETUTCDATE(), DeletedBy = 'System'
                                                        WHERE  Applications.Id = @applicationId;",
                                                    new { applicationId = app.Id, rejectedStatus = ApplicationStatus.Rejected, rejectedSequenceStatus = ApplicationSequenceStatus.Rejected });
                }
            }
        }

        public async Task<List<Domain.Entities.Apply>> GetOpenApplications()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .QueryAsync<Apply>(
                        @"SELECT ApplicationId, OrganisationId, ApplyData, ApplicationStatus, ReviewStatus
	                        FROM Apply
	                        WHERE ApplicationStatus = @gatewayAssessed",
                        new
                        {
                            gatewayAssessed = ApplicationStatus.GatewayAssessed
                        })).ToList();
            }
        }

        public async Task<List<Domain.Entities.Apply>> GetFeedbackAddedApplications()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return(await connection
                    .QueryAsync<Apply>(
                        @"SELECT ApplicationId, OrganisationId, ApplyData, ApplicationStatus, ReviewStatus
	                        FROM Apply
	                        WHERE ApplicationStatus = @feedbackAdded",
                        new
                        {
                            feedbackAdded = ApplicationStatus.FeedbackAdded
                        })).ToList();
            }
        }

        public async Task<List<Domain.Entities.Apply>> GetClosedApplications()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .QueryAsync<Apply>(
                        @"SELECT ApplicationId, OrganisationId, ApplyData, ApplicationStatus, ReviewStatus
	                        FROM Apply
	                        WHERE ApplicationStatus IN ( @approvedStatus, @rejectedStatus )",
                        new
                        {
                            approvedStatus = ApplicationStatus.Approved,
                            rejectedStatus = ApplicationStatus.Rejected
                        })).ToList();
            }
        }

        public async Task<List<Apply>> GetOpenFinancialApplications()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .QueryAsync<Apply>(
                        @"SELECT a.ApplicationId, a.OrganisationId, a.ApplicationStatus, a.ReviewStatus, a.ApplyData FROM Apply a
                        CROSS APPLY OPENJSON(a.ApplyData)
                        WITH (
                            Sequences nvarchar(max) '$.Sequences' AS JSON
                        ) AS i
                        CROSS APPLY (
                            SELECT *
                            FROM OPENJSON(i.Sequences)
                            WITH (
                                [SequenceNo] nvarchar(max) '$.SequenceNo',
                                [NotRequired] nvarchar(max) '$.NotRequired'
                            )
                        ) s
                        where s.SequenceNo = @financialHealthSequence and s.NotRequired = 'false'
                        and a.ApplicationStatus = @gatewayAssessed",
                        new
                        {
                            financialHealthSequence = 2,
                            gatewayAssessed = ApplicationStatus.GatewayAssessed
                        })).ToList();
            }
        }

        public async Task<List<Domain.Entities.Apply>> GetFeedbackAddedFinancialApplications()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                    .QueryAsync<Apply>(
                        @"SELECT a.ApplicationId, a.OrganisationId, a.ApplicationStatus, a.ReviewStatus, a.ApplyData FROM Apply a
                        CROSS APPLY OPENJSON(a.ApplyData)
                        WITH (
                            Sequences nvarchar(max) '$.Sequences' AS JSON
                        ) AS i
                        CROSS APPLY (
                            SELECT *
                            FROM OPENJSON(i.Sequences)
                            WITH (
                                [SequenceNo] nvarchar(max) '$.SequenceNo',
                                [NotRequired] nvarchar(max) '$.NotRequired'
                            )
                        ) s
                        where s.SequenceNo = @financialHealthSequence and s.NotRequired = 'false'
                        and a.ApplicationStatus = @feedbackAdded",
                        new
                        {
                            financialHealthSequence = 2,
                            feedbackAdded = ApplicationStatus.FeedbackAdded
                        })).ToList();
            }
        }

        public async Task<List<Domain.Entities.Apply>> GetClosedFinancialApplications()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection
                   .QueryAsync<Apply>(
                       @"SELECT a.ApplicationId, a.OrganisationId, a.ApplicationStatus, a.ReviewStatus, a.ApplyData FROM Apply a
                        CROSS APPLY OPENJSON(a.ApplyData)
                        WITH (
                            Sequences nvarchar(max) '$.Sequences' AS JSON
                        ) AS i
                        CROSS APPLY (
                            SELECT *
                            FROM OPENJSON(i.Sequences)
                            WITH (
                                [SequenceNo] nvarchar(max) '$.SequenceNo',
                                [NotRequired] nvarchar(max) '$.NotRequired'
                            )
                        ) s
                        where s.SequenceNo = @financialHealthSequence and s.NotRequired = 'false'
                        and a.ApplicationStatus IN ( @approvedStatus, @rejectedStatus )",
                       new
                       {
                           financialHealthSequence = 2,
                           approvedStatus = ApplicationStatus.Approved,
                           rejectedStatus = ApplicationStatus.Rejected
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

        public async Task<bool> MarkSectionAsCompleted(Guid applicationId, Guid applicationSectionId)
        {
            var completed = true;

            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var recordsAffected = await connection.ExecuteAsync(@"INSERT INTO ApplicationWorkflow 
                                                (ApplicationId, ApplicationSectionId, Completed)
                                                VALUES (@applicationId, @applicationSectionId, @completed)",
                    new
                    {
                        applicationId, applicationSectionId, completed
                    });

                return await Task.FromResult(recordsAffected > 0);
            }
        }

        public async Task<bool> IsSectionCompleted(Guid applicationId, Guid applicationSectionId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                return (await connection.QueryAsync<bool>(@"SELECT Completed FROM ApplicationWorkflow 
                                                            WHERE ApplicationId = @applicationId 
                                                            AND ApplicationSectionId = @applicationSectionId",
                    new
                    {
                        applicationId, applicationSectionId
                    })).FirstOrDefault();
            }
        }

        public async Task RemoveSectionCompleted(Guid applicationId, Guid applicationSectionId)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"DELETE FROM ApplicationWorkflow 
                                                Where ApplicationId = @ApplicationId and ApplicationSectionId = @ApplicationSectionId",
                    new
                    {
                        applicationId,
                        applicationSectionId
                    });
            }
        }

        public async Task<IEnumerable<RoatpApplicationStatus>> GetExistingApplicationStatusByUkprn(string ukprn)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var applicationStatuses = await connection.QueryAsync<RoatpApplicationStatus>(
                    @"select a.Id AS ApplicationId, a.ApplicationStatus AS Status
                      from dbo.Applications a
                      inner join dbo.Organisations o
                      on a.ApplyingOrganisationId = o.Id
                      where JSON_VALUE(o.OrganisationDetails, '$.OrganisationReferenceType') = 'UKRLP'
                      and o.OrganisationType = 'TrainingProvider'
                      and OrganisationUKPRN = @ukprn",
                 new { ukprn });

                return await Task.FromResult(applicationStatuses);
            }
        }

        public async Task<string> GetNextRoatpApplicationReference()
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                var nextInSequence = (await connection.QueryAsync<int>(@"SELECT NEXT VALUE FOR RoatpAppReferenceSequence")).FirstOrDefault();

                return $"APR{nextInSequence}";
            }
        }
    }
}
