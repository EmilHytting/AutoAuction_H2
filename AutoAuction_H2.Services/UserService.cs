using System;
using System.Collections.Generic;
using System.Linq;


using AutoAuction_H2.Models.Interfaces;
using AutoAuction_H2.Models.Entities;

namespace AutoAuction_H2.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new();

        // ---------- Create ----------
        public PrivateUser CreatePrivateUser(string userName, string password, int zipCode, decimal initialBalance, string cprNumber)
        {
            if (_users.Any(u => u.UserName == userName))
                throw new ArgumentException($"Brugernavn '{userName}' er allerede taget.");

            var user = new PrivateUser(userName, password, zipCode, initialBalance, cprNumber);
            _users.Add(user);
            return user;
        }

        public CorporateUser CreateCorporateUser(string userName, string password, int zipCode, decimal initialBalance, string cvrNumber, decimal credit)
        {
            if (_users.Any(u => u.UserName == userName))
                throw new ArgumentException($"Brugernavn '{userName}' er allerede taget.");

            var user = new CorporateUser(userName, password, zipCode, initialBalance, cvrNumber, credit);
            _users.Add(user);
            return user;
        }

        // ---------- Authentication ----------
        public User? Authenticate(string userName, string password)
        {
            var user = _users.FirstOrDefault(u => u.UserName == userName);
            return user != null && user.VerifyPassword(password) ? user : null;
        }

        // ---------- Find ----------
        public User? FindByUserName(string userName)
        {
            return _users.FirstOrDefault(u => u.UserName == userName);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _users;
        }

        // ---------- Update ----------
        public bool UpdatePassword(string userName, string newPassword)
        {
            var user = FindByUserName(userName);
            if (user == null) return false;

            var type = user.GetType();
            var passwordHashProp = typeof(User).GetProperty("PasswordHash", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (passwordHashProp == null) return false;

            var hashMethod = typeof(User).GetMethod("HashPassword", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            if (hashMethod == null) return false;

            string hashed = (string)hashMethod.Invoke(null, new object[] { newPassword })!;
            passwordHashProp.SetValue(user, hashed);
            return true;
        }

        public bool UpdateZipCode(string userName, int newZipCode)
        {
            var user = FindByUserName(userName);
            if (user == null) return false;

            var zipProp = typeof(User).GetProperty("ZipCode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (zipProp == null) return false;

            zipProp.SetValue(user, newZipCode);
            return true;
        }

        public bool UpdateBalance(string userName, decimal newBalance)
        {
            var user = FindByUserName(userName);
            if (user == null) return false;

            user.Deposit(newBalance - user.Balance);
            return true;
        }

        // ---------- Delete ----------
        public bool DeleteUser(string userName)
        {
            var user = FindByUserName(userName);
            if (user == null) return false;

            return _users.Remove(user);
        }
    }
}
