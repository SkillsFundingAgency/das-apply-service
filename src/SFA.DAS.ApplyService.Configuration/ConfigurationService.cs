using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly string _environment;
        private readonly string _storageConnectionString;
        private readonly string _version;
        private readonly string _serviceName;

        public ConfigurationService(IWebHostEnvironment hostingEnvironment, string environment,
            string storageConnectionString, string version, string serviceName)
        {
            _hostingEnvironment = hostingEnvironment;
            _environment = environment;
            _storageConnectionString = storageConnectionString;
            _version = version;
            _serviceName = serviceName;
        }

        private ApplyConfig _applyConfig;

        public async Task<IApplyConfig> GetConfig()
        {
            //This class is registered as singleton, so it is possible that the _applyConfig is already populated
            if (_applyConfig != null) return _applyConfig;
            
            if (_environment == null || _storageConnectionString == null)
            {
                if (_hostingEnvironment.IsDevelopment())
                {
                    throw new DeveloperEnvironmentException(
                        @"Cannot find settings 'EnvironmentName' and 'ConfigurationStorageConnectionString' in appsettings.json. Please ensure your appsettings.json file is at least set up like `{
                    ""Logging"": {
                        ""IncludeScopes"": false,
                        ""LogLevel"": {
                            ""Default"": ""Debug"",
                            ""System"": ""Information"",
                            ""Microsoft"": ""Information""
                        }
                    },
                    ""ConfigurationStorageConnectionString"": ""UseDevelopmentStorage=true;"",
                    ""ConnectionStrings"": {
                        ""Redis"": """"
                    },
                    ""EnvironmentName"": ""LOCAL""
                }`");
                }

                throw new InvalidOperationException(
                    "Cannot find settings 'EnvironmentName' and 'ConfigurationStorageConnectionString'");
            }

            var conn = CloudStorageAccount.Parse(_storageConnectionString);
            var tableClient = conn.CreateCloudTableClient();
            var table = tableClient.GetTableReference("Configuration");

            var operation = TableOperation.Retrieve(_environment, $"{_serviceName}_{_version}");
            TableResult result;
            try
            {
                result = await table.ExecuteAsync(operation);
            }
            catch (Exception e)
            {
                if (_hostingEnvironment.IsDevelopment())
                {
                    throw new DeveloperEnvironmentException(
                        "Could not connect to Storage to retrieve settings.  Please ensure you have a Azure Storage server (azure or local emulator) configured with a `Configuration` table and the correct row.  See README.MD for details.");
                }

                throw new InvalidOperationException("Could not connect to Storage to retrieve settings.", e);
            }

            var dynResult = result.Result as DynamicTableEntity;
            if (result.HttpStatusCode == StatusCodes.Status404NotFound)
            {
                if (_hostingEnvironment.IsDevelopment())
                {
                    throw new DeveloperEnvironmentException(
                        "Cannot open Configuration table. Please ensure you have a Azure Storage server (azure or local emulator) configured with a `Configuration` table and the correct row.  See README.MD for details.");
                }

                throw new InvalidOperationException("Cannot open Configuration table.");
            }

            var data = dynResult.Properties["Data"].StringValue;

            ApplyConfig webConfig;
            try
            {
                webConfig = JsonConvert.DeserializeObject<ApplyConfig>(data);
            }
            catch (Exception)
            {
                if (_hostingEnvironment.IsDevelopment())
                {
                    throw new DeveloperEnvironmentException(
                        "There is a mismatch between ApplyConfig:IApplyConfig and the JSON returned from storage.");
                }

                throw;
            }
                
            _applyConfig = webConfig;

            return webConfig;
        }

        public string GetEnvironmentName()
        {
            return _environment;
        }
    }

    public interface IConfigurationService
    {
        Task<IApplyConfig> GetConfig();
        string GetEnvironmentName();
    }
}