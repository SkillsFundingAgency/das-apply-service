using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SFA.DAS.ApplyService.Web.TagHelpers
{
    [HtmlTargetElement("a", Attributes = ValidationForAttributeName)]
    public class ValidationLinkTagHelper : TagHelper
    {
        public const string ValidationForAttributeName = "sfa-validation-for";

        [HtmlAttributeName(ValidationForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ModelStateEntry entry;
            ViewContext.ViewData.ModelState.TryGetValue(For.Name, out entry);
            if (entry == null || !entry.Errors.Any()) return;

            var tagBuilder = new TagBuilder("a");

            tagBuilder.Attributes.Add("href", $"#{For.Name}");
            output.MergeAttributes(tagBuilder);

            output.Content.SetContent(entry.Errors[0].ErrorMessage);
        }
    }
}