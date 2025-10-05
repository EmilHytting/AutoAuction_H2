using System;
using System.Security.Cryptography;
using System.Text;

namespace AutoAuction_H2.Models.Persistence
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public decimal Balance { get; set; }
        public int ZipCode { get; set; }

        // 0 = PrivateUser, 1 = CorporateUser
        public int UserType { get; set; }
        public decimal CreditLimit { get; set; } // Only used if Corporate

        // ✅ Bruges når vi gemmer (fra app → API → DB)
        public static string DoubleHash(string clientHash)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(clientHash);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        // ✅ Bruges når vi logger ind (app sender hash → API sammenligner mod DB)
        public bool VerifyPassword(string clientHash)
        {
            var doubleHashed = DoubleHash(clientHash);
            return PasswordHash == doubleHashed;
        }
    }
}
