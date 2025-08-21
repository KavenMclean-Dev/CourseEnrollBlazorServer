using CourseEnrollBlazorServer.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

// Configure DbContext with Identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("SchoolDb").EnableSensitiveDataLogging());

// Register Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Add authentication and authorization
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// Add UserManager and SignInManager services
builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddScoped<SignInManager<IdentityUser>>();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Seed Courses with explicit IDs
    if (!db.Courses.Any())
    {
        db.Courses.AddRange(
            new Course
            {
                CourseId = 1,
                Title = "Mathematics",
                Description = "Numbers, algebra, and calculus"
            },
            new Course
            {
                CourseId = 2,
                Title = "Science and Space",
                Description = "Physics, chemistry, biology and astronomy"
            },
            new Course
            {
                CourseId = 3,
                Title = "History of UK",
                Description = "World history and events"
            },
            new Course
            {
                CourseId = 4,
                Title = "Literature",
                Description = "Fiction, modern drama"
            }
        );
        db.SaveChanges();
    }

    // Seed Students with explicit IDs
    if (!db.Students.Any())
    {
        db.Students.AddRange(
            new Student
            {
                StudentId = 1,
                Name = "Alice Johnson",
                IdentityUserId = 1, // Assuming IdentityUserId is set to 1 for Alice
                Email = "aliceJ@school.com"
            },
            new Student
            {
                StudentId = 2,
                IdentityUserId = 2, // Assuming IdentityUserId is set to 1 for Alice
                Name = "Bob Smith",
                Email = "bob@school.co.za"
            },
            new Student
            {
                StudentId = 3,
                IdentityUserId = 3, // Assuming IdentityUserId is set to 1 for Alice,
                Name = "Charlie Brown",
                Email = "charliebrown@gmail.com"
            }
        );
        db.SaveChanges();
    }

    // Seed Enrollments
    if (!db.Enrollments.Any())
    {
        db.Enrollments.AddRange(
            new Enrollment { StudentId = 1, CourseId = 1 }, // Alice -> Mathematics
            new Enrollment { StudentId = 1, CourseId = 2 }, // Alice -> Science
            new Enrollment { StudentId = 2, CourseId = 3 }  // Bob -> History
        );
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();