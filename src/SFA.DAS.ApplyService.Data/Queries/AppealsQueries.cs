﻿using System;
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

        public async Task<AppealFiles> GetStagedAppealFiles(Guid applicationId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
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

        public async Task<Appeal> GetAppeal(Guid applicationId, Guid oversightReviewId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                Appeal result = null;

                await connection.QueryAsync<Appeal, Appeal.AppealUpload, Appeal>(
                    @"SELECT
                        a.Id, a.Message, a.UserId, a.UserName, a.CreatedOn,
                        u.Id, u.Filename, u.ContentType
                        FROM [Appeal] a
                        LEFT JOIN [AppealUpload] u on u.AppealId = a.Id
                        JOIN [OversightReview] r ON r.Id = a.OversightReviewId
                        where a.OversightReviewId = @oversightReviewId
                        AND r.ApplicationId = @applicationId",
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
                        applicationId,
                        oversightReviewId
                    });

                return result;
            }
        }
    }
}
