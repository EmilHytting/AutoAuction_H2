using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Models.Interfaces;

namespace AutoAuction_H2.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new();

        // ---------- CREATE ----------
        public Task<PrivateUser> CreatePrivateUserAsync(string userName, string password, int zipCode, decimal initialBalance, string cprNumber)
        {
            var user = new PrivateUser(userName, password, zipCode, initialBalance, cprNumber);
            _users.Add(user);
            return Task.FromResult(user);
        }

        public Task<CorporateUser> CreateCorporateUserAsync(string userName, string password, int zipCode, decimal initialBalance, string cvrNumber, decimal credit)
        {
            var user = new CorporateUser(userName, password, zipCode, initialBalance, cvrNumber, credit);
            _users.Add(user);
            return Task.FromResult(user);
        }

        // ---------- AUTH ----------
        public Task<User?> AuthenticateAsync(string userName, string password)
        {
            var user = _users.FirstOrDefault(u => u.UserName == userName);
            if (user == null) return Task.FromResult<User?>(null);

            return Task.FromResult(user.VerifyPassword(password) ? user : null);
        }

        public Task<User?> FindByUserNameAsync(string userName)
        {
            var user = _users.FirstOrDefault(u => u.UserName == userName);
            return Task.FromResult(user);
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return Task.FromResult<IEnumerable<User>>(_users);
        }

        // ---------- UPDATE ----------
        public Task<bool> UpdatePasswordAsync(string userName, string newPassword)
        {
            var user = _users.FirstOrDefault(u => u.UserName == userName);
            if (user == null) return Task.FromResult(false);

            // hash på samme måde som User gør det
            var hash = User.HashPasswordForService(newPassword);
            user.GetType().GetProperty("PasswordHash")?.SetValue(user, hash);

            return Task.FromResult(true);
        }

        public Task<bool> UpdateZipCodeAsync(string userName, int newZipCode)
        {
            var user = _users.FirstOrDefault(u => u.UserName == userName);
            if (user == null) return Task.FromResult(false);

            user.GetType().GetProperty("ZipCode")?.SetValue(user, newZipCode);
            return Task.FromResult(true);
        }

        public Task<bool> UpdateBalanceAsync(string userName, decimal newBalance)
        {
            var user = _users.FirstOrDefault(u => u.UserName == userName);
            if (user == null) return Task.FromResult(false);

            user.GetType().GetProperty("Balance")?.SetValue(user, newBalance);
            return Task.FromResult(true);
        }

        // ---------- DELETE ----------
        public Task<bool> DeleteUserAsync(string userName)
        {
            var user = _users.FirstOrDefault(u => u.UserName == userName);
            if (user == null) return Task.FromResult(false);

            _users.Remove(user);
            return Task.FromResult(true);
        }
    }
}
