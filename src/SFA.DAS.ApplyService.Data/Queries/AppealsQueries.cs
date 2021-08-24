using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;

namespace SFA.DAS.ApplyService.Data.Queries
{
    public class AppealsQueries : IAppealsQueries
    {
        private readonly IApplyConfig _config;

        public AppealsQueries(IConfigurationService configurationService)
        {
            _config = configurationService.GetConfig().Result;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_config.SqlConnectionString);
        }

        public async Task<Appeal> GetAppeal(Guid applicationId)
        {
            using (var connection = GetConnection())
            {
                Appeal result = null;

                await connection.QueryAsync<Appeal, AppealFile, Appeal>(
                    @"SELECT
                        ap.*,
                        apFile.Id, apFile.ApplicationId, apFile.Filename, apFile.ContentType
                        FROM [Appeal] ap
                        LEFT JOIN [AppealFile] apFile on apFile.ApplicationId = ap.ApplicationId
                        where ap.ApplicationId = @applicationId",
                        (appeal, upload) =>
                        {
                            if (result == null)
                            {
                                result = appeal;
                                result.AppealFiles = new List<AppealFile>();
                            }

                            if (upload != null)
                            {
                                result.AppealFiles.Add(upload);
                            }

                            return result;
                        }, new
                        {
                            applicationId
                        });

                return result;
            }
        }

        public async Task<List<AppealFile>> GetAppealFilesForApplication(Guid applicationId)
        {
            using (var connection = GetConnection())
            {
                return (await connection.QueryAsync<AppealFile>(
                    @"SELECT * FROM [AppealFile] where ApplicationId = @applicationId ORDER BY CreatedOn ASC",
                    new
                    {
                        applicationId
                    })).ToList();
            }
        }
    }
}
