using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Validation
{
    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidatorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public List<IValidator> Build(Question question)
        {
            var validators = new List<IValidator>();

            if (question.Input.Validations != null && question.Input.Validations.Any())
            {
                foreach (var inputValidation in question.Input.Validations)
                {
                    var validator = _serviceProvider.GetServices<IValidator>()
                        .FirstOrDefault(v => v.GetType().Name == inputValidation.Name + "Validator");

                    if (validator != null)
                    {
                        validator.ValidationDefinition = inputValidation;
                        validators.Add(validator);
                    }
                }
            }
            else
            {
                validators.Add(new NullValidator());
            }

            return validators;
        }
    }
}