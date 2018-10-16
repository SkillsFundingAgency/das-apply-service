using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SFA.DAS.ApplyService.Web.TagHelpers
{
    [HtmlTargetElement("div", Attributes = RoleTagHelperAttributeName)]
    [HtmlTargetElement("a", Attributes = RoleTagHelperAttributeName)]
    public class RoleTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAuthorizationService _authorizationService;

        public RoleTagHelper(IHttpContextAccessor contextAccessor, IAuthorizationService authorizationService)
        {
            _contextAccessor = contextAccessor;
            _authorizationService = authorizationService;
        }

        private const string RoleTagHelperAttributeName = "sfa-show-for-roles";

        [HtmlAttributeName(RoleTagHelperAttributeName)]
        public string Roles { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string[] roles = Roles.Split(',');

            Dictionary<string, bool> roleValidation = roles.ToDictionary(role => role, role => _contextAccessor.HttpContext.User.IsInRole(role));

            if (roleValidation.All(kvp => kvp.Value == false))
            {
                output.SuppressOutput();
            }
        }
    }
}