using Microsoft.AspNetCore.Http;
using SFA.DAS.ApplyService.Domain.Apply;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.Services
{
    public interface IAnswerFormService
    {
        List<Answer> GetAnswersFromForm(HttpContext httpContext);
    }
}
