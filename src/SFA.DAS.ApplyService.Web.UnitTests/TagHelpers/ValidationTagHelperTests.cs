using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Web.TagHelpers;

namespace SFA.DAS.ApplyService.Web.UnitTests.TagHelpers
{
    [TestFixture]
    public class ValidationTagHelperTests
    {
        private static ViewContext BuildViewContext(params string[] fieldsWithErrors)
        {
            var modelState = new ModelStateDictionary();
            foreach (var field in fieldsWithErrors)
            {
                modelState.AddModelError(field, $"{field} is required");
            }

            var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor(), modelState);
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState);
            return new ViewContext(actionContext, Mock.Of<IView>(), viewData, Mock.Of<ITempDataDictionary>(), System.IO.TextWriter.Null, new HtmlHelperOptions());
        }

        private static string ClassOf(TagHelperOutput output)
        {
            var value = output.Attributes["class"].Value;
            if (value is IHtmlContent html)
            {
                using var writer = new System.IO.StringWriter();
                html.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                return writer.ToString();
            }

            return value?.ToString();
        }

        private static (TagHelperContext context, TagHelperOutput output) BuildTagHelperIo(string tagName, string existingClass = "govuk-form-group")
        {
            var context = new TagHelperContext(tagName, new TagHelperAttributeList(), new Dictionary<object, object>(), "test");
            var output = new TagHelperOutput(tagName, new TagHelperAttributeList { { "class", existingClass } },
                (_, _) => System.Threading.Tasks.Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
            return (context, output);
        }

        [Test]
        public void ValidationClass_adds_error_class_when_field_has_errors()
        {
            var helper = new ValidationClassTagHelper
            {
                For = "FirstTimeSignin",
                ValidationErrorClass = "govuk-form-group--error",
                ViewContext = BuildViewContext("FirstTimeSignin")
            };
            var (context, output) = BuildTagHelperIo("div");

            helper.Process(context, output);

            ClassOf(output).Should().Be("govuk-form-group govuk-form-group--error");
        }

        [Test]
        public void ValidationClass_leaves_element_alone_when_field_has_no_errors()
        {
            var helper = new ValidationClassTagHelper
            {
                For = "FirstTimeSignin",
                ValidationErrorClass = "govuk-form-group--error",
                ViewContext = BuildViewContext("SomeOtherField")
            };
            var (context, output) = BuildTagHelperIo("div");

            helper.Process(context, output);

            ClassOf(output).Should().Be("govuk-form-group");
        }

        [Test]
        public void ValidationClassListedErrors_adds_class_when_any_listed_field_has_errors()
        {
            var helper = new ValidationClassListedErrorsTagHelper
            {
                ForList = "TimeInRoleMonths, TimeInRoleYears",
                ValidationErrorClass = "govuk-input--error",
                ViewContext = BuildViewContext("TimeInRoleYears")
            };
            var (context, output) = BuildTagHelperIo("input", "govuk-input");

            helper.Process(context, output);

            ClassOf(output).Should().Be("govuk-input govuk-input--error");
        }

        [Test]
        public void ValidationClassListedErrors_leaves_element_alone_when_no_listed_field_has_errors()
        {
            var helper = new ValidationClassListedErrorsTagHelper
            {
                ForList = "TimeInRoleMonths,TimeInRoleYears",
                ValidationErrorClass = "govuk-input--error",
                ViewContext = BuildViewContext("OtherOrgName")
            };
            var (context, output) = BuildTagHelperIo("input", "govuk-input");

            helper.Process(context, output);

            ClassOf(output).Should().Be("govuk-input");
        }

        [Test]
        public void ValidationLink_sets_href_and_message_when_field_has_errors()
        {
            var helper = new ValidationLinkTagHelper
            {
                For = "OrganisationType",
                ViewContext = BuildViewContext("OrganisationType")
            };
            var (context, output) = BuildTagHelperIo("a", "govuk-link");

            helper.Process(context, output);

            output.Attributes["href"].Value.Should().Be("#OrganisationType");
            output.Content.GetContent().Should().Be("OrganisationType is required");
        }

        [Test]
        public void ValidationLink_leaves_anchor_alone_when_field_has_no_errors()
        {
            var helper = new ValidationLinkTagHelper
            {
                For = "OrganisationType",
                ViewContext = BuildViewContext()
            };
            var (context, output) = BuildTagHelperIo("a", "govuk-link");

            helper.Process(context, output);

            output.Attributes.ContainsName("href").Should().BeFalse();
            output.Content.GetContent().Should().BeEmpty();
        }
    }
}
