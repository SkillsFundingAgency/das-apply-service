using Dapper;
using SFA.DAS.ApplyService.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Infrastructure.Database;

namespace SFA.DAS.ApplyService.Data
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        private readonly IDbConnectionHelper _dbConnectionHelper;

        public EmailTemplateRepository(IDbConnectionHelper dbConnectionHelper)
        {
            _dbConnectionHelper = dbConnectionHelper;
        }

        public async Task<EmailTemplate> GetEmailTemplate(string templateName)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var sql =
                    "SELECT * " +
                    "FROM [EmailTemplates] " +
                    "WHERE TemplateName = @templateName " + 
                    "AND Status = 'Live'";

                var emailTemplates = await connection.QueryAsync<EmailTemplate>(sql, new { templateName });
                return emailTemplates.FirstOrDefault();
            }
        }
    }
}
