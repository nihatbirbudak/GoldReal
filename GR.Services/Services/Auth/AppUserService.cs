using GR.Core.Entities.Identity;
using GR.Models.Entities.Identity;
using GR.Models.ViewModels.Admin;
using GR.Models.ViewModels.Auth;
using GR.Services.Abstract.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace GR.Services.Services.Auth
{
    public class AppUserService : IAppUserService
    {
        private readonly UserManager<AppUser> userManager;

        public AppUserService(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IdentityResult> createAsync(AddUserViewModel request)
        {
            request.UserName = request.Name + " " + request.Surname;
            var trimName = string.Empty;
            foreach (var c in request.UserName!.Trim().ToLower().ToArray())
            {
                if (c != ' ')
                {
                    if (c == 'ş')
                    {
                        trimName += 's';
                    }
                    else if (c == 'ç')
                    {
                        trimName += 'c';
                    }
                    else if (c == 'ı')
                    {
                        trimName += 'i';
                    }
                    else
                    {
                        trimName += c;
                    }
                }
            }
            return await userManager.CreateAsync(new AppUser() { 
                UserName = trimName, 
                Email = request.Email, 
                FullName = request.UserName.Trim(),
                description=request.description,
                PicturePath=request.PicturePath,
                PhoneNumber=request.PhoneNumber,
                Name=request.Name,
                Surname=request.Surname,
                EmployeeStatus=request.EmployeeStatus,
            }, request.PasswordConfirm!);
        }

        public async Task<IdentityResult> createAsync(AppUser request)
        {
            var trimName = string.Empty;
            foreach (var c in request.UserName!.Trim().ToLower().ToArray())
            {
                if (c != ' ')
                {
                    if (c == 'ş')
                    {
                        trimName += 's';
                    }
                    else if (c == 'ç')
                    {
                        trimName += 'c';
                    }
                    else if (c == 'ı')
                    {
                        trimName += 'i';
                    }
                    else if (c == 'ö')
                    {
                        trimName += 'o';
                    }
                    else
                    {
                        trimName += c;
                    }
                }
            }
            return await userManager.CreateAsync(request);
        }

        public async Task<IdentityResult> addToRole(AppUser user, string role)
        {
            return await userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> deleteAsync(AppUser user)
        {

            return await userManager.DeleteAsync(user);
        }

        public async Task<AppUser> findByIdAsync(string userId)
        {
            var result = await userManager.FindByIdAsync(userId);
            return result!;
        }

        public async Task<IdentityResult> updateAsync(UpdateUserViewModel request)
        {
            var user = await userManager.FindByIdAsync(request.Id!);
            if (!await userManager.IsInRoleAsync(user!, request.Role!))
            {
                var rolesUser = await userManager.GetUsersInRoleAsync("Admin");
                if (rolesUser.Count > 1)
                {
                    var UserRole = await userManager.GetRolesAsync(user!);
                    var roleIdentityResutl = await userManager.AddToRoleAsync(user!, request.Role!);

                    if (!roleIdentityResutl.Succeeded)
                    {
                        return roleIdentityResutl;
                    }

                    await userManager.RemoveFromRoleAsync(user!, UserRole.FirstOrDefault()!);
                }
            }
            var trimName = string.Empty;
            if (user!.UserName != request.UserName)
            {
                
                foreach (var c in request.UserName!.Trim().ToLower().ToArray())
                {
                    if (c != ' ')
                    {
                        if (c == 'ş')
                        {
                            trimName += 's';
                        }
                        else if (c == 'ç')
                        {
                            trimName += 'c';
                        }
                        else if (c == 'ı')
                        {
                            trimName += 'i';
                        }
                        else
                        {
                            trimName += c;
                        }
                    }
                }
            }
            else
            {
                trimName = user.UserName;
            }


            user!.UserName = trimName;
            user.Email = request.Email;
            user.FullName = (request.Name+" "+request.Surname).Trim();
            user.description = request.description;
            user.PicturePath = request.PicturePath;
            user.PhoneNumber = request.PhoneNumber;
            user.Name = request.Name;
            user.Surname = request.Surname;
            user.EmployeeStatus = request.EmployeeStatus;

            return await userManager.UpdateAsync(user!);
        }

        public async Task<IdentityResult> updateUserImagePathAsync(AppUser User)
        {
            var user = await userManager.FindByIdAsync(User.Id!);
            user!.PicturePath = User.PicturePath;
            return await userManager.UpdateAsync(user!);
        }

        public async Task<List<AppUser>> getAll()
        {
            return await userManager.Users.ToListAsync();
        }

        public async Task<List<UserRole>> getUsersRoleList(List<AppUser> users)
        {
            var list = new List<UserRole>();
            foreach (var user in users)
            {
                var userNew = new UserRole();
                IList<string> list1 = await userManager.GetRolesAsync(user);
                userNew.RoleName = list1.FirstOrDefault()!;
                userNew.Name = user.UserName!;
                list.Add(userNew);
            }
            return list;
        }

        public async Task<IList<string>> getRolesAsync(AppUser user)
        {
            return await userManager.GetRolesAsync(user);
        }

        public async Task<AppUser> findByEmailAsyn(string email)
        {
            Task<AppUser> task = userManager.FindByEmailAsync(email)!;
            return await task;
        }

        public async Task<AppUser> findByNameAsync(string userName)
        {
            Task<AppUser> task = userManager.FindByNameAsync(userName)!;
            return await task;
        }

        public async Task<IdentityResult> addClaim(AppUser user, Claim claim)
        {
            var claims = await userManager.GetClaimsAsync(user);
            var FullNameClaim= claims.Where(c => c.Type == "FullName").ToList();
            if (FullNameClaim.Any())
            {
                await userManager.RemoveClaimsAsync(user, FullNameClaim);
            }
            return await userManager.AddClaimAsync(user, claim);
        }

        public Task<IdentityResult> deleteUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<AppUser>> getUsersInRole(string roleName)
        {
            return await userManager.GetUsersInRoleAsync(roleName);
        }

        public async Task<IdentityResult> ifChangeRole(string userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (!await userManager.IsInRoleAsync(user!, roleName))
            {
                return await userManager.AddToRoleAsync(user!, roleName);
            }

            var resutl = new IdentityResult();
            resutl.Errors.ToList().Add(new IdentityError()
            {
                Code = string.Empty,
                Description = "Bu kullanıcı bu role sahiptir"
            });
            return resutl;
        }

        public async Task<(bool, IEnumerable<IdentityError>?)> ChangePasswordAsync(PasswordChangeViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id!);
            if (!await userManager.CheckPasswordAsync(user!, model.PasswordOld!))
            {

                return (false, new List<IdentityError>()
                {
                    new IdentityError() {Code=string.Empty, Description="Eski parolanız yanlış"}
                });
            }

            var ıdentityResult = await userManager.ChangePasswordAsync(user!, model.PasswordOld!, model.PasswordNew!);

            return (ıdentityResult.Succeeded, ıdentityResult.Errors);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(AppUser user)
        {
            return await userManager.GeneratePasswordResetTokenAsync(user);
        }

        public Task<IdentityResult> ResetPasswordAsync(AppUser user, string token, string password)
        {
            return userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<List<string>> GetAllEmailsAsync()
        {
            var emails = new List<string>();

            foreach (var item in await userManager.Users.ToListAsync())
            {
                emails.Add(item.Email!);
            }

            return emails;
        }

        public async Task<AppUser> GetUserAsync(ClaimsPrincipal user)
        {
            var u = await userManager.GetUserAsync(user);
            return u!;
        }

        public async Task<AppUser> GetUserByIdAsync(string userId)
        {
            var u = await userManager.FindByIdAsync(userId);
            return u!;
        }

        public async Task<List<AppUser>> GetUserInIsAvtiveClass()
        {
            return await userManager.Users.Where(u=>u.IsActive).ToListAsync();
        }
        public async Task<IdentityResult> changeIsActive(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.IsActive = !user.IsActive;
                return await userManager.UpdateAsync(user);
            }
            var resutl = new IdentityResult();
            resutl.Errors.ToList().Add(new IdentityError()
            {
                Code = string.Empty,
                Description = "Kullanıcı bulunamadı"
            });
            return resutl;
        }
    }
}
