using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface IQuestionPropertyTokeniser
    {
        Task<string> GetTokenisedValue(Guid applicationId, string tokenisedValue);
    }
}
