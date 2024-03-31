using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Todo_Application.Data;
using Todo_Application.dto;
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

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto loginDto)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserName == loginDto.UserName);

            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                    { 
                    var jwt = GenerateJwtToken(user);
                    return Ok(jwt);
                
                }
                else
                {
                    return Unauthorized("Invalid password.");
                }

            }

            return Unauthorized("Invalid username.");
            
          
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                     new Claim(ClaimTypes.Email, user.Email)
                }),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(1), // Token expiry time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) //Hs256
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }



    }
}
