using System;
using System.Threading.Tasks;


namespace SFA.DAS.ApplyService.Application.Apply.GetAnswers
{
    public interface IGetAnswersService
    {
        Task<string> GetAnswers(string questionIdentifier, Guid applicationId);
        Task<IdentifierSourceValue?> GetIdentifierSourceValue(string identifier);
        Task<string> GetAnswersForQuestion(string questionId, Guid applicationId);

        Task<string> GetAnswersFromTable(string table, string field, Guid applicationId);
    }

    public struct IdentifierSourceValue
    {
        public string Source { get; set; }
        public string Value { get; set; }
    }
}