using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class TabularDataRepository : ITabularDataRepository
    {
        private readonly IQnaApiClient _apiClient;

        public TabularDataRepository(IQnaApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        
        public async Task<TabularData> GetTabularDataAnswer(Guid applicationId, string questionTag)
        {
            var answer = await _apiClient.GetAnswerByTag(applicationId, questionTag);

            if (answer == null || answer.Value == null)
            {
                return null;
            }

            var tabularData = JsonConvert.DeserializeObject<TabularData>(answer.Value);

            return tabularData;
        }

        public async Task<bool> SaveTabularDataAnswer(Guid applicationId, Guid sectionId, string pageId, string questionId, TabularData tabularData)
        {
            var answerJson = JsonConvert.SerializeObject(tabularData);
            var answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = questionId,
                    Value = answerJson
                }
            };

            var result = await _apiClient.UpdatePageAnswers(applicationId, sectionId, pageId, answers);

            return await Task.FromResult(result.ValidationPassed);
        }
        
        public async Task<bool> AddTabularDataRecord(Guid applicationId, Guid sectionId, string pageId, string questionId, string questionTag, TabularDataRow record)
        {
            var tabularDataAnswer = await GetTabularDataAnswer(applicationId, questionTag);
            if (tabularDataAnswer == null)
            {
                tabularDataAnswer = new TabularData
                {
                    HeadingTitles = new List<string> { "Name", "Date of birth" },
                    DataRows = new List<TabularDataRow>()
                };
            }
            if (tabularDataAnswer.DataRows == null)
            {
                tabularDataAnswer.DataRows = new List<TabularDataRow>();
            }
            tabularDataAnswer.DataRows.Add(record);

            return await SaveTabularDataAnswer(applicationId, sectionId, pageId, questionId, tabularDataAnswer);
        }

        public async Task<bool> EditTabularDataRecord(Guid applicationId, Guid sectionId, string pageId, string questionId, string questionTag, TabularDataRow record, int index)
        {
            var tabularDataAnswer = await GetTabularDataAnswer(applicationId, questionTag);
            if (index < 0 || index >= tabularDataAnswer.DataRows.Count)
            {
                return await Task.FromResult(false);
            }
            tabularDataAnswer.DataRows[index] = record;

            return await SaveTabularDataAnswer(applicationId, sectionId, pageId, questionId, tabularDataAnswer);
        }
    }
}
