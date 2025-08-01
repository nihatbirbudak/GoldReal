using GR.Core.Entities.Identity;
using GR.Models.Entities;
using GR.Models.Entities.Home_Entities;
using GR.Services.Abstract;
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

        public static async Task AddPropertyTypes(IPropertyTypeService propertyTypeService)
        {
            var propertyTypes = await propertyTypeService.GetAllAsync();
            if (!propertyTypes.Any())
            {
                await propertyTypeService.AddAsync(new PropertyType { Name = "Daire" });
                await propertyTypeService.AddAsync(new PropertyType { Name = "Villa" });
                await propertyTypeService.AddAsync(new PropertyType { Name = "Arsa" });
                await propertyTypeService.AddAsync(new PropertyType { Name = "Ofis" });
                await propertyTypeService.AddAsync(new PropertyType { Name = "Tarla" });
                await propertyTypeService.AddAsync(new PropertyType { Name = "Fabrika/Depo" });
            }
        }

        public static async Task AddHomeContact(IHomeContactService homeContactService)
        {
            var contacts = await homeContactService.GetAllAsync();
            if (!contacts.Any())
            {
                await homeContactService.AddAsync(new HomeContact
                {
                    Title = "Hayalinizdeki Villaya Ulaşmanın En Kısa Yolu!",
                    Description = "Villa satın almak ya da mevcut villanızı satmak istiyorsunuz ama nereden başlayacağınızı bilmiyor musunuz? Siz hayalinize odaklanın, biz 10 yılı aşkın tecrübemizle bütçenize ve ihtiyaçlarınıza en uygun çözümleri bulalım.",
                    Title2 = "Satın Alma ve Satış Sürecinde Güvenilir Rehberiniz",
                    Description2 = "Villa almak, yalnızca bir mülk sahibi olmak değil; aynı zamanda yaşam tarzınızı değiştirmek demektir. Ancak bu süreçte doğru lokasyonu bulmak, bütçenize uygun fiyatlandırma yapmak ve yasal süreçleri sorunsuz yönetmek kolay değildir. Biz, yılların verdiği deneyimle ihtiyaçlarınızı analiz eder, size en uygun villayı en güvenilir şekilde sunarız.",
                    title3 = "Doğru Yatırım İçin Doğru Adres",
                    Description3 = "Yeni bir villa satın almak mı istiyorsunuz? Sizin için onlarca seçeneği değerlendirir, piyasayı analiz eder ve bütçenize en uygun fırsatları belirleriz. Üstelik sadece evi değil, yaşamak istediğiniz hayatı da birlikte tasarlarız.",

                });
            }
        }
    }
}
    

