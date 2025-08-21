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

            // ==========================
            // Enrollment Table Configuration
            // This is a combination of Student and Course Tables
            // ==========================
            modelBuilder.Entity<Enrollment>(entity =>
            {
                // Prevent duplicate enrollments
                entity.HasIndex(e => new { e.StudentId, e.CourseId }).IsUnique();

                // Configure relationships
                entity.HasOne(e => e.Student)
                      .WithMany(s => s.Enrollments)
                      .HasForeignKey(e => e.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Course)
                      .WithMany(c => c.Enrollments)
                      .HasForeignKey(e => e.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            // ==========================
            // Student Table Configuration
            // ==========================
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.StudentId);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Email).IsRequired().HasMaxLength(150);
            });
            // ==========================
            // Course Table Configuration
            // ==========================
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(c => c.CourseId);
                entity.Property(c => c.Title).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Description).HasMaxLength(500);
            });
        }
    }
}