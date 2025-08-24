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

// Add Identity (with Roles support if needed)
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.SignIn.RequireConfirmedAccount = false; // optional
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders(); // required for email/password resets etc.

// Razor Pages & Blazor
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

// Authentication & Authorization
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthentication>();
builder.Services.AddScoped<CustomAuthentication>(); 
builder.Services.AddAuthentication();
builder.Services.AddAuthorizationCore();
builder.Services.AddAuthorization();

var app = builder.Build();

// ===== Seed Database =====
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    // Seed Courses
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

    // Seed Students + IdentityUsers
    if (!db.Students.Any())
    {
        var aliceUser = new IdentityUser { UserName = "aliceJ@school.com", Email = "aliceJ@school.com" };
        await userManager.CreateAsync(aliceUser, "Password123!");

        var bobUser = new IdentityUser { UserName = "bob@school.co.za", Email = "bob@school.co.za" };
        await userManager.CreateAsync(bobUser, "Password123!");

        var charlieUser = new IdentityUser { UserName = "charliebrown@gmail.com", Email = "charliebrown@gmail.com" };
        await userManager.CreateAsync(charlieUser, "Password123!");

        db.Students.AddRange(
            new Student { StudentId = 1, Name = "Alice Johnson", Email = "aliceJ@school.com", IdentityUserId = aliceUser.Id },
            new Student { StudentId = 2, Name = "Bob Smith", Email = "bob@school.co.za", IdentityUserId = bobUser.Id },
            new Student { StudentId = 3, Name = "Charlie Brown", Email = "charliebrown@gmail.com", IdentityUserId = charlieUser.Id }
        );

        db.SaveChanges();
    }

    // Seed Enrollments
    if (!db.Enrollments.Any())
    {
        db.Enrollments.AddRange(
            new Enrollment { StudentId = 1, CourseId = 1 }, // Alice -> Mathematics
            new Enrollment { StudentId = 1, CourseId = 2 }, // Alice -> Science
            new Enrollment { StudentId = 2, CourseId = 3 }, // Bob -> History
            new Enrollment { StudentId = 3, CourseId = 4 }  // Charlie -> Literature
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

// ? Must come before MapBlazorHub
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
