using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Text.Encodings.Web;

namespace SFA.DAS.ApplyService.Web.TagHelpers
{
    [HtmlTargetElement(TagName)]
    public class SfaValidationErrorDetailTagHelper : TagHelper
    {
        public const string TagName = "sfa-validationerror-detail";

        public const string ValidationErrorClassName = "sfa-validationerror-class";

        public const string ValidationForAttributeName = "sfa-validation-for";

        [HtmlAttributeName(ValidationForAttributeName)]
        public string For { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(ValidationErrorClassName)]
        public string ValidationErrorClass { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ViewContext.ViewData.ModelState.TryGetValue(For, out ModelStateEntry entry);
            if (entry == null || !entry.Errors.Any())
            {
                output.SuppressOutput();
                return;
            }

            output.TagName = "span";  
            output.AddClass(ValidationErrorClass, HtmlEncoder.Default);
            output.Content.SetContent(entry.Errors.First().ErrorMessage);
        }
    }
}