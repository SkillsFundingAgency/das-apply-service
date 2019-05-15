using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class ModelStatePersistAttribute : ActionFilterAttribute
    {
        private readonly ModelStatePersist _modelStatePersist;

        protected static readonly string TempKey = typeof(ModelStatePersistAttribute).FullName;

        public ModelStatePersistAttribute(ModelStatePersist modelStatePersist)
        {
            _modelStatePersist = modelStatePersist;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_modelStatePersist == ModelStatePersist.RestoreEntry)
            {
                if (filterContext.Controller is Controller controller)
                {
                    RestoreModelState(controller, filterContext.ModelState);
                }
            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                if (filterContext.Controller is Controller controller)
                {
                    if (filterContext.Result is RedirectResult
                        || filterContext.Result is RedirectToRouteResult
                        || filterContext.Result is RedirectToActionResult)
                    {
                        if (filterContext.ModelState != null && _modelStatePersist == ModelStatePersist.Store)
                        {
                            // export when redirecting
                            StoreModelState(controller, filterContext.ModelState);
                        }
                    }
                    else if(filterContext.Result is ViewResult && _modelStatePersist == ModelStatePersist.RestoreExit)
                    {
                        // by default the PRG pattern should restore the model state after refreshing 
                        // the view model to restore the previously entered invalid values
                        RestoreModelState(controller, filterContext.ModelState);
                    }
                    else
                    {
                        // otherwise remove previously exported data
                        controller.TempData.Remove(TempKey);
                    }
                }
            }

            base.OnActionExecuted(filterContext);
        }

        private void StoreModelState(Controller controller, ModelStateDictionary modelState)
        {
            var serializedModelState = SerializeModelState(modelState);
            controller.TempData[TempKey] = serializedModelState;
        }

        private void RestoreModelState(Controller controller, ModelStateDictionary modelState)
        {
            if (controller?.TempData[TempKey] is string serializedModelState)
            {
                var deserializedModelState = DeserializeModelState(serializedModelState);
                modelState.Merge(deserializedModelState);
            }
        }

        private string SerializeModelState(ModelStateDictionary modelStateDictionary)
        {
            // only the basic model state can be serialized
            var serializableModelState = modelStateDictionary
                .Select(kvp => new ModelStateTransferValue
                {
                    Key = kvp.Key,
                    AttemptedValue = kvp.Value.AttemptedValue,
                    RawValue = kvp.Value.RawValue,
                    ErrorMessages = kvp.Value.Errors.Select(err => err.ErrorMessage).ToList(),
                });

            return JsonConvert.SerializeObject(serializableModelState);
        }

        private ModelStateDictionary DeserializeModelState(string serializedModelState)
        {
            var deserializedModelState = JsonConvert.DeserializeObject<List<ModelStateTransferValue>>(serializedModelState);
            var modelState = new ModelStateDictionary();

            foreach (var item in deserializedModelState)
            {
                modelState.SetModelValue(item.Key, item.RawValue, item.AttemptedValue);
                foreach (var error in item.ErrorMessages)
                {
                    modelState.AddModelError(item.Key, error);
                }
            }
            return modelState;
        }

        public class ModelStateTransferValue
        {
            public string Key { get; set; }
            public string AttemptedValue { get; set; }
            public object RawValue { get; set; }
            public ICollection<string> ErrorMessages { get; set; } = new List<string>();
        }
    }

    public enum ModelStatePersist
    {
        Store,
        RestoreEntry,
        RestoreExit
    }
}
