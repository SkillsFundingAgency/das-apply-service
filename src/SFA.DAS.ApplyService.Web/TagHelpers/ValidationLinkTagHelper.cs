using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SFA.DAS.ApplyService.Web.TagHelpers
{
    /// <summary>
    /// Turns an anchor into an error summary link: when the model state entry named in
    /// <c>sfa-validation-for</c> has errors, the href points at that field and the link text
    /// becomes the first error message.
    /// </summary>
    [HtmlTargetElement("a", Attributes = ValidationForAttributeName)]
    public class ValidationLinkTagHelper : TagHelper
    {
        public const string ValidationForAttributeName = "sfa-validation-for";

        [HtmlAttributeName(ValidationForAttributeName)]
        public string For { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrWhiteSpace(For)) return;

            ViewContext.ViewData.ModelState.TryGetValue(For, out var entry);
            if (entry == null || !entry.Errors.Any()) return;

            var tagBuilder = new TagBuilder("a");
            tagBuilder.Attributes.Add("href", $"#{For}");
            output.MergeAttributes(tagBuilder);

            output.Content.SetContent(entry.Errors[0].ErrorMessage);
        }
    }
}
