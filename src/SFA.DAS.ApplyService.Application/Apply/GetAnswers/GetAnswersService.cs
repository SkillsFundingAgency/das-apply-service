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
            var sections = await _applyRepository.GetSections(applicationId);

            foreach (var section in sections)
            {
                foreach (var qna in section.QnAData.Pages)
                {
                    if (qna.Questions.Any(x => x.QuestionTag == questionTag))
                    {
                        var questionId = qna.Questions.FirstOrDefault(x => x.QuestionTag == questionTag)?.QuestionId;
                        if (questionId==null) return null;
                        foreach (var page in qna.PageOfAnswers)
                        {
                            var answers =
                                page.Answers.Where(x => x.QuestionId == questionId ||
                                                        x.QuestionId.Contains($"{questionId}.")).ToList();
                            {
                                if (answers.Count == 1) return answers.First().Value;
                                if (answers.Count < 1) continue;
                                var subAnswers =
                                    answers.Where(x => x.QuestionId.Contains($"{questionId}.")).ToList();
                                return !subAnswers.Any()
                                    ? answers.First().Value
                                    : (subAnswers.Count == 1
                                        ? subAnswers.First().Value
                                        : string.Join(",", subAnswers));
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
