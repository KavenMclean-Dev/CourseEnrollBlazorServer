using Microsoft.AspNetCore.Identity;

// Use int as the key type to match Student

namespace CourseEnrollBlazorServer.Data
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; } = string.Empty; // To sync with Student.Name
    }
}
