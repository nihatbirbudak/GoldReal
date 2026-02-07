using GR.Services.Abstract.Auth;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace WebUI.Extensions
{
    public class UserRoleNamesTagHelper : TagHelper
    {
        public string UserId { get; set; } = null!;

        private readonly IAppUserService appUserService;

        public UserRoleNamesTagHelper(IAppUserService appUserService)
        {
            this.appUserService = appUserService;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = await appUserService.findByIdAsync(UserId);
            var userRoles = await appUserService.getRolesAsync(user);
            var stringBuilder = new StringBuilder();

            userRoles.ToList().ForEach(x =>
            {
                stringBuilder.Append(x);
            });

            output.Content.SetHtmlContent(stringBuilder.ToString());
        }
    }
}
