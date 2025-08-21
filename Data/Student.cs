using CourseEnrollBlazorServer.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseEnrollBlazorServer.Data
{
    public class Student
    {
        [Key, Required]
        public int StudentId { get; set; }
        [Required]
        [ForeignKey(nameof(LoginModel))]
        public int IdentityUserId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
