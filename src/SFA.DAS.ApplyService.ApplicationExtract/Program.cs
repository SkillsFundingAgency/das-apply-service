using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.ApplicationExtract
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configService = new ConfigService();
            configService.GetQnaApiConfig();

            Console.Clear();
            Console.WriteLine("Enter the output file path:");
            var outputPath = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Enter the application id (GUID):");
            var applicationId = Guid.Parse(Console.ReadLine());

            var qnaClient = new QnaApiClient(configService, new NullLogger<QnaApiClient>(), new QnaTokenService(configService, new HostingEnvironment { EnvironmentName = EnvironmentName.Development }));
            var result = await qnaClient.GetSections(applicationId);

            foreach (var section in result.Where(x => !x.NotRequired))
            {
                WriteSectionQuestionsAndAnswers(section, outputPath);
            }
        }

        private static void WriteSectionQuestionsAndAnswers(ApplicationSection section, string outputPath)
        {
            var outputSection = new Section();
            outputSection.Title = section.Title;

            foreach (var page in section.QnAData.Pages.Where(x => x.Active && !x.NotRequired))
            {
                foreach (var question in page.Questions)
                {
                    outputSection.Questions.Add(new Question
                    {
                        QuestionText = question.Label,
                        Answer = page.PageOfAnswers.SingleOrDefault()?.Answers.SingleOrDefault(x => x.QuestionId == question.QuestionId)?.Value
                    });
                }
            }

            var outputString = JsonConvert.SerializeObject(outputSection);
            File.WriteAllText(outputPath + @"\" + section.Title + ".json", outputString);
        }
    }

    public class ConfigService : IConfigurationService
    {
        private string _qnaUrl;
        private string _qnaTenantId;
        private string _qnaClientId;
        private string _qnaClientSecret;
        private string _qnaResourceId;

        public void GetQnaApiConfig()
        {
            Console.WriteLine("Enter the QnA API base url:");
            _qnaUrl = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Enter the QnA API tenant id:");
            _qnaTenantId = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Enter the QnA API client id:");
            _qnaClientId = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Enter the QnA API client secret:");
            _qnaClientSecret = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Enter the QnA API resource id:");
            _qnaResourceId = Console.ReadLine();
        }

        public Task<IApplyConfig> GetConfig()
        {
            return Task.FromResult(new ApplyConfig
            {
                QnaApiAuthentication = new QnaApiAuthentication
                {
                    ApiBaseAddress = _qnaUrl,
                    ClientId = _qnaClientId,
                    ClientSecret = _qnaClientSecret,
                    ResourceId = _qnaResourceId,
                    TenantId = _qnaTenantId
                }
            } as IApplyConfig);
        }

        public string GetEnvironmentName()
        {
            return "DEV";
        }
    }
}
