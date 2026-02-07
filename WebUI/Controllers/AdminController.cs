using GR.Core.Entities.Identity;
using GR.Models.Entities;
using GR.Models.Entities.Home_Entities;
using GR.Models.ViewModels;
using GR.Models.ViewModels.Admin;
using GR.Models.ViewModels.Auth;
using GR.Services.Abstract;
using GR.Services.Abstract.Auth;
using GR.Services.Abstract.HomeService;
using GR.Services.Services;
using GR.Services.Services.Home_Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Claims;
using WebUI.Extensions;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Admin,User")]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly IAppUserService userService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IAppRoleService roleService;
        private readonly ICustomerReviewService customerReviewService;
        private readonly IContactRequestService contactRequestService;

        // Home content services
        private readonly IHomeBannerService _bannerService;
        private readonly IHomeSectionService _sectionService;
        private readonly IHomeContactService _homeContactService;
        private readonly IHomeCounterService _homeCounterService;

        public AdminController(IAppUserService userService, SignInManager<AppUser> signInManager, IAppRoleService roleService,ICustomerReviewService customerReviewService,
            IContactRequestService contactRequestService,IHomeBannerService bannerService,
            IHomeSectionService sectionService,
            IHomeContactService homeContactService,
            IHomeCounterService homeCounterService)
        {
            this.userService = userService;
            this.signInManager = signInManager;
            this.roleService = roleService;
            this.customerReviewService = customerReviewService;
            this.contactRequestService = contactRequestService;
            _bannerService = bannerService;
            _sectionService = sectionService;
            _homeContactService = homeContactService;
            _homeCounterService = homeCounterService;

        }
        // Admin ana sayfa (index) — Views/Admin/Index.cshtml ile uyumlu
        [HttpGet("")]
        [HttpGet("anasayfa")]
        public async Task<IActionResult> Index()
        {
            var vm = new AdminHomeContentViewModel
            {
                Banners = await _bannerService.GetAllAsync(),
                Sections = await _section_service_safeguard(),
                HomeContact = await _homeContactService.GetAsync(),
                HomeCounter = await _homeCounterService.GetAsync(),
                CustomerReviews = await customerReviewService.GetAllAsync()
            };

            // View konumu Views/Admin/Index.cshtml
            return View("~/Views/Admin/Index.cshtml", vm);
        }

        // Helper to ensure null-safe GetAllAsync for sections (keeps code compact)
        private async Task<IEnumerable<HomeSection>> _section_service_safeguard()
        {
            var sections = await _sectionService.GetAllAsync();
            return sections ?? new List<HomeSection>();
        }

        // Her form ayrı ayrı post eder: route isimleri View'deki asp-action ile eşleşir.
        [HttpPost("UpdateBanner/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBanner(int id, string Title, string Description, string btnText, string btnLink, IFormFile ImageFile)
        {
            var banner = await _bannerService.GetByIdAsync(id);
            if (banner == null) return NotFound();

            banner.Title = Title ?? banner.Title;
            banner.Description = Description ?? banner.Description;
            banner.btnText = btnText ?? banner.btnText;
            banner.btnLink = btnLink ?? banner.btnLink;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                if (!ImageFile.ContentType.StartsWith("image/", System.StringComparison.OrdinalIgnoreCase))
                {
                    TempData["BannerError"] = "Lütfen bir resim dosyası yükleyin.";
                    return RedirectToAction(nameof(Index));
                }
                const long maxBytes = 4 * 1024 * 1024;
                if (ImageFile.Length > maxBytes)
                {
                    TempData["BannerError"] = "Dosya boyutu 4MB'ı geçemez.";
                    return RedirectToAction(nameof(Index));
                }

                var ext = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                if (!allowed.Contains(ext))
                {
                    TempData["BannerError"] = "Sadece .jpg, .jpeg, .png, .webp izinlidir.";
                    return RedirectToAction(nameof(Index));
                }

                var fileName = $"{Guid.NewGuid()}{ext}";
                var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var folder = Path.Combine(webRoot, "assets", "img", "banner");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                var path = Path.Combine(folder, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                // eski dosya sil (hata tolere edilir)
                try
                {
                    if (!string.IsNullOrWhiteSpace(banner.ImageUrl))
                    {
                        var old = banner.ImageUrl.TrimStart('/');
                        var oldPhysical = Path.Combine(webRoot, old.Replace('/', Path.DirectorySeparatorChar));
                        if (System.IO.File.Exists(oldPhysical)) System.IO.File.Delete(oldPhysical);
                    }
                }
                catch { }

                banner.ImageUrl = $"/assets/img/banner/{fileName}";
            }

            await _bannerService.UpdateAsync(banner);
            TempData["BannerSuccess"] = "Banner güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("UpdateSection/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSection(int id, string Title, string Description, string btnText, string btnLink)
        {
            var section = await _sectionService.GetByIdAsync(id);
            if (section == null) return NotFound();

            section.Title = Title ?? section.Title;
            section.Description = Description ?? section.Description;
            section.btnText = btnText ?? section.btnText;
            section.btnLink = btnLink ?? section.btnLink;

            await _sectionService.UpdateAsync(section);
            TempData["SectionSuccess"] = "Bölüm güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("UpdateContact")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateContact(int id, string Title, string Description, string Title2, string Description2, string title3, string Description3)
        {
            var contact = await _homeContactService.GetByIdAsync(id);
            if (contact == null) return NotFound();

            contact.Title = Title ?? contact.Title;
            contact.Description = Description ?? contact.Description;
            contact.Title2 = Title2 ?? contact.Title2;
            contact.Description2 = Description2 ?? contact.Description2;
            contact.title3 = title3 ?? contact.title3;
            contact.Description3 = Description3 ?? contact.Description3;

            await _homeContactService.UpdateAsync(contact);
            TempData["ContactSuccess"] = "İletişim bölümü güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("UpdateCounter")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCounter(int id, string Count1, string Description1, string Count2, string Description2, string Count3, string Description3, string Count4, string Description4)
        {
            var counter = await _homeCounterService.GetByIdAsync(id);
            if (counter == null) return NotFound();

            counter.Count1 = Count1 ?? counter.Count1;
            counter.Description1 = Description1 ?? counter.Description1;
            counter.Count2 = Count2 ?? counter.Count2;
            counter.Description2 = Description2 ?? counter.Description2;
            counter.Count3 = Count3 ?? counter.Count3;
            counter.Description3 = Description3 ?? counter.Description3;
            counter.Count4 = Count4 ?? counter.Count4;
            counter.Description4 = Description4 ?? counter.Description4;

            await _homeCounterService.UpdateAsync(counter);
            TempData["CounterSuccess"] = "Sayaçlar güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("UpdateReview/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateReview(int id, string Name, string Surname, string TransactionType, string Comment, double Rating)
        {
            var review = await customerReviewService.GetByIdAsync(id);
            if (review == null) return NotFound();

            review.Name = Name ?? review.Name;
            review.Surname = Surname ?? review.Surname;
            review.TransactionType = TransactionType ?? review.TransactionType;
            review.Comment = Comment ?? review.Comment;
            review.Rating = Rating;

            await customerReviewService.UpdateAsync(review);
            TempData["ReviewSuccess"] = "Müşteri yorumu güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("mesajlar")]
        public async Task<IActionResult> Messages(int page = 1, int pageSize = 20)
        {
            var result = await contactRequestService.GetPageAsync(page, pageSize);
            var vm = new ContactRequestViewModel
            {
                ContactRequests = result.Items,
                CurrentPage = result.Page,
                TotalPages = result.TotalPages
            };
            
            return View(vm);
        }

        [HttpGet("kullanıcılar")]
        public async Task<IActionResult> Users()
        {
            var UserListViewModel = new UserListViewModel()
            {
                Users = await userService.getAll(),
                userRoleList = await userService.getUsersRoleList(await userService.getAll())
            };

            return View(UserListViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("kullanıcı-ekle")]
        public async Task<IActionResult> AddUser()
        {
            var addUserViewModel = new AddUserViewModel()
            {
                appRoles = await roleService.gelAll(),
            };

            return View(addUserViewModel);
        }        

        [Authorize(Roles = "Admin")]
        [HttpPost("kullanıcı-ekle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(AddUserViewModel request)
        {
            var addUserViewModel = new AddUserViewModel()
            {
                appRoles = await roleService.gelAll(),
            };
            if (!ModelState.IsValid)
            {
                return View(addUserViewModel);
            }

            var identityResult = await userService.createAsync(request);

            if (!identityResult.Succeeded)
            {
                ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());
                return View(addUserViewModel);
            }

            if (request.PictureFile != null && request.PictureFile.Length > 0)
            {
                var saveResult = await SaveImageAsync(request.PictureFile, request.Email!);
                if (!saveResult.ok)
                {
                    ModelState.AddModelError("", saveResult.error!);
                    return RedirectToAction("UpdateUser", "Admin", userService.findByNameAsync(request.UserName!).Id);
                }
                request.PicturePath = saveResult.webPath;
            }

            await userService.addToRole(await userService.findByEmailAsyn(request.Email!), request.Role!);

            TempData["SuccessMessage"] = "Üyelik kayıt işlemi başarıla gerçekleşmiştir.";

            return RedirectToAction("Users", "Admin");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("kullanıcı-düzenle/{id}")]
        public async Task<IActionResult> UpdateUser(string id)
        {
            var updatedUser = await userService.findByIdAsync(id);
            IList<string> roleList = await userService.getRolesAsync(updatedUser);

            // Eğer daha sonra fazladan role ekmeke yaparsam bu andan liste olara gelen rolleri liste olarak döndürmem gerekicek...
            var viewModel = new UpdateUserViewModel()
            {
                UserName = updatedUser.UserName,
                Email = updatedUser.Email,
                Role = roleList.ToList().FirstOrDefault(),
                FullName = updatedUser.FullName,
                PicturePath = updatedUser.PicturePath,
                Name = updatedUser.Name,
                Surname = updatedUser.Surname,
                PhoneNumber = updatedUser.PhoneNumber,
                description = updatedUser.description,
                EmployeeStatus = updatedUser.EmployeeStatus,
            };
            viewModel.appRoles = await roleService.gelAll();
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("kullanıcı-düzenle/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UpdateUserViewModel request)
        {
            if (!ModelState.IsValid)
            {
                request.appRoles = await roleService.gelAll();
                return View(request);
            }

            var result = await userService.updateAsync(request);

            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors);
                request.appRoles = await roleService.gelAll();
                return View(request);
            }


            if (request.PictureFile != null && request.PictureFile.Length > 0)
            {
                var saveResult = await SaveImageAsync(request.PictureFile, request.Email!);
                if (!saveResult.ok)
                {
                    ModelState.AddModelError("", saveResult.error!);
                    return RedirectToAction("UpdateUser", "Admin", userService.findByNameAsync(request.UserName!).Id);
                }
                DeleteImage(request.PicturePath!);
                request.PicturePath = saveResult.webPath;
            }

            var vl = User.Claims.ToList().FirstOrDefault(x => x.Type == "FullName")!.Value;
            var hasUser = await userService.findByIdAsync(request.Id!);

            if (hasUser.FullName != vl)
            {
                await userService.addClaim(hasUser, new Claim("FullName", hasUser.FullName!));
                await signInManager.RefreshSignInAsync(hasUser);
            }

            TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
            return RedirectToAction("Users","Admin");
        }


        [HttpGet("aktiflestir/{id}")]
        public async  Task<IActionResult> ActiveUser(string id)
        {
            var result = await userService.changeIsActive(id);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı durumu güncellenemedi.");
            }
            return RedirectToAction("Users", "Admin");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("kullanıcı-sil/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var deletedUser = await userService.findByIdAsync(id);
            var userList = await userService.getUsersInRole("Admin");
            userList.Remove(deletedUser);
            if (userList.IsNullOrEmpty())
            {
                ModelState.AddModelError(string.Empty, "Başka bir Admin kullanıcı Olmadığı için bu kullanıcı silinemez.");
                return RedirectToAction("Users","Admin");
            }
            if (deletedUser.UserName == HttpContext.User.Identity!.Name)
            {
                ModelState.AddModelError(string.Empty, "Kendinizi Silemezsiniz");
                return RedirectToAction("Users", "Admin");
            }
            var resutl = await userService.deleteAsync(deletedUser);
            if (!resutl.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Silme işlemi şuan gerçekleştirilemiyor.");
                return RedirectToAction("Users", "Admin");
            }
            return RedirectToAction("Users", "Admin");
        }

        private async Task<(bool ok, string? webPath, string? error)> SaveImageAsync(IFormFile file, string email)
        {
            // Basit doğrulama
            if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                return (false, null, "Lütfen bir resim dosyası yükleyin.");

            const long maxBytes = 4 * 1024 * 1024; // 4MB
            if (file.Length > maxBytes)
                return (false, null, "Dosya boyutu 4MB'ı geçemez.");

            var extention = Path.GetExtension(file.FileName);
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowed.Contains(extention))
                return (false, null, "Sadece .jpg, .jpeg, .png, .webp dosyalarına izin verilir.");

            // Klasör & dosya adları
            
            var randomName = $"{Guid.NewGuid()}{extention}";
            var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var folder = Path.Combine(webRoot, "assets", "img", "UserPic");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            var path = Path.Combine(folder, randomName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var user = await userService.findByEmailAsyn(email);
            user.PicturePath = randomName;
            await userService.updateUserImagePathAsync(user);

            return (true, randomName, null);
        }
        private (bool ok, string? error) DeleteImage(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return (false, "Silinecek bir resim bulunamadı.");

            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "img", "UserPic", fileName);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Dosya silinirken bir hata oluştu: {ex.Message}");
            }
        }
    }
}
