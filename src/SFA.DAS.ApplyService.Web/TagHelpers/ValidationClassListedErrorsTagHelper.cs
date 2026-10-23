using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SFA.DAS.ApplyService.Web.TagHelpers
{
    /// <summary>
    /// Adds the CSS class in <c>sfa-validationerror-class</c> to the element when any of the
    /// comma-separated model state entries named in <c>sfa-validation-for-list</c> has validation errors.
    /// </summary>
    [HtmlTargetElement("div", Attributes = ValidationForListAttributeName + "," + ValidationErrorClassName)]
    [HtmlTargetElement("input", Attributes = ValidationForListAttributeName + "," + ValidationErrorClassName)]
    [HtmlTargetElement("select", Attributes = ValidationForListAttributeName + "," + ValidationErrorClassName)]
    [HtmlTargetElement("textarea", Attributes = ValidationForListAttributeName + "," + ValidationErrorClassName)]
    [HtmlTargetElement("fieldset", Attributes = ValidationForListAttributeName + "," + ValidationErrorClassName)]
    public class ValidationClassListedErrorsTagHelper : TagHelper
    {
        public const string ValidationForListAttributeName = "sfa-validation-for-list";
        public const string ValidationErrorClassName = "sfa-validationerror-class";

        [HtmlAttributeName(ValidationForListAttributeName)]
        public string ForList { get; set; }

        [HtmlAttributeName(ValidationErrorClassName)]
        public string ValidationErrorClass { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrWhiteSpace(ForList) || string.IsNullOrWhiteSpace(ValidationErrorClass)) return;

            var modelState = ViewContext.ViewData.ModelState;
            var hasErrors = ForList
                .Split(',')
                .Select(key => key.Trim())
                .Where(key => key.Length > 0)
                .Any(key => modelState.TryGetValue(key, out var entry) && entry.Errors.Any());

            if (!hasErrors) return;

            var tagBuilder = new TagBuilder(context.TagName);
            tagBuilder.AddCssClass(ValidationErrorClass);
            output.MergeAttributes(tagBuilder);
        }
    }
}
