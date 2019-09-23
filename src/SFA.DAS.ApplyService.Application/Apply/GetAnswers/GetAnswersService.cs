using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Apply.GetAnswers
{
    public class GetAnswersService : IGetAnswersService
    {
        private readonly IApplyRepository _applyRepository;

        public GetAnswersService(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<string> GetAnswersForQuestion(string questionTag, Guid applicationId)
        {
            var answers = await FindAnswersForQuestion(questionTag, applicationId);

            return answers == null 
                ? null 
                : string.Join(", ", answers?.Select(p => p.ToString()));
        }

        public async Task<string> GetJsonAnswersForQuestion(string questionTag, Guid applicationId)
        {
            var answers = await FindAnswersForQuestion(questionTag, applicationId);

            if (answers == null)
                return null;

            var jsonAnswers = new JObject();
            answers?.ForEach(p => jsonAnswers.Add(p.QuestionId, p.JsonValue));
            return JsonConvert.SerializeObject(jsonAnswers);
        }

        private async Task<List<Answer>> FindAnswersForQuestion(string questionTag, Guid applicationId)
        {
            var sections = await _applyRepository.GetSections(applicationId);

            foreach (var section in sections)
            {
                foreach (var qna in section.QnAData.Pages)
                {
                    if (qna.Questions.Any(x => x.QuestionTag == questionTag))
                    {
                        var questionId = qna.Questions.FirstOrDefault(x => x.QuestionTag == questionTag)?.QuestionId;
                        if (questionId == null) return null;
                        foreach (var page in qna.PageOfAnswers)
                        {
                            var answers =
                                page.Answers.Where(x => x.QuestionId == questionId ||
                                                        x.QuestionId.Contains($"{questionId}.")).ToList();
                            {
                                if (answers.Count < 1) continue;
                                if (answers.Count == 1) return answers.Take(1).ToList();
                                var subAnswers =
                                    answers.Where(x => x.QuestionId.Contains($"{questionId}.")).ToList();
                                return !subAnswers.Any()
                                    ? answers.Take(1).ToList()
                                    : (subAnswers.Count == 1
                                        ? subAnswers.Take(1).ToList()
                                        : subAnswers);
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
