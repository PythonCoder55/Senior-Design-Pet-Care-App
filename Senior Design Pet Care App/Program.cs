using Senior_Design_Pet_Care_App.Components;
using Senior_Design_Pet_Care_App.Components.Pages;
using Senior_Design_Pet_Care_App.Services;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Senior_Design_Pet_Care_App.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Authentication
builder.Services.AddAuthorization();

//The cookie authentication is never used, but it is required to prevent a runtime error
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_cookie";
        options.Cookie.MaxAge = TimeSpan.FromHours(24);
    });

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddCascadingAuthenticationState();

// Add EF Core SQLite
var conn = builder.Configuration.GetConnectionString("PetCareDb") ?? "Data Source=PetCare.db";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(conn));

// Register AuthDataService (now async)
builder.Services.AddScoped<IAuthDataService, AuthDataService>();

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<ICustomSessionService, CustomSessionService>();

var app = builder.Build();

// Ensure database created (simple approach, you can use migrations instead)
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    ctx.Database.EnsureCreated();

    // Optional: seed admin/super accounts if missing (only for initial run)
    var pwHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Senior_Design_Pet_Care_App.Entities.User>();
    if (!ctx.Users.Any(u => u.Email.ToLower() == "admin@acme.com"))
    {
        var admin = new Senior_Design_Pet_Care_App.Entities.User
        {
            Email = "admin@acme.com",
            Role = "Admin",
            PasswordHash = pwHasher.HashPassword(null, "admin123") // change default password if desired
        };
        ctx.Users.Add(admin);
    }
    if (!ctx.Users.Any(u => u.Email.ToLower() == "super@acme.com"))
    {
        var super = new Senior_Design_Pet_Care_App.Entities.User
        {
            Email = "super@acme.com",
            Role = "Super",
            PasswordHash = pwHasher.HashPassword(null, "super123") // change default password if desired
        };
        ctx.Users.Add(super);
    }
    ctx.SaveChanges();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();