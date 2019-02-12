using System;
using System.Threading.Tasks;


namespace SFA.DAS.ApplyService.Application.Apply.GetAnswers
{
    public interface IGetAnswersService
    {
        Task<string> GetAnswersForQuestion(string questionTag, Guid applicationId);
    }
}