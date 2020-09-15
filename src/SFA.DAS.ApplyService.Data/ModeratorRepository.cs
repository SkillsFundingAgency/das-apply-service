using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;

namespace SFA.DAS.ApplyService.Data
{
    public class ModeratorRepository : IModeratorRepository
    {
        private readonly IApplyConfig _config;
        private readonly ILogger<ModeratorRepository> _logger;

        public ModeratorRepository(IConfigurationService configurationService, ILogger<ModeratorRepository> logger)
        {
            _logger = logger;
            _config = configurationService.GetConfig().Result;
        }

        public async Task CreateModeratorPageOutcomes(List<ModeratorPageReviewOutcome> assessorPageReviewOutcomes)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(Guid));
            dataTable.Columns.Add("ApplicationId", typeof(Guid));
            dataTable.Columns.Add("SequenceNumber", typeof(int));
            dataTable.Columns.Add("SectionNumber", typeof(int));
            dataTable.Columns.Add("PageId", typeof(string));
            dataTable.Columns.Add("ModeratorUserId", typeof(string));
            dataTable.Columns.Add("ModeratorReviewStatus", typeof(string));
            dataTable.Columns.Add("ModeratorReviewComment", typeof(string));
            dataTable.Columns.Add("ExternalComment", typeof(string));
            dataTable.Columns.Add("CreatedAt", typeof(DateTime));
            dataTable.Columns.Add("CreatedBy", typeof(string));

            foreach (var outcome in assessorPageReviewOutcomes)
            {
                dataTable.Rows.Add(Guid.NewGuid(),
                    outcome.ApplicationId,
                    outcome.SequenceNumber,
                    outcome.SectionNumber,
                    outcome.PageId,
                    outcome.UserId,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    DateTime.UtcNow,
                    outcome.UserId
                );
            }

            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                await connection.OpenAsync();
                using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, null))
                {
                    bulkCopy.DestinationTableName = "ModeratorPageReviewOutcome";
                    await bulkCopy.WriteToServerAsync(dataTable);
                }
                connection.Close();
            }
        }
    }
}
