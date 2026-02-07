using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Extensions
{
    [HtmlTargetElement("auth")]
    public class AuthTagHelper : TagHelper
    {
        public string? Roles { get; set; }      // "Admin,Editor" gibi
        public string? Policy { get; set; }     // "AdminOnly" gibi

        [ViewContext]
        public ViewContext ViewContext { get; set; } = default!;

        private readonly IAuthorizationService _authorization;

        public AuthTagHelper(IAuthorizationService authorization)
        {
            _authorization = authorization;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = ViewContext.HttpContext.User;

            // Kimlik yoksa gizle
            if (user?.Identity?.IsAuthenticated != true)
            {
                output.SuppressOutput();
                return;
            }

            // Roles kontrolü
            if (!string.IsNullOrWhiteSpace(Roles))
            {
                var anyMatch = Roles
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Any(r => user.IsInRole(r));

                if (!anyMatch)
                {
                    output.SuppressOutput();
                    return;
                }
            }

            // Policy kontrolü
            if (!string.IsNullOrWhiteSpace(Policy))
            {
                var result = await _authorization.AuthorizeAsync(user, null, Policy);
                if (!result.Succeeded)
                {
                    output.SuppressOutput();
                    return;
                }
            }

            // Yetkiliyse içeriği render et
            await Task.CompletedTask;
        }
    }
}
