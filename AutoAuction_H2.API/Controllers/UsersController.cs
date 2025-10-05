using AutoAuction_H2.Models.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoAuction_H2.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AuctionDbContext _context;

        public UsersController(AuctionDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserEntity>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserEntity>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            return user;
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserEntity>> CreateUser(CreateUserRequest request)
        {
            var user = new UserEntity
            {
                UserName = request.UserName,
                PasswordHash = UserEntity.DoubleHash(request.Password), // dobbelthash
                Balance = request.Balance,
                ZipCode = request.ZipCode,
                UserType = request.UserType,
                CreditLimit = request.CreditLimit
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserRequest request)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.UserName = request.UserName ?? user.UserName;
            user.ZipCode = request.ZipCode != 0 ? request.ZipCode : user.ZipCode;
            user.Balance = request.Balance != 0 ? request.Balance : user.Balance;
            user.UserType = request.UserType != 0 ? request.UserType : user.UserType;
            user.CreditLimit = request.CreditLimit != 0 ? request.CreditLimit : user.CreditLimit;

            if (!string.IsNullOrEmpty(request.Password))
            {
                // ⚡ Password er allerede SHA256 fra klienten
                // Her laver vi en ekstra SHA256 → dobbelthash
                user.PasswordHash = UserEntity.DoubleHash(request.Password);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }


        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class CreateUserRequest
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;  // hash fra app (første hash)
        public decimal Balance { get; set; }
        public int ZipCode { get; set; }
        public int UserType { get; set; }
        public decimal CreditLimit { get; set; }
    }

    public class UpdateUserRequest : CreateUserRequest
    {
        public int Id { get; set; }
    }
}
