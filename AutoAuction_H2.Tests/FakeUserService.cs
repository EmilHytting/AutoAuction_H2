using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAuction_H2.Tests.Fakes
{
    /// <summary>
    /// In-memory implementation of IUserService for unit tests.
    /// No HTTP calls or EF dependencies.
    /// </summary>
    public class FakeUserService : IUserService
    {
        private readonly List<User> _users = new();
        private int _nextId = 1;

        // ----------------------------------------------------
        // CREATE PRIVATE USER
        // ----------------------------------------------------
        public Task<PrivateUser> CreatePrivateUserAsync(string userName, string password, int zipCode, decimal initialBalance, string cprNumber)
        {
            var user = new PrivateUser(userName, password, zipCode, initialBalance, cprNumber);
            SetId(user, _nextId++); // ✅ uses reflection instead of direct assignment
            _users.Add(user);
            return Task.FromResult(user);
        }

        // ----------------------------------------------------
        // CREATE CORPORATE USER
        // ----------------------------------------------------
        public Task<CorporateUser> CreateCorporateUserAsync(string userName, string password, int zipCode, decimal initialBalance, string cvrNumber, decimal credit)
        {
            var user = new CorporateUser(userName, password, zipCode, initialBalance, cvrNumber, credit);
            SetId(user, _nextId++);
            _users.Add(user);
            return Task.FromResult(user);
        }

        // ----------------------------------------------------
        // AUTHENTICATE
        // ----------------------------------------------------
        public Task<User?> AuthenticateAsync(string userName, string password)
        {
            var user = _users.FirstOrDefault(u => u.UserName == userName && u.PasswordHash == password);
            return Task.FromResult(user);
        }

        // ----------------------------------------------------
        // FIND BY USERNAME
        // ----------------------------------------------------
        public Task<User?> FindByUserNameAsync(string userName)
        {
            var user = _users.FirstOrDefault(u => u.UserName == userName);
            return Task.FromResult(user);
        }

        // ----------------------------------------------------
        // GET ALL USERS
        // ----------------------------------------------------
        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return Task.FromResult<IEnumerable<User>>(_users.ToList());
        }

        // ----------------------------------------------------
        // UPDATE METHODS
        // ----------------------------------------------------
        public Task<bool> UpdatePasswordAsync(string userName, string newPassword)
        {
            var user = _users.FirstOrDefault(u => u.UserName == userName);
            if (user == null) return Task.FromResult(false);
            typeof(User).GetProperty(nameof(User.PasswordHash))?.SetValue(user, newPassword);
            return Task.FromResult(true);
        }

        public Task<bool> UpdateZipCodeAsync(string userName, int newZipCode)
        {
            var user = _users.FirstOrDefault(u => u.UserName == userName);
            if (user == null) return Task.FromResult(false);
            typeof(User).GetProperty(nameof(User.ZipCode))?.SetValue(user, newZipCode);
            return Task.FromResult(true);
        }

        public Task<bool> UpdateBalanceAsync(string userName, decimal newBalance)
        {
            var user = _users.FirstOrDefault(u => u.UserName == userName);
            if (user == null) return Task.FromResult(false);
            typeof(User).GetProperty(nameof(User.Balance))?.SetValue(user, newBalance);
            return Task.FromResult(true);
        }

        // ----------------------------------------------------
        // DELETE
        // ----------------------------------------------------
        public Task<bool> DeleteUserAsync(string userName)
        {
            var user = _users.FirstOrDefault(u => u.UserName == userName);
            if (user == null) return Task.FromResult(false);
            _users.Remove(user);
            return Task.FromResult(true);
        }

        // ----------------------------------------------------
        // Utility helper (reflection setter)
        // ----------------------------------------------------
        private static void SetId(User user, int id)
        {
            typeof(User).GetProperty(nameof(User.Id))?.SetValue(user, id);
        }
    }
}
