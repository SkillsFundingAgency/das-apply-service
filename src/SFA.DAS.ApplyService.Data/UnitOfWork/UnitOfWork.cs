using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Infrastructure.Database;

namespace SFA.DAS.ApplyService.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnectionHelper _dbConnectionHelper;
        private readonly List<Func<Task>> _operations;
        private SqlTransaction _transaction;

        public UnitOfWork(IDbConnectionHelper dbConnectionHelper)
        {
            _dbConnectionHelper = dbConnectionHelper;
            _operations = new List<Func<Task>>();
        }

        public void Register(Func<Task> operation)
        {
            _operations.Add(operation);
        }

        public async Task Commit()
        {
            if (_operations.Count == 0) { return; }

            using (var connection = _dbConnectionHelper.GetDatabaseConnection() as SqlConnection)
            {
                await connection.OpenAsync();
                using (_transaction = connection.BeginTransaction())
                {
                    foreach (var o in _operations)
                    {
                        await o();
                    }

                    _transaction.Commit();
                }

                _transaction = null;
            }

            Clear();
        }

        public void Clear()
        {
            _operations.Clear();
        }

        public SqlTransaction GetTransaction()
        {
            return _transaction;
        }
    }

    public interface IUnitOfWork
    {
        void Register(Func<Task> operation);
        Task Commit();
        void Clear();
        SqlTransaction GetTransaction();
    }
}
