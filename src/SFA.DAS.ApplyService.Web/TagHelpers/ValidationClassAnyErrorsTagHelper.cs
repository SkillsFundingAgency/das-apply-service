using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SFA.DAS.ApplyService.Web.TagHelpers
{
  [HtmlTargetElement("div", Attributes = ValidationErrorClassName)]
  [HtmlTargetElement("input", Attributes = ValidationErrorClassName)]
  [HtmlTargetElement("fieldset", Attributes = ValidationErrorClassName)]
  public class ValidationClassAnyErrorsTagHelper : TagHelper
  {
    public const string ValidationErrorClassName = "sfa-anyvalidationerror-class";

    [HtmlAttributeName(ValidationErrorClassName)]
    public string ValidationErrorClass { get; set; }

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      if (ViewContext.ViewData.ModelState.IsValid) return;

      var tagBuilder = new TagBuilder(context.TagName);
      tagBuilder.AddCssClass(ValidationErrorClass);
      output.MergeAttributes(tagBuilder);
    }
  }

}