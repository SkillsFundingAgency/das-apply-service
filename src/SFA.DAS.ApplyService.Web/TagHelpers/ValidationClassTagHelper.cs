using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SFA.DAS.ApplyService.Web.TagHelpers
{
    /// <summary>
    /// Adds the CSS class in <c>sfa-validationerror-class</c> to the element when the model state
    /// entry named in <c>sfa-validation-for</c> has validation errors.
    /// </summary>
    [HtmlTargetElement("div", Attributes = ValidationForAttributeName + "," + ValidationErrorClassName)]
    [HtmlTargetElement("input", Attributes = ValidationForAttributeName + "," + ValidationErrorClassName)]
    [HtmlTargetElement("select", Attributes = ValidationForAttributeName + "," + ValidationErrorClassName)]
    [HtmlTargetElement("textarea", Attributes = ValidationForAttributeName + "," + ValidationErrorClassName)]
    [HtmlTargetElement("fieldset", Attributes = ValidationForAttributeName + "," + ValidationErrorClassName)]
    public class ValidationClassTagHelper : TagHelper
    {
        public const string ValidationForAttributeName = "sfa-validation-for";
        public const string ValidationErrorClassName = "sfa-validationerror-class";

        [HtmlAttributeName(ValidationForAttributeName)]
        public string For { get; set; }

        [HtmlAttributeName(ValidationErrorClassName)]
        public string ValidationErrorClass { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrWhiteSpace(For) || string.IsNullOrWhiteSpace(ValidationErrorClass)) return;

            ViewContext.ViewData.ModelState.TryGetValue(For, out var entry);
            if (entry == null || !entry.Errors.Any()) return;

            var tagBuilder = new TagBuilder(context.TagName);
            tagBuilder.AddCssClass(ValidationErrorClass);
            output.MergeAttributes(tagBuilder);
        }
    }
}
