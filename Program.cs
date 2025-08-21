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

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("SchoolDb"));

//builder.Services.Add<IdentityUser>(options =>
//{
//    options.SignIn.RequireConfirmedAccount = false; // set to true if you want email confirmation
//})
//.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Seed Courses with explicit IDs
    if (!db.Courses.Any())
    {
        db.Courses.AddRange(
            new Course { 
                CourseId = 1,
                Title = "Mathematics", 
                Description = "Numbers, algebra, and calculus" 
            },
            new Course {
                CourseId = 2, 
                Title = "Science and Space", 
                Description = "Physics, chemistry, biology and astronomy" 
            },
            new Course { 
                CourseId = 3, 
                Title = "History of UK", 
                Description = "World history and events" 
            },
            new Course { 
                CourseId = 4, 
                Title = "Literature", 
                Description = "Fiction, modern drama" }
        );
        db.SaveChanges();
    }

    // Seed Students with explicit IDs
    if (!db.Students.Any())
    {
        db.Students.AddRange(
            new Student { 
                StudentId = 1,
                Name = "Alice Johnson",
                IdentityUserId = 1, // Assuming IdentityUserId is set to 1 for Alice
                Email = "aliceJ@school.com" 
            },
            new Student { 
                StudentId = 2, 
                Name = "Bob Smith", 
                Email = "bob@school.co.za" 
            },
            new Student { 
                StudentId = 3, 
                Name = "Charlie Brown", 
                Email = "charliebrown@gmail.com" }
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
    if (!db.LoginModel.Any())
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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
