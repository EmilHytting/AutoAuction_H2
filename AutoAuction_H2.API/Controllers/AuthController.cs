using AutoAuction_H2.Models.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoAuction_H2.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuctionDbContext _context;

        public AuthController(AuctionDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
            if (user == null)
                return Unauthorized("User not found");

            // Sammenlign med dobbelthash
            if (!user.VerifyPassword(request.Password))
                return Unauthorized("Invalid password");

            return Ok(new
            {
                Message = "Login successful",
                UserId = user.Id,
                UserName = user.UserName,
                Balance = user.Balance,
                UserType = user.UserType
            });
        }
    }

    public class LoginRequest
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;  // hash fra app (første hash)
    }
}
