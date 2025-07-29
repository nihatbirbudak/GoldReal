using GR.Core.Entities.Identity;
using GR.Core.Interface;
using GR.Infrastructure.Data;
using GR.Infrastructure.Repositories;
using GR.Services.Mapping;
using Microsoft.AspNetCore.Identity;

namespace WebUI.Extensions
{
    public static class StartUpExtensions
    {
        public static void AddIdentityWithExt(this IServiceCollection service)
        {
            service.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(3);
            });

            service.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
            }).AddDefaultTokenProviders()
                .AddEntityFrameworkStores<AppDbContext>();
        }


        public static void AddDependencyInjection(this IServiceCollection service)
        {
            // UnitOfWork & Repository
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            // AutoMapper
            service.AddAutoMapper(typeof(AutoMapperProfile));
        }

    }
}
