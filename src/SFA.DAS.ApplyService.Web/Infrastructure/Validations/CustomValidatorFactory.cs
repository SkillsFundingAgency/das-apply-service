using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SFA.DAS.ApplyService.Web.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Validations
{
    public class CustomValidatorFactory : ICustomValidatorFactory
    {
        private readonly List<CustomValidationConfiguration> _configuration;

        public CustomValidatorFactory(IOptions<List<CustomValidationConfiguration>> configuration)
        {
            _configuration = configuration.Value;
        }

        public IEnumerable<ICustomValidator> GetCustomValidationsForQuestion(string pageId, string questionId, IFormFileCollection files)
        {
            var validationConfigs = _configuration.Where(c => c.PageId == pageId && c.QuestionId == questionId).ToList();

            var validators = new List<ICustomValidator>();

            foreach(var validationConfig in validationConfigs)
            {
                switch(validationConfig.Name)
                {
                    case "FileSizeMaxMb":
                        validators.Add(new FileSizeMaxMbValidator(validationConfig, files));
                        break;
                }
            }

            return validators;
        }
    }
}
