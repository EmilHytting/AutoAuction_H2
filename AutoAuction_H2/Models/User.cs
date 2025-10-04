using System;
using System.Security.Cryptography;
using System.Text;

namespace AutoAuction_H2.Models
{
    public abstract class User : IUser
    {
        public int Id { get; private set; }
        public string UserName { get; private set; }
        public string PasswordHash { get; private set; }
        public decimal Balance { get; protected set; }
        public int ZipCode { get; private set; }
        public UserType UserType { get; private set; }

        protected User(string userName, string password, int zipCode, decimal initialBalance, UserType userType)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Brugernavn må ikke være tomt.");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Adgangskode må ikke være tom.");

            UserName = userName;
            PasswordHash = HashPassword(password);
            ZipCode = zipCode;
            Balance = initialBalance;
            UserType = userType;
        }

        protected User() { } // EF Core

        // ---------- Password ----------
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public bool VerifyPassword(string password)
        {
            return PasswordHash == HashPassword(password);
        }

        // ---------- Balance ----------
        public virtual bool Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Beløbet skal være større end 0.");
            if (Balance - amount < 0)
                return false;

            Balance -= amount;
            return true;
        }

        public virtual void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Beløbet skal være større end 0.");
            Balance += amount;
        }

        // ---------- Notifications ----------
        public virtual void NotifyAboutBid(Auction auction, decimal amount)
        {
            Console.WriteLine($"🔔 Notifikation til {UserName}: Der er afgivet et bud på {amount} kr. for {auction.Vehicle.Name}");
        }

        public override string ToString()
        {
            return $"{UserName} (Saldo: {Balance} kr., Postnr.: {ZipCode}, Type: {UserType})";
        }
    }
}
