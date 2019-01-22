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

//                if (question.Input.Options != null)
//                {
//                    foreach (var option in question.Input.Options)
//                    {
//                        if (option.FurtherQuestions != null && option.FurtherQuestions.Count > 0)
//                        {
//                            foreach (var questn in option.FurtherQuestions)
//                            {
//                                if (questn.Input.Validations != null && questn.Input.Validations.Any())
//                                {
//                                    foreach (var inputVal in questn.Input.Validations)
//                                    {
//                                        var validator = _serviceProvider.GetServices<IValidator>()
//                                            .FirstOrDefault(
//                                                v => v.GetType().Name == inputVal.Name + "Validator");
//
//                                        if (validator != null)
//                                        {
//                                            validator.ValidationDefinition = inputVal;
//                                            validators.Add(validator);
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
            }
            else
            {
                validators.Add(new NullValidator());
            }

            return validators;
        }
    }
}