using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using TestingApp.Data;
using TestingApp.Models;
using TestingApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultUI()
        .AddDefaultTokenProviders();

// External Logins

var config = builder.Configuration;

builder.Services.AddAuthentication()
                .AddGoogle(opts =>
                {
                    IConfigurationSection section =
                    config.GetSection("Authentication:Google");
                    opts.ClientId = section["ClientId"];
                    opts.ClientSecret = section["ClientSecret"];
                });
                //.AddMicrosoftAccount(opts =>
                //{
                //    IConfigurationSection section =
                //    config.GetSection("Authentication:Microsoft");
                //    opts.ClientId = section["ClientId"];
                //    opts.ClientSecret = section["ClientSecret"];
                ////})
                //.AddFacebook(opts =>
                //{
                //    IConfigurationSection section =
                //    config.GetSection("Authentication:Facebook");
                //    opts.ClientId = section["ClientId"];
                //    opts.ClientSecret = section["ClientSecret"];
                //    opts.AccessDeniedPath = "/AccessDenied";
                //});


builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

// "en-US","de-AT", "de-DE", "fr-FR"

var cultures = new List<CultureInfo>()
{
    new CultureInfo("de"),
    new CultureInfo("en"),
    new CultureInfo("fr")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture(cultures[0].Name);
    options.SupportedUICultures = cultures;
    options.SupportedCultures = cultures;
    options.FallBackToParentUICultures = true;

    options.RequestCultureProviders.Remove(options
    .RequestCultureProviders
    .First(x => x.GetType() == typeof(AcceptLanguageHeaderRequestCultureProvider)));
});

builder.Services
    .AddRazorPages()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

builder.Services.AddScoped<RequestLocalizationCookiesMiddleware>();

builder.Services.AddSession();

var app = builder.Build();

app.UseRequestLocalization();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await ContextSeed.SeedRolesAsync(userManager, roleManager);
        await ContextSeed.SeedSuperAdminAsync(userManager, roleManager);
        await ContextSeed.SeedBasicUserAsync(userManager, roleManager);
        await ContextSeed.SeedArticlesAsync(context);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapDefaultControllerRoute();

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization();

app.UseRequestLocalizationCookies();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();