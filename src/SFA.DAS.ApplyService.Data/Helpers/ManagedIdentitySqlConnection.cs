using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Data.Helpers
{
    public static class ManagedIdentitySqlConnection
    {
        public static SqlConnection GetSqlConnection(string sqlConnectionString, AzureServiceTokenProvider azureServiceTokenProvider)
        {
            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);
            if (azureServiceTokenProvider != null)
            {
                sqlConnection.AccessToken = azureServiceTokenProvider.GetAccessTokenAsync("https://database.windows.net/").Result;
            }
            return sqlConnection;
        }
    }
}
