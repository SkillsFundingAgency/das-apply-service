using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Data
{
    public class AuditRepository : IAuditRepository
    {
        private readonly IApplyConfig _config;

        public AuditRepository(IConfigurationService configurationService)
        {
            _config = configurationService.GetConfig().Result;
        }

        public async Task Add(Audit audit)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.ExecuteAsync(@"INSERT INTO [dbo].[Audit]
           ([EntityType]
           ,[EntityId]
           ,[UserId]
           ,[UserName]
           ,[UserAction]
           ,[AuditDate]
           ,[InitialState]
           ,[UpdatedState]
           ,[Diff]
           ,[CorrelationId])
            VALUES
           (@EntityType
           ,@EntityId
           ,@UserId
           ,@UserName
           ,@UserAction
           ,@AuditDate
           ,@InitialState
           ,@UpdatedState
           ,@Diff
           ,@CorrelationId)",
                    audit);
            }
        }
    }
}
