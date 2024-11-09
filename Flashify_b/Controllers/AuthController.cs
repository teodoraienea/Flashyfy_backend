using Microsoft.AspNetCore.Mvc;
using Flashify_b.Models;
using Flashify_b.Services;
using Flashify_b.Data;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace Flashify_b.Controllers
{
   
        [Route("api/[controller]")]
        [ApiController]
        public class AuthController : ControllerBase
        {
            private readonly ApplicationDbContext _context;
            private readonly JwtService _jwtService;

            public AuthController(ApplicationDbContext context, JwtService jwtService)
            {
                _context = context;
                _jwtService = jwtService;
            }

            [HttpPost("register")]
            public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
            {
                if (await _context.Users.AnyAsync(u => u.Email == registerModel.Email))
                {
                    return BadRequest("User with this email already exists.");
                }

                var user = new User
                {
                    Username = registerModel.Username,
                    Email = registerModel.Email,
                    Password = HashPassword(registerModel.Password)
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var token = _jwtService.GenerateToken(user);
                return Ok(new { token });
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginModel.Email);
                if (user == null || !VerifyPassword(loginModel.Password, user.Password))
                {
                    return Unauthorized("Invalid credentials.");
                }

                var token = _jwtService.GenerateToken(user);
                return Ok(new { token });
            }

            private string HashPassword(string password)
            {
                using var sha256 = SHA256.Create();
                var hash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }

            private bool VerifyPassword(string inputPassword, string storedPasswordHash)
            {
                var inputHash = HashPassword(inputPassword);
                return inputHash == storedPasswordHash;
            }
        }
 }
