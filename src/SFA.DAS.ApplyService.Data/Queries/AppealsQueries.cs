﻿using System;
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
    // TODO: APPEALREVIEW - Review once appeal work starts
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

                await connection.QueryAsync<Appeal, Appeal.AppealUpload, Appeal>(
                    @"SELECT
                        ap.*,
                        ul.Id, ul.Filename, ul.ContentType
                        FROM [Appeal] ap
                        LEFT JOIN [AppealUpload] ul on ul.AppealId = ap.Id
                        where ap.ApplicationId = @applicationId",
                        (appeal, upload) =>
                        {
                            if (result == null)
                            {
                                result = appeal;
                                result.Uploads = new List<Appeal.AppealUpload>();
                            }

                            if (upload != null)
                            {
                                result.Uploads.Add(upload);
                            }

                            return result;
                        }, new
                        {
                            applicationId
                        });

                return result;
            }
        }

        public async Task<AppealFiles> GetStagedAppealFiles(Guid applicationId)
        {
            using (var connection = GetConnection())
            {
                var files = (await connection.QueryAsync<AppealFile>(
                    @"SELECT Id, Filename FROM [AppealUpload] where ApplicationId = @applicationId and AppealId IS NULL ORDER BY CreatedOn ASC",
                    new
                    {
                        applicationId
                    })).ToList();

                return new AppealFiles
                {
                    Files = files
                };
            }
        }


    }
}
