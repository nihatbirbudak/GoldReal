using GR.Core.Entities.Identity;
using GR.Models.Entities;
using GR.Models.Entities.Home_Entities;
using GR.Services.Abstract.HomeService;
using GR.Services.Services.Home_Service;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Services.SeedData
{
    public class Seeder
    {
        // Seed data for roles
        public static async Task AddRole(RoleManager<AppRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new AppRole { Name = "Admin" });
            }
        }

        // Seed data for users
        public static async Task AddUser(UserManager<AppUser> userManager)
        {
            var hasUser = userManager.Users.ToList();
            if (!hasUser.Any())
            {
                await userManager.CreateAsync(new AppUser() { Email = "admin@gurdemirinsaat.com", UserName = "Admin" }, "Admin.1234");
                var user = await userManager.FindByNameAsync("Admin");
                await userManager.AddToRoleAsync(user!, "Admin");
            }
        }

        // Seed data for home sections and banners
        public static async Task AddHomeSections(IHomeSectionService homeSectionService)
        {
            var sections = await homeSectionService.GetAllAsync();
            if (!sections.Any())
            {
                await homeSectionService.AddAsync(new HomeSection
                {
                    Title = "15 Yıllık Sektör Tecrübesi",
                    Description = "İnşaat ve gayrimenkul sektöründeki 15 yıllık bilgi birikimimizle, her yatırımınızı güvenle yönlendiriyoruz.",
                    btnText = "Daha Fazla Bilgi",
                    btnLink = "/about"
                });
                await homeSectionService.AddAsync(new HomeSection
                {
                    Title = "Güvenilir ve Şeffaf Hizmet",
                    Description = "Müşteri memnuniyetini öncelik edinerek, tüm süreçlerde açık ve güvenilir bir danışmanlık sunuyoruz.",
                    btnText = "Satılık İlanları İncele",
                    btnLink = "/about"
                });
                await homeSectionService.AddAsync(new HomeSection
                {
                    Title = "Doğru Lokasyon ve Yatırım",
                    Description = "Sadece gelişme potansiyeli yüksek ve uzun vadede değer kazandıracak portföylerle sizi buluşturuyoruz.",
                    btnText = "Bize Ulaşın",
                    btnLink = "/about"
                });
                await homeSectionService.AddAsync(new HomeSection
                {
                    Title = "Uzun Vadeli Kazanç Odaklılık",
                    Description = "Amacımız kısa vadeli kazanç değil; size ve yatırımlarınıza değer katacak kalıcı çözümler üretmek.",
                    btnText = "Bize Ulaşın",
                    btnLink = "/about"
                });
            }
        }

        // Seed data for home banners
        public static async Task AddHomeBanners(IHomeBannerService homeBannerService)
        {
            var banners = await homeBannerService.GetAllAsync();
            if (!banners.Any())
            {
                await homeBannerService.AddAsync(new HomeBanner
                {
                    Title = "15 Yıllık Tecrübe ile Güvenli Yatırımlar",
                    Description = "Gold Real olarak sadece bir mülk değil, geleceğe değer katan yatırımlar sunuyoruz.",
                    ImageUrl = "/assets/img/banner/banner-1.jpg",
                    btnText = "Daha Fazla Bilgi",
                    btnLink = "/about"
                });
                await homeBannerService.AddAsync(new HomeBanner
                {
                    Title = "Hayalinizdeki Evi Bulun",
                    Description = "İzmir’in en değerli bölgelerinde kazançlı gayrimenkul fırsatları sizi bekliyor.",
                    ImageUrl = "/assets/img/banner/banner-2.jpg",
                    btnText = "Satılık İlanları İncele",
                    btnLink = "/about"
                });
                await homeBannerService.AddAsync(new HomeBanner
                {
                    Title = "Doğru Yatırım, Doğru Lokasyon",
                    Description = "Bütçenizi ve hedeflerinizi analiz ediyor, sizi en doğru yatırımla buluşturuyoruz.",
                    ImageUrl = "/assets/img/banner/banner-3.jpg",
                    btnText = "Bize Ulaşın",
                    btnLink = "/about"
                });
            }
        }


    }
}
