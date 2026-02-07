using GR.Core.Entities.Identity;
using GR.Models.ViewModels.Auth;
using GR.Services.Abstract.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAppUserService userService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IAppRoleService roleService;
        public AuthController(IAppUserService userService, SignInManager<AppUser> signInManager, IAppRoleService roleService)
        {
            this.userService = userService;
            this.signInManager = signInManager;
            this.roleService = roleService;
        }
        [AllowAnonymous]
        public IActionResult SingIn()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SingIn(SingInViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var hasUser = await userService.findByEmailAsyn(request.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifre yanlış");
                return View();
            }
            await userService.addClaim(hasUser, new Claim("FullName", hasUser.FullName!));
            var signInResult = await signInManager.PasswordSignInAsync(hasUser, request.Password, request.RememberMe, true);

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "3 Dakika boyunca tektar giriş denemesi yapamazsınız.");
                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifre yanlış");
                return View();
            }

            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
