using GR.Core.Entities.Identity;
using GR.Core.Interface;
using GR.Infrastructure.Data;
using GR.Infrastructure.Repositories;
using GR.Services.Base;
using GR.Services.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
    options.EnableSensitiveDataLogging();
});

builder.Services.AddIdentity<AppUser, AppRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddScoped<DbContext, AppDbContext>();

//****************    Database Connetion End   ***************************************************

// Email and Google-ReCaptcha Configuration with appsettings data




//******   Dependecy Injection    Use WebUI Extentions/StarUpExtensions  ******
builder.Services.AddDependencyInjection();

builder.Services.ConfigureApplicationCookie(opt =>
{
    var cookieBuilder = new CookieBuilder();

    cookieBuilder.Name = "AppCookie";
    opt.LoginPath = new PathString("/Admin/Auth/SingIn");
    opt.LogoutPath = new PathString("/Admin/Auth/logout");
    opt.AccessDeniedPath = new PathString("/Admin/Pages/Error500");
    opt.Cookie = cookieBuilder;
    opt.ExpireTimeSpan = TimeSpan.FromDays(1);
    opt.SlidingExpiration = true;
});




var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
