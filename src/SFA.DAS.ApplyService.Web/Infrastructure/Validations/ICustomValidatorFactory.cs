using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Validations
{
    public interface ICustomValidatorFactory
    {
        IEnumerable<ICustomValidator> GetCustomValidationsForPage(Page page, IFormFileCollection files);

        IEnumerable<ICustomValidator> GetCustomValidationsForQuestion(string pageId, string questionId, IFormFileCollection files);
    }
}
