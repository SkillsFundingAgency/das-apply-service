using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SFA.DAS.ApplyService.Web.Extensions
{
    public static class SerializableModelStateDictionaryExtensions
    {
        public static ModelStateDictionary ToModelState(this SerializableModelStateDictionary serializableModelState)
        {
            var modelState = new ModelStateDictionary();

            foreach (var data in serializableModelState.Data)
            {
                modelState.SetModelValue(data.Key, data.RawValue, data.AttemptedValue);

                foreach (var error in data.ErrorMessages)
                {
                    modelState.AddModelError(data.Key, error);
                }
            }

            return modelState;
        }
    }
}
