using Dapper;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Data
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        private readonly IApplyConfig _config;

        public EmailTemplateRepository(IConfigurationService configurationService)
        {
            _config = configurationService.GetConfig().Result;
        }

        public async Task<EmailTemplate> GetEmailTemplate(string templateName)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

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
