using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScioBlazor.Components;
using ScioBlazor.Components.Account;
using ScioBlazor.Data;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authentication;
using ScioBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();
builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// SendGrid booking notifications
builder.Services.Configure<SendGridOptions>(builder.Configuration.GetSection("Email:SendGrid"));
builder.Services.AddSingleton<IBookingNotificationService, SendGridBookingNotificationService>();

// Name declension API
builder.Services.Configure<NameDeclensionOptions>(builder.Configuration.GetSection("Names:SklonovaniJmen"));
builder.Services.AddHttpClient<INameDeclensionService, SklonovaniJmenService>();

// External authentication providers (Google)
builder.Services
    .AddAuthentication()
    .AddGoogle(options =>
    {
        var googleSection = builder.Configuration.GetSection("Authentication:Google");
        options.ClientId = googleSection["ClientId"] ?? string.Empty;
        options.ClientSecret = googleSection["ClientSecret"] ?? string.Empty;
        // Map standard Google profile claims to .NET claim types
        options.ClaimActions.MapJsonKey(System.Security.Claims.ClaimTypes.GivenName, "given_name");
        options.ClaimActions.MapJsonKey(System.Security.Claims.ClaimTypes.Surname, "family_name");
        // Default callback is "/signin-google" which matches typical Google OAuth setup
        // options.CallbackPath = "/signin-google";
    });

var app = builder.Build();

// Force Czech culture for the entire app (dates, numbers, UI)
var cs = new CultureInfo("cs-CZ");
var locOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cs),
    SupportedCultures = new List<CultureInfo> { cs },
    SupportedUICultures = new List<CultureInfo> { cs }
};
app.UseRequestLocalization(locOptions);
CultureInfo.DefaultThreadCurrentCulture = cs;
CultureInfo.DefaultThreadCurrentUICulture = cs;

// Apply pending EF Core migrations at startup (dev-friendly)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // First, apply EF Core migrations and fail loudly in Development
    try
    {
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Failed to apply EF Core migrations at startup");
        if (app.Environment.IsDevelopment()) throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
