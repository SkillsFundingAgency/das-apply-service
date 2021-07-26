using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Data.UnitOfWork;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Data
{
    public class AuditRepository : IAuditRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuditRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public void Add(Audit audit)
        {
            _unitOfWork.Register(() => PersistAdd(audit));
        }

        public async Task PersistAdd(Audit audit)
        {
            var transaction = _unitOfWork.GetTransaction();

            await transaction.Connection.ExecuteAsync(@"INSERT INTO [dbo].[Audit]
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
                audit, transaction);
        }
    }
}
