using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Validations
{
    public interface ICustomValidatorFactory
    {
        IEnumerable<ICustomValidator> GetCustomValidationsForQuestion(string pageId, string questionId, IFormFileCollection files);
    }
}
