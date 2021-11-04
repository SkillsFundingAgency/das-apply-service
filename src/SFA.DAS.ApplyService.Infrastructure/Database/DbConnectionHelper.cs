using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Services.AppAuthentication;
using SFA.DAS.ApplyService.Configuration;
using System.Data;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Infrastructure.Database
{
    public class DbConnectionHelper : IDbConnectionHelper
    {
        private readonly IApplyConfig _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public DbConnectionHelper(IConfigurationService configurationService, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configurationService.GetConfig().GetAwaiter().GetResult();
            _hostingEnvironment = hostingEnvironment;
        }

        public IDbConnection GetDatabaseConnection()
        {
            var connectionString = _configuration.SqlConnectionString;

            var connection = new System.Data.SqlClient.SqlConnection(connectionString);

            if (!_hostingEnvironment.IsDevelopment())
            {
                var generateTokenTask = GenerateTokenAsync();
                connection.AccessToken = generateTokenTask.GetAwaiter().GetResult();
            }

            return connection;
        }

        private static async Task<string> GenerateTokenAsync()
        {
            const string AzureResource = "https://database.windows.net/";

            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var accessToken = await azureServiceTokenProvider.GetAccessTokenAsync(AzureResource);

            return accessToken;
        }
    }
}
