//using CourseEnrollBlazorServer.Data;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;


//namespace SchoolApp.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EnrollmentsController : ControllerBase
//    {
//        private readonly ApplicationDbContext _db;

//        public EnrollmentsController(ApplicationDbContext db)
//        {
//            _db = db;
//        }

//        public class EnrollmentRequest
//        {
//            public int StudentId { get; set; }
//            public int CourseId { get; set; }
//        }

//        [HttpPost("Enroll")]
//        public async Task<IActionResult> Enroll([FromBody] EnrollmentRequest request)
//        {
//            // Check if already enrolled
//            var exists = await _db.Enrollments
//                .AnyAsync(e => e.StudentId == request.StudentId && e.CourseId == request.CourseId);

//            if (exists)
//                return BadRequest("Already enrolled.");

//            var enrollment = new Enrollment
//            {
//                StudentId = request.StudentId,
//                CourseId = request.CourseId
//            };

//            _db.Enrollments.Add(enrollment);
//            await _db.SaveChangesAsync();

//            return Ok(new { Message = "Enrollment successful" });
//        }

//        [HttpPost("Deregister")]
//        public async Task<IActionResult> Deregister([FromBody] EnrollmentRequest request)
//        {
//            var enrollment = await _db.Enrollments
//                .FirstOrDefaultAsync(e => e.StudentId == request.StudentId && e.CourseId == request.CourseId);

//            if (enrollment == null)
//                return NotFound("Enrollment not found.");

//            _db.Enrollments.Remove(enrollment);
//            await _db.SaveChangesAsync();

//            return Ok(new { Message = "Deregistered successfully" });
//        }

//        [HttpGet("MyCourses/{studentId}")]
//        public async Task<IActionResult> GetMyCourses(int studentId)
//        {
//            var courses = await _db.Enrollments
//                .Where(e => e.StudentId == studentId)
//                .Select(e => e.Course)
//                .ToListAsync();

//            return Ok(courses);
//        }

//        [HttpGet("AllCourses")]
//        public async Task<IActionResult> GetAllCourses()
//        {
//            var courses = await _db.Courses.ToListAsync();
//            return Ok(courses);
//        }
//    }
//}
