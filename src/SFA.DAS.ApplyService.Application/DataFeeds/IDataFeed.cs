using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.DataFeeds
{
    public interface IDataFeed
    {
        Task<DataFedAnswerResult> GetAnswer(Guid applicationId);
    }
}