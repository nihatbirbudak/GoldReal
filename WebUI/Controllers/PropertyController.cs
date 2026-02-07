using GR.Core.Entities.Identity;
using GR.Models.Entities.Property;
using GR.Models.ViewModels.PropertyViewModelFolder;
using GR.Services.Abstract.Auth;
using GR.Services.Abstract.PropertyServiceFolder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class PropertyController : Controller
    {
        private readonly ICityService cityService;
        private readonly IPropertyService propertyService;
        private readonly IDistrictService districtService;
        private readonly INeighborhoodService neighborhoodService;
        private readonly IPropertyCategoryService propertyCategoryService;
        private readonly IPropertyPhotosService propertyPhotosService;
        private readonly IPropertySubtypeService propertySubtypeService;
        private readonly ITransactionTypeService transactionTypeService;
        private readonly IAppUserService appUserService;
        private Task<AppUser> CurrentUser() => appUserService.GetUserAsync(HttpContext.User);

        public PropertyController(ICityService cityService, 
            IPropertyService propertyService, 
            IDistrictService districtService,
            INeighborhoodService neighborhoodService,
            IPropertyCategoryService propertyCategoryService,
            IPropertyPhotosService propertyPhotosService,
            IPropertySubtypeService propertySubtypeService,
            ITransactionTypeService transactionTypeService,
            IAppUserService appUserService)
        {
            this.cityService = cityService;
            this.propertyService = propertyService;
            this.districtService = districtService;
            this.neighborhoodService = neighborhoodService;
            this.propertyCategoryService = propertyCategoryService;
            this.propertyPhotosService = propertyPhotosService;
            this.propertySubtypeService = propertySubtypeService;
            this.transactionTypeService = transactionTypeService;
            this.appUserService = appUserService;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var mv = new PropertyAddViewModel();
            mv.Cities = (await cityService.GetAllAsync()).ToList();
            mv.PropertyCategories = (await propertyCategoryService.GetAllAsync()).ToList();
            mv.TransactionTypes = (await transactionTypeService.GetAllAsync()).ToList();
            mv.Users = await appUserService.getAll();
            mv.Owner = await CurrentUser();
            return View(mv);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PropertyAddViewModel vm)
        {
            // … City/District/Neighborhood uyum doğrulamaları + ModelState …
            if (!ModelState.IsValid)
            {
                // lookup’ları doldur (kısaltıldı)
                vm.Cities = (await cityService.GetAllAsync()).ToList();
                vm.PropertyCategories = (await propertyCategoryService.GetAllAsync()).ToList();
                vm.TransactionTypes = (await transactionTypeService.GetAllAsync()).ToList();
                vm.Users = await appUserService.getAll();
                vm.Owner = await CurrentUser();
                return View(vm);
            }

            var currentUser = await CurrentUser();
            if (currentUser is null) return Challenge();

            string ownerId = currentUser.Id;
            if (User.IsInRole("Admin") && !string.IsNullOrWhiteSpace(vm.SelectedOwnerId))
            {
                // Güvenlik: seçilen kullanıcı gerçekten Agent/Admin mi?
                var isValidOwner = await (appUserService.getRolesAsync(await appUserService.GetUserByIdAsync(vm.SelectedOwnerId)))
                    .ContinueWith(t => t.Result.Intersect(new[] { "Admin", "User" }).Any());

                if (!isValidOwner)
                {
                    ModelState.AddModelError(nameof(vm.SelectedOwnerId), "Seçilen kullanıcı yetkili değil.");
                    vm.Cities = (await cityService.GetAllAsync()).ToList();
                    vm.PropertyCategories = (await propertyCategoryService.GetAllAsync()).ToList();
                    vm.TransactionTypes = (await transactionTypeService.GetAllAsync()).ToList();
                    vm.Users = await appUserService.getAll();
                    vm.Owner = await CurrentUser();
                    return View(vm);
                }
                ownerId = vm.SelectedOwnerId!;
            }

            var entity = new Property
            {
                // … diğer alanlar …
                CityId = vm.CityId,
                DistrictId = vm.DistrictId,
                NeighborhoodId = vm.NeighborhoodId,
                TransactionTypeId = vm.TransactionTypeId,
                Title = vm.Title,
                Description = vm.Description,
                Price = vm.Price,
                Currency = vm.Currency,
                GrossM2 = vm.GrossM2,
                NetM2 = vm.NetM2,
                RoomPlan = vm.RoomPlan,
                CategoryId = vm.CategoryId,
                SubtypeId = vm.SubtypeId,
                Floor = vm.Floor,
                TotalFloors = vm.TotalFloor,
                BuildingAge = vm.Age,
                BathroomCount = vm.BathroomCount,
                AddressLine = vm.AddressLine,
                AddressNote = vm.AddressNote,
                OwnerId = ownerId,
                CreatedAt = DateTime.UtcNow
            };

            var createdEntity = await propertyService.AddAsync(entity);
            return RedirectToAction("List","Property");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = new PropertyAddViewModel();
            vm.Cities = (await cityService.GetAllAsync()).ToList();
            vm.PropertyCategories = (await propertyCategoryService.GetAllAsync()).ToList();
            vm.TransactionTypes = (await transactionTypeService.GetAllAsync()).ToList();
            vm.Users = await appUserService.getAll();
            vm.Owner = await CurrentUser();
            var property = await propertyService.GetByIdAsync(id);
            if (property == null) return NotFound();

            vm.Id = property.Id;
            vm.CityId = property.CityId;
            vm.DistrictId = property.DistrictId;
            vm.NeighborhoodId = property.NeighborhoodId;
            vm.TransactionTypeId = property.TransactionTypeId;
            vm.Title = property.Title;
            vm.Description = property.Description;
            vm.Price = property.Price ?? 0;
            vm.Currency = property.Currency!;
            vm.GrossM2 = property.GrossM2 ?? 0;
            vm.NetM2 = property.NetM2;
            vm.RoomPlan = property.RoomPlan;
            vm.CategoryId = property.CategoryId;
            vm.SubtypeId = property.SubtypeId;
            vm.Floor = property.Floor;
            vm.TotalFloor = property.TotalFloors;
            vm.Age = property.BuildingAge;
            vm.BathroomCount = property.BathroomCount;
            vm.AddressLine = property.AddressLine;
            vm.AddressNote = property.AddressNote;
            vm.SelectedOwnerId = property.OwnerId;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PropertyAddViewModel vm)
        {
            // … City/District/Neighborhood uyum doğrulamaları + ModelState …
            if (!ModelState.IsValid)
            {
                // lookup’ları doldur (kısaltıldı)
                vm.Cities = (await cityService.GetAllAsync()).ToList();
                vm.PropertyCategories = (await propertyCategoryService.GetAllAsync()).ToList();
                vm.TransactionTypes = (await transactionTypeService.GetAllAsync()).ToList();
                vm.Users = await appUserService.getAll();
                vm.Owner = await CurrentUser();
                return View(vm);
            }

            var entity = new Property
            {
                // … diğer alanlar …
                Id = vm.Id,
                CityId = vm.CityId,
                DistrictId = vm.DistrictId,
                NeighborhoodId = vm.NeighborhoodId,
                TransactionTypeId = vm.TransactionTypeId,
                Title = vm.Title,
                Description = vm.Description,
                Price = vm.Price,
                Currency = vm.Currency,
                GrossM2 = vm.GrossM2,
                NetM2 = vm.NetM2,
                RoomPlan = vm.RoomPlan,
                CategoryId = vm.CategoryId,
                SubtypeId = vm.SubtypeId,
                Floor = vm.Floor,
                TotalFloors = vm.TotalFloor,
                BuildingAge = vm.Age,
                BathroomCount = vm.BathroomCount,
                AddressLine = vm.AddressLine,
                AddressNote = vm.AddressNote,
                OwnerId = vm.SelectedOwnerId,
                UpdatedAt = DateTime.UtcNow
            };

            var createdEntity = await propertyService.UpdateAsync(entity);
            return RedirectToAction("List", "Property");
        }


        public async Task<IActionResult> List(int page = 1, int pageSize = 10)
        {
            var currentUser = await CurrentUser();

            var result = await propertyService.GetPageAsync(page, pageSize, currentUser.Id);

            var vm = new PropertyListViewModel
            {
                Properties = result.Items,
                CurrentPage = result.Page,
                TotalPages = result.TotalPages
            };

            vm.Photos = await propertyPhotosService.GetCurrentCovers(page, pageSize);

            return View(vm);
        }

        
        public async Task<IActionResult> ListAll(int page = 1, int pageSize = 10)
        {
            var currentUser = await CurrentUser();

            var result = await propertyService.GetPageAsync(page, pageSize);

            var vm = new PropertyListViewModel
            {
                Properties = result.Items,
                CurrentPage = result.Page,
                TotalPages = result.TotalPages
            };

            vm.Photos = await propertyPhotosService.GetCurrentCovers(page, pageSize);

            return View(vm);
        }

        [HttpGet] // İstersen [HttpPost] yapıp aynı mantığı form ile de kullanabilirsin
        [ActionName("IsActive")] // olası isim çakışmalarına karşı güvenli
        public async Task<IActionResult> IsActive(int id, [FromQuery] string? returnUrl)
        {
            var p = await propertyService.GetByIdAsync(id);
            if (p is null) return NotFound();

            p.IsActive = !p.IsActive;
            await propertyService.UpdateAsync(p);

            // 1) returnUrl geldiyse ve yerelse, oraya dön
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            // 2) aksi halde Referer (geldiğin sayfa) yerelse, oraya dön
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrWhiteSpace(referer) && Url.IsLocalUrl(referer))
                return Redirect(referer);

            // 3) son çare: List
            return RedirectToAction("List", "Property");
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int id, string? returnUrl)
        {
            // 1) İlanın fotoğraflarını çek
            var photos = await propertyPhotosService.GetPhotosByPropertyIdAsync(id);
            if (photos != null)
            {
                foreach (var ph in photos)
                {
                    // 1.a) Diskteki dosyayı sil
                    TryDeleteFileByUrl(ph.Url);

                    // 1.b) DB kaydını sil (istersen bunu atlayıp cascade’e bırakabilirsin)
                    await propertyPhotosService.DeleteAsync(ph.Id);
                }
            }

            // 2) İlanı sil
            await propertyService.DeleteAsync(id);

            // 3) Geri dön (geldiğin yere)
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrWhiteSpace(referer) && Url.IsLocalUrl(referer))
                return Redirect(referer);

            return RedirectToAction("List", "Property");
        }

        // Diske kayıtlı URL'den fiziksel yolu bulup siler
        private void TryDeleteFileByUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url)) return;

            // url -> "/assets/img/PropertyPhotos/xxxx.jpg" formatında
            var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var relative = url.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(webRoot, relative);

            try
            {
                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
            }
            catch
            {
                // burada loglayabilirsin; silme başarısız olsa da akışı bozma
            }
        }


        [HttpGet("/properties/{id:int}/photos/list")]
        public async Task<IActionResult> PhotosList(int id)
        {
            var items = (await propertyPhotosService.GetPhotosByPropertyIdAsync(id))
                .OrderBy(x => x.SortOrder).ThenBy(x => x.Id)
                .Select(x => new { x.Id, x.Url, x.IsCover })
                .ToList();

            return Json(items);
        }

        // Yönetim sayfası
        [HttpGet("/properties/{id:int}/photos")]
        public async Task<IActionResult> Manage(int id)
        {
            var prop = await propertyService.GetByIdAsync(id);
            if (prop is null) return NotFound();

            ViewBag.PropertyTitle = prop.Title;
            ViewBag.MaxCount = 20;
            return View("Manage", model: id); // model => propertyId
        }
        [HttpGet]
        public async Task<IActionResult> IsSoldListAll(int id)
        {
            var p = await propertyService.GetByIdAsync(id);
            if (p is null) return NotFound();
            p.IsSold = !p.IsSold;
            await propertyService.UpdateAsync(p);
            return RedirectToAction("ListAll", "Property");
        }
        [HttpGet]
        public async Task<IActionResult> IsSold(int id)
        {
            var p = await propertyService.GetByIdAsync(id);
            if (p is null) return NotFound();
            p.IsSold = !p.IsSold;
            await propertyService.UpdateAsync(p);
            return RedirectToAction("List", "Property");
        }

        // Yükleme
        [HttpPost("/properties/{id:int}/photos/upload")]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(50_000_000)]
        public async Task<IActionResult> Upload(int id, IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("Dosya boş.");
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowed.Contains(ext)) return BadRequest("Sadece JPG/PNG/WebP yükleyiniz.");
            if (file.Length > 10 * 1024 * 1024) return BadRequest("Maksimum 10MB.");

            // MAX 20 kontrolü
            var count = await propertyPhotosService.CurentCounter(id);
            if (count >= 20) return BadRequest("Maksimum 20 fotoğraf yüklenebilir.");

            bool isFirst = count == 0;
            int maxSort = await propertyPhotosService.MaxCounter(id); // hiç yoksa 0 döndürsün
            int nextSort = maxSort + 1;

            // Dosyayı kaydet
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var saveDir = Path.Combine(webRoot, "assets", "img", "PropertyPhotos");
            Directory.CreateDirectory(saveDir);
            var diskPath = Path.Combine(saveDir, fileName);

            await using (var fs = System.IO.File.Create(diskPath))
                await file.CopyToAsync(fs);

            var relUrl = $"/assets/img/PropertyPhotos/{fileName}";

            // DB kaydı
            var photo = new PropertyPhoto
            {
                PropertyId = id,
                Url = relUrl,
                IsCover = isFirst,   // ilk foto ise kapak
                SortOrder = nextSort
            };
            await propertyPhotosService.AddAsync(photo);

            // UI bu alanları kullanmıyor ama isterseniz döndürüyoruz
            return Json(new { id = photo.Id, url = photo.Url, isCover = photo.IsCover });
        }

        // Kapak yap
        [HttpPost("/properties/{id:int}/photos/cover")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetCover(int id, int photoId)
        {
            var selected = await propertyPhotosService.GetByPropertyIdAndPhotoId(id, photoId)!;
            if (selected is null) return NotFound();
            if (selected.IsCover) return Ok();

            var currentCover = await propertyPhotosService.GetCurrentCover(id)!;
            if (currentCover != null)
            {
                currentCover.IsCover = false;
                await propertyPhotosService.UpdateAsync(currentCover);
            }

            selected.IsCover = true;
            await propertyPhotosService.UpdateAsync(selected);
            return Ok();
        }

        // Sil
        [HttpPost("/properties/{id:int}/photos/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePhoto(int id, int photoId)
        {
            var ph = await propertyPhotosService.GetDeleted(id, photoId);
            if (ph is null) return NotFound();
            if (ph.IsCover) return BadRequest("Kapak fotoğrafı silinemez. Önce başka bir fotoğrafı kapak yapın.");

            // ph.Url şu an /assets/img/PropertyPhotos/xxx.jpg
            var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var diskPath = Path.Combine(webRoot, ph.Url.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (System.IO.File.Exists(diskPath)) System.IO.File.Delete(diskPath);

            await propertyPhotosService.DeleteAsync(ph.Id);
            return Ok();
        }
    }
}
