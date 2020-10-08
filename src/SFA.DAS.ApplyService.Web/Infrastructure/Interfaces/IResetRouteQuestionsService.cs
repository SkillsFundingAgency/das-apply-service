using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Interfaces
{
    public interface IResetRouteQuestionsService
    {
        Task ResetRouteQuestions(Guid applicationId, int routeId);
    }
}