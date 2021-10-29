using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.QueryResults;
using SFA.DAS.ApplyService.Infrastructure.Database;

namespace SFA.DAS.ApplyService.Data.Queries
{
    public class AppealsQueries : IAppealsQueries
    {
        private readonly IDbConnectionHelper _dbConnectionHelper;

        public AppealsQueries(IDbConnectionHelper dbConnectionHelper)
        {
            _dbConnectionHelper = dbConnectionHelper;
        }

        public async Task<Appeal> GetAppeal(Guid applicationId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
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

        public async Task<AppealFile> GetAppealFile(Guid applicationId, string fileName)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<AppealFile>(
                    @"SELECT * FROM [AppealFile] where ApplicationId = @applicationId AND FileName = @fileName",
                    new
                    {
                        applicationId,
                        fileName
                    });
            }
        }

        public async Task<List<AppealFile>> GetAppealFilesForApplication(Guid applicationId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
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
