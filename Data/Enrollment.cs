using CourseEnrollBlazorServer.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseEnrollBlazorServer.Data
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        [Required, ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        public Student Student { get; set; } = default!;

        [Required, ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public Course Course { get; set; } = default!;
    }
}
