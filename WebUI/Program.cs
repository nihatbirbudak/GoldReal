using GR.Core.Entities.Identity;
using GR.Core.Interface;
using GR.Infrastructure.Data;
using GR.Infrastructure.Repositories;
using GR.Services.Abstract;
using GR.Services.Abstract.HomeService;
using GR.Services.Abstract.PropertyServiceFolder;
using GR.Services.Base;
using GR.Services.Mapping;
using GR.Services.SeedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebUI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



//****************    Database Connetion   ***************************************************
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"), options =>
    {
        options.MigrationsAssembly("GR.Infrastructure");
    });
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
    }
});

// Identity ayarlarý (Password, Lockout vb.) StartUpExtensions üzerinden
builder.Services.AddIdentityWithExt();
builder.Services.AddScoped<DbContext, AppDbContext>();

//****************    Database Connetion End   ***************************************************

// Email and Google-ReCaptcha Configuration with appsettings data




//******   Dependecy Injection    Use WebUI Extentions/StarUpExtensions  ******
builder.Services.AddDependencyInjection();

builder.Services.ConfigureApplicationCookie(opt =>
{
    var cookieBuilder = new CookieBuilder();

    cookieBuilder.Name = "AppCookie";
    opt.LoginPath = "/Auth/SingIn";
    opt.LogoutPath = "/Auth/Logout";
    opt.AccessDeniedPath = "/Auth/AccessDenied";
    opt.ReturnUrlParameter = "returnUrl";
    opt.Cookie = cookieBuilder;
    opt.ExpireTimeSpan = TimeSpan.FromDays(1);
    opt.SlidingExpiration = true;
});





var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Initialize the database and seed data
        var dbContext = services.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
        // Seed roles and users
        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
        await Seeder.AddRole(roleManager);
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        await Seeder.AddUser(userManager);
        // Seed home sections
        var homeSectionService = services.GetRequiredService<IHomeSectionService>();
        await Seeder.AddHomeSections(homeSectionService);
        // Seed home banners
        var homeBannerService = services.GetRequiredService<IHomeBannerService>();
        await Seeder.AddHomeBanners(homeBannerService);
        // Seed property types
        var propertyTypeService = services.GetRequiredService<IPropertyTypeService>();
        await Seeder.AddPropertyTypes(propertyTypeService);
        // Seed Home Contact
        var homeContactService = services.GetRequiredService<IHomeContactService>();
        await Seeder.AddHomeContact(homeContactService);
        // Seed Counter
        var counterService = services.GetRequiredService<IHomeCounterService>();
        await Seeder.AddHomeCounter(counterService);
        // Seed Customer Reviews
        var customerReviewService = services.GetRequiredService<ICustomerReviewService>();
        await Seeder.AddCustomerReviews(customerReviewService);
        // Seed Property Categories
        var propertyCategoryService = services.GetRequiredService<IPropertyCategoryService>();
        await Seeder.AddPropertyCategory(propertyCategoryService);
        // Seed Transaction Types
        var transactionTypeService = services.GetRequiredService<ITransactionTypeService>();
        await Seeder.AddTransactionType(transactionTypeService);
    }
    catch (Exception ex)
    {
        // Handle exceptions during seeding
        Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
