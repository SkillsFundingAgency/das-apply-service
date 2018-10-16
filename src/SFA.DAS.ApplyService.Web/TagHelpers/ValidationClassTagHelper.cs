using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SFA.DAS.ApplyService.Web.TagHelpers
{
  [HtmlTargetElement("div", Attributes = ValidationForAttributeName + "," + ValidationErrorClassName)]
  [HtmlTargetElement("input", Attributes = ValidationForAttributeName + "," + ValidationErrorClassName)]
  [HtmlTargetElement("fieldset", Attributes = ValidationForAttributeName + "," + ValidationErrorClassName)]
  public class ValidationClassTagHelper : TagHelper
  {
    public const string ValidationErrorClassName = "sfa-validationerror-class";

    public const string ValidationForAttributeName = "sfa-validation-for";

    [HtmlAttributeName(ValidationForAttributeName)]
    public ModelExpression For { get; set; }

    [HtmlAttributeName(ValidationErrorClassName)]
    public string ValidationErrorClass { get; set; }

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      ModelStateEntry entry;
      ViewContext.ViewData.ModelState.TryGetValue(For.Name, out entry);
      if (entry == null || !entry.Errors.Any()) return;

      var tagBuilder = new TagBuilder(context.TagName);
      tagBuilder.AddCssClass(ValidationErrorClass);
      output.MergeAttributes(tagBuilder);
    }
  }
}