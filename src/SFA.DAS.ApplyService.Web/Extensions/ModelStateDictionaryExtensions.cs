using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace SFA.DAS.ApplyService.Web.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        public static void AddModelError(this ModelStateDictionary modelState, ValidationException ex)
        {
            if (string.IsNullOrWhiteSpace(ex.Message) && !ex.Errors.Any())
            {
                modelState.AddModelError("", "");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(ex.Message))
                {
                    modelState.AddModelError("", ex.Message);
                }

                foreach (var validationError in ex.Errors)
                {
                    var key = ExpressionHelper.GetExpressionText(validationError.PropertyName);

                    modelState.AddModelError(key, validationError.ErrorMessage);
                }
            }
        }

        public static SerializableModelStateDictionary ToSerializable(this ModelStateDictionary modelState)
        {
            var data = modelState
                .Select(kvp => new SerializableModelState
                {
                    AttemptedValue = kvp.Value.AttemptedValue,
                    ErrorMessages = kvp.Value.Errors.Select(e => e.ErrorMessage).ToList(),
                    Key = kvp.Key,
                    RawValue = kvp.Value.RawValue
                })
                .ToList();

            return new SerializableModelStateDictionary
            {
                Data = data
            };
        }
    }
}
