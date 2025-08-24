using CourseEnrollBlazorServer.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===== Add Services =====

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("SchoolDb").EnableSensitiveDataLogging());

// Add Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Razor Pages & Blazor
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Custom Authentication
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthentication>();
builder.Services.AddScoped<CustomAuthentication>();
builder.Services.AddAuthorizationCore();
builder.Services.AddAuthorization();

var app = builder.Build();

// ===== Seed Database =====
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    if (!db.Courses.Any())
    {
        db.Courses.AddRange(
            new Course { CourseId = 1, Title = "Mathematics", Description = "Numbers, algebra, and calculus" },
            new Course { CourseId = 2, Title = "Science and Space", Description = "Physics, chemistry, biology and astronomy" },
            new Course { CourseId = 3, Title = "History of UK", Description = "World history and events" },
            new Course { CourseId = 4, Title = "Literature", Description = "Fiction, modern drama" }
        );
        db.SaveChanges();
    }

    if (!db.Enrollments.Any())
    {
        db.Enrollments.AddRange(
            new Enrollment { StudentId = 1, CourseId = 1 },
            new Enrollment { StudentId = 1, CourseId = 2 },
            new Enrollment { StudentId = 2, CourseId = 3 },
            new Enrollment { StudentId = 3, CourseId = 4 }
        );
        db.SaveChanges();
    }
}

// ===== Configure Middleware =====
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host"); // Only one fallback

// Redirect root ("/") to login
app.MapGet("/", context =>
{
    context.Response.Redirect("/login");
    return Task.CompletedTask;
});

app.Run();
