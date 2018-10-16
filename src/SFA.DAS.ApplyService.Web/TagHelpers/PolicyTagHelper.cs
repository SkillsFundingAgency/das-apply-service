using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SFA.DAS.ApplyService.Web.TagHelpers
{
    [HtmlTargetElement("div", Attributes = PolicyTagHelperAttributeName )]
    [HtmlTargetElement("a", Attributes = PolicyTagHelperAttributeName )]
    public class PolicyTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAuthorizationService _authorizationService;

        public PolicyTagHelper(IHttpContextAccessor contextAccessor, IAuthorizationService authorizationService)
        {
            _contextAccessor = contextAccessor;
            _authorizationService = authorizationService;
        }

        private const string PolicyTagHelperAttributeName = "sfa-show-for-policy";

        [HtmlAttributeName(PolicyTagHelperAttributeName)]
        public string PolicyName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var allowed = _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, PolicyName).Result;
            if (!allowed.Succeeded)
            {
                output.SuppressOutput();
            }
        }
    }
}