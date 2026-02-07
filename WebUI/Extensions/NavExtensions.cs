using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebUI.Extensions
{
    public static class NavExtensions 
    {
        // controllers: "Admin,Users", actions: "Index,Messages" gibi virgüllü listeler alabilir.
        public static string IsActive(this IHtmlHelper html, string? controllers = null, string? actions = null, string cssClass = "active")
        {
            var routeData = html.ViewContext.RouteData.Values;
            var currentAction = (routeData["action"]?.ToString() ?? "");
            var currentController = (routeData["controller"]?.ToString() ?? "");

            bool controllerMatch = string.IsNullOrEmpty(controllers)
                                   || controllers.Split(',').Any(c => c.Trim().Equals(currentController, StringComparison.OrdinalIgnoreCase));

            bool actionMatch = string.IsNullOrEmpty(actions)
                               || actions.Split(',').Any(a => a.Trim().Equals(currentAction, StringComparison.OrdinalIgnoreCase));

            return (controllerMatch && actionMatch) ? cssClass : "";
        }
    }
}
