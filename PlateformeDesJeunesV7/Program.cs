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
connectionString = "Data Source=52.117.100.168\\MSSQLSERVER2019;Initial Catalog=plateformedesjeunes-ainsebaa_ma_si;Persist Security Info=False;Integrated Security=false;User ID=plateformedesjeunes-ainsebaa_ma_si;pwd=Freeak-badr1;Connect Timeout=15;Encrypt=False;Packet Size=4096";
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

static void ApplyMigrations(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred applying migrations.");
        }
    }
}

ApplyMigrations(app);

static void CreateDbIfNotExists(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            DbInitializer.Initialize(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
}
CreateDbIfNotExists(app);

app.Run();
