//using CourseEnrollBlazorServer.Data;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace CourseEnrollBlazorServer.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class LoginController : ControllerBase
//    {
//        private readonly SignInManager<IdentityUser> _signInManager;
//        private readonly UserManager<IdentityUser> _userManager;
//        private readonly ApplicationDbContext _db;

//        //Constructor to inject dependencies
//        public LoginController(
//            SignInManager<IdentityUser> signInManager,
//            UserManager<IdentityUser> userManager,
//            ApplicationDbContext db)
//        {
//            _signInManager = signInManager;
//            _userManager = userManager;
//            _db = db;
//        }

//        public class LoginRequest
//        {
//            public string Email { get; set; } = string.Empty;
//            public string Password { get; set; } = string.Empty;
//        }

//        public class LoginResponse
//        {
//            public bool Success { get; set; }
//            public string Message { get; set; } = string.Empty;
//            public int? StudentId { get; set; }
//            public string? Name { get; set; }
//        }

//        /// <summary>
//        /// Use a login request to authenticate a user and return their student information.
//        /// This is the base of the login functionality as a post request
//        /// Taks and ActionResult are used to handle the asynchronous nature of the request.
//        [HttpPost("Login")]
//        public async Task<ActionResult<LoginResponse>> LoginStudent([FromBody] LoginRequest request)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(new LoginResponse { Success = false, Message = "Invalid request" });

//            // Attempt sign-in
//            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent: false, lockoutOnFailure: false);

//            if (!result.Succeeded)
//                return Unauthorized(new LoginResponse { Success = false, Message = "Invalid login attempt" });

//            // Find the linked student
//            var student = await _db.Students.FirstOrDefaultAsync(s => s.Email == request.Email);

//            if (student == null)
//                return NotFound(new LoginResponse { Success = false, Message = "Student record not found" });

//            return  Ok(new LoginResponse
//            {
//                Success = true,
//                Message = "Login successful",
//                StudentId = Convert.ToInt16(student.StudentId),
//                Name = student.Name
//            });
//        }
//    }
//}
