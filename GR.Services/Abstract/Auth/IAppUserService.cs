using GR.Core.Entities.Identity;
using GR.Models.Entities.Identity;
using GR.Models.ViewModels.Admin;
using GR.Models.ViewModels.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GR.Services.Abstract.Auth
{
    public interface IAppUserService
    {
        Task<IdentityResult> createAsync(AddUserViewModel request);
        Task<IdentityResult> createAsync(AppUser request);
        Task<IdentityResult> updateAsync(UpdateUserViewModel request);
        Task<IdentityResult> updateUserImagePathAsync(AppUser User);
        Task<IdentityResult> deleteAsync(AppUser user);
        Task<AppUser> findByIdAsync(string userId);
        Task<AppUser> findByEmailAsyn(string email);

        Task<List<AppUser>> getAll();

        Task<List<UserRole>> getUsersRoleList(List<AppUser> users);
        Task<IList<string>> getRolesAsync(AppUser user);

        Task<IdentityResult> addToRole(AppUser user, string role);
        Task<AppUser> findByNameAsync(string userName);
        Task<IdentityResult> addClaim(AppUser user, Claim claim);
        Task<IList<AppUser>> getUsersInRole(string roleName);
        Task<IdentityResult> ifChangeRole(string userId, string roleName);
        Task<(bool, IEnumerable<IdentityError>?)> ChangePasswordAsync(PasswordChangeViewModel model);
        Task<string> GeneratePasswordResetTokenAsync(AppUser user);
        Task<IdentityResult> ResetPasswordAsync(AppUser user, string token, string password);
        Task<List<string>> GetAllEmailsAsync();
        Task<AppUser> GetUserAsync(ClaimsPrincipal user);
        Task<AppUser> GetUserByIdAsync(string userId);
        Task<List<AppUser>> GetUserInIsAvtiveClass();
        Task<IdentityResult> changeIsActive(string userId);
    }
}
