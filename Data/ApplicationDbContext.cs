using CourseEnrollBlazorServer.Data;
using Microsoft.EntityFrameworkCore;


namespace CourseEnrollBlazorServer.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<LoginModel> LoginModel => Set<LoginModel>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();

        // Model configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // Seed Courses
            // =========================
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(c => c.CourseId);
                entity.Property(c => c.Title).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Description).HasMaxLength(500);

                entity.HasData(
                    new Course { CourseId = 1, Title = "Mathematics", Description = "Numbers, algebra, and calculus" },
                    new Course { CourseId = 2, Title = "Science and Space", Description = "Physics, chemistry, biology and astronomy" },
                    new Course { CourseId = 3, Title = "History of UK", Description = "World history and events" },
                    new Course { CourseId = 4, Title = "Literature", Description = "Fiction, modern drama" }
                );
            });

            // =========================
            // Seed LoginModel
            // =========================
            modelBuilder.Entity<LoginModel>(entity =>
            {
                entity.HasKey(l => l.IntentityUserId);
                entity.Property(l => l.Email).IsRequired();
                entity.Property(l => l.Password).IsRequired();

                entity.HasData(
                    new LoginModel { IntentityUserId = 1, Email = "aliceJ@school.com", Password = "Password123!", RememberMe = false },
                    new LoginModel { IntentityUserId = 2, Email = "bob@school.co.za", Password = "Password123!", RememberMe = false },
                    new LoginModel { IntentityUserId = 3, Email = "charliebrown@gmail.com", Password = "Password123!", RememberMe = false }
                );
            });

            // =========================
            // Seed Students
            // =========================
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.StudentId);
                entity.Property(s => s.Name).IsRequired();
                entity.Property(s => s.Email).IsRequired();

                entity.HasData(
                    new Student { StudentId = 1, IdentityUserId = 1, Name = "Alice Johnson", Email = "aliceJ@school.com" },
                    new Student { StudentId = 2, IdentityUserId = 2, Name = "Bob Smith", Email = "bob@school.co.za" },
                    new Student { StudentId = 3, IdentityUserId = 3, Name = "Charlie Brown", Email = "charliebrown@gmail.com" }
                );
            });

            // =========================
            // Seed Enrollments
            // =========================
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.CourseId }); // composite key

                entity.HasData(
                    new Enrollment { StudentId = 1, CourseId = 1 }, // Alice -> Mathematics
                    new Enrollment { StudentId = 1, CourseId = 2 }, // Alice -> Science
                    new Enrollment { StudentId = 2, CourseId = 3 }, // Bob -> History
                    new Enrollment { StudentId = 3, CourseId = 4 }  // Charlie -> Literature
                );
            });
        }

    }
}