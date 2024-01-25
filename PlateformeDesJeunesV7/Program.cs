using DinkToPdf;
using DinkToPdf.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Data;
using Rotativa.AspNetCore;
using Service;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString(builder.Environment.IsProduction() ? "SQLProd" : "SQLDev");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
                Config =>
                {
                    Config.SignIn.RequireConfirmedEmail = true;
                }
            )
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
 {
     options.LoginPath = "/Authentication/Login";
     options.LogoutPath = "/Authentication/logout";
     options.AccessDeniedPath = "/Authentication/AccessDenied";
 });

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddAuthentication().AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
});
builder.Services.ConfigureApplicationCookie(options =>
{
    // Définissez ici la durée de la session souhaitée en minutes
    options.ExpireTimeSpan = TimeSpan.FromDays(30); // Par exemple, 30 minutes
    options.SlidingExpiration = true; // La session se prolonge à chaque requête
});
// Dans la méthode `ConfigureServices` de votre Startup.cs
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    // Désactiver la déconnexion automatique
    options.ValidationInterval = TimeSpan.FromDays(365);
});

// services
builder.Services.AddServices(builder.Configuration);
// repository and db context
builder.Services.AddRepositories(builder.Configuration);

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

builder.Services.AddMvc();

RotativaConfiguration.Setup(builder.Environment.WebRootPath, "Rotativa");

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Candidats}/{action=Index}/{id?}");

app.Run();
