using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IConfigurationService _configurationService;
        private readonly List<Func<Task>> _operations;
        private SqlTransaction _transaction;

        public UnitOfWork(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
            _operations = new List<Func<Task>>();
        }

        public void Register(Func<Task> operation)
        {
            _operations.Add(operation);
        }

        public async Task Commit()
        {
            if (_operations.Count == 0) { return; }

            var config = await _configurationService.GetConfig();

            using (var connection = new SqlConnection(config.SqlConnectionString))
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
