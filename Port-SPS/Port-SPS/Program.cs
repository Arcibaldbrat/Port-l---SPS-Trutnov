using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Port_SPS.Data;
using Port_SPS.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
    ?? "Data Source=portal.db";
var databaseProvider = builder.Configuration["DatabaseProvider"] ?? "Sqlite";

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (databaseProvider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlServer(connectionString);
    }
    else
    {
        options.UseSqlite(connectionString);
    }
});
builder.Services.AddScoped<PasswordHasher<AppUser>>();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "PortSps.Auth";
        options.LoginPath = "/login.html";
        options.AccessDeniedPath = "/login.html";
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await SeedDatabaseAsync(scope.ServiceProvider);
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/api/auth/login", async (
    LoginRequest request,
    AppDbContext db,
    PasswordHasher<AppUser> passwordHasher,
    HttpContext httpContext) =>
{
    var login = request.Username.Trim().ToLowerInvariant();
    var user = await db.Users.FirstOrDefaultAsync(item =>
        item.Username.ToLower() == login || item.Email.ToLower() == login);

    if (user is null)
    {
        return Results.Unauthorized();
    }

    var verification = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
    if (verification == PasswordVerificationResult.Failed)
    {
        return Results.Unauthorized();
    }

    var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new(ClaimTypes.Name, user.Username),
        new(ClaimTypes.Email, user.Email),
        new(ClaimTypes.GivenName, user.FirstName),
        new(ClaimTypes.Surname, user.LastName),
        new(ClaimTypes.Role, user.Role)
    };

    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    var principal = new ClaimsPrincipal(identity);
    var properties = new AuthenticationProperties
    {
        IsPersistent = request.RememberMe,
        ExpiresUtc = request.RememberMe ? DateTimeOffset.UtcNow.AddDays(14) : null
    };

    await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);

    return Results.Ok(ToResponse(user));
});

app.MapPost("/api/auth/register", async (
    RegisterRequest request,
    AppDbContext db,
    PasswordHasher<AppUser> passwordHasher) =>
{
    var role = UserRoles.IsValid(request.Role) ? request.Role : UserRoles.Student;
    var username = request.Username.Trim();
    var email = request.Email.Trim();

    if (username.Length < 3 || request.Password.Length < 8)
    {
        return Results.BadRequest(new { message = "Uživatelské jméno musí mít aspoň 3 znaky a heslo aspoň 8 znaků." });
    }

    var exists = await db.Users.AnyAsync(user => user.Username == username || user.Email == email);
    if (exists)
    {
        return Results.Conflict(new { message = "Uživatel se stejným jménem nebo emailem už existuje." });
    }

    var user = new AppUser
    {
        Username = username,
        Email = email,
        FirstName = request.FirstName.Trim(),
        LastName = request.LastName.Trim(),
        Role = role,
        ClassName = string.IsNullOrWhiteSpace(request.ClassName) ? null : request.ClassName.Trim()
    };
    user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/api/users/{user.Id}", ToResponse(user));
});

app.MapPost("/api/auth/logout", async (HttpContext httpContext) =>
{
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.NoContent();
});

app.MapGet("/api/auth/me", async (ClaimsPrincipal principal, AppDbContext db) =>
{
    var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);
    if (id is null || !int.TryParse(id, out var userId))
    {
        return Results.Unauthorized();
    }

    var user = await db.Users.FindAsync(userId);
    return user is null ? Results.Unauthorized() : Results.Ok(ToResponse(user));
}).RequireAuthorization();

app.MapGet("/api/users", async (AppDbContext db) =>
{
    var users = await db.Users
        .OrderBy(user => user.Role)
        .ThenBy(user => user.LastName)
        .Select(user => new UserResponse(
            user.Id,
            user.Username,
            user.Email,
            user.FirstName,
            user.LastName,
            user.Role,
            user.ClassName))
        .ToListAsync();

    return Results.Ok(users);
}).RequireAuthorization(policy => policy.RequireRole(UserRoles.Teacher, UserRoles.Admin));

app.Run();

static UserResponse ToResponse(AppUser user)
{
    return new UserResponse(
        user.Id,
        user.Username,
        user.Email,
        user.FirstName,
        user.LastName,
        user.Role,
        user.ClassName);
}

static async Task SeedDatabaseAsync(IServiceProvider services)
{
    var db = services.GetRequiredService<AppDbContext>();
    var passwordHasher = services.GetRequiredService<PasswordHasher<AppUser>>();

    await db.Database.EnsureCreatedAsync();

    if (await db.Users.AnyAsync())
    {
        return;
    }

    var demoUsers = new[]
    {
        new AppUser
        {
            Username = "student",
            Email = "student@sps.local",
            FirstName = "Demo",
            LastName = "Žák",
            Role = UserRoles.Student,
            ClassName = "2.IT"
        },
        new AppUser
        {
            Username = "ucitel",
            Email = "ucitel@sps.local",
            FirstName = "Demo",
            LastName = "Učitel",
            Role = UserRoles.Teacher
        },
        new AppUser
        {
            Username = "admin",
            Email = "admin@sps.local",
            FirstName = "Portál",
            LastName = "Admin",
            Role = UserRoles.Admin
        }
    };

    foreach (var user in demoUsers)
    {
        var password = user.Role switch
        {
            UserRoles.Student => "Student123!",
            UserRoles.Teacher => "Teacher123!",
            _ => "Admin123!"
        };

        user.PasswordHash = passwordHasher.HashPassword(user, password);
        db.Users.Add(user);
    }

    await db.SaveChangesAsync();
}
