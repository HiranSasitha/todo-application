using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo_Application.Data;
using Todo_Application.Model;

namespace Todo_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly string _jwtSecret;

        public AuthController(AppDbContext context,IConfiguration configuration )
        {
            _context = context;
            _jwtSecret = configuration.GetValue<String>("JwtSecret");  // get jwtSecret by json file
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            if (await _context.User.AnyAsync(u => u.UserName == user.UserName))
                return BadRequest("Username already exists.");

            var newUser = new User
            {
                UserName = user.UserName,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password) //password hashing using Bcrypt
            };

            _context.User.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("registered successfully.");
        }

        // POST: api/Auth/get-all-users
        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUser()       // get All available-todo-items
        {
            try
            {
                var todo = await _context.User.ToListAsync();
                return Ok(todo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



    }
}
