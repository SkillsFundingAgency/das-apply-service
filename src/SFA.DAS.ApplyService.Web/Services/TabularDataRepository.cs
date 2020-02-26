using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class TabularDataRepository : ITabularDataRepository
    {
        private readonly IQnaApiClient _apiClient;
        private readonly ITabularDataService _tabularDataService;
        public TabularDataRepository(IQnaApiClient apiClient, ITabularDataService tabularDataService)
        {
            _apiClient = apiClient;
            _tabularDataService = tabularDataService;
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

            var tabularDataDeduplicated = _tabularDataService.DeduplicateData(tabularData);

            var answerJson = JsonConvert.SerializeObject(tabularDataDeduplicated);
            var answers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = questionId,
                    Value = answerJson
                }
            };

            if (tabularData.DataRows.Count == 0)
            {
                var resultReset = await _apiClient.ResetPageAnswers(applicationId, sectionId, pageId);
                return await Task.FromResult(resultReset.ValidationPassed);
            }
           
            var result = await _apiClient.UpdatePageAnswers(applicationId, sectionId, pageId, answers);
            return await Task.FromResult(result.ValidationPassed);

        }
        
        public async Task<bool> UpsertTabularDataRecord(Guid applicationId, Guid sectionId, string pageId, string questionId, string questionTag, TabularDataRow record)
        {
            var tabularDataAnswer = await GetTabularDataAnswer(applicationId, questionTag);

            if (_tabularDataService.IsRowAlreadyPresent(tabularDataAnswer, record)) return true;

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