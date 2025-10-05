using System;
using System.Security.Cryptography;
using System.Text;
using AutoAuction_H2.Models.Interfaces;

namespace AutoAuction_H2.Models.Entities
{
    public abstract class User : IUser
    {
        private static int _idCounter = 0;

        public int Id { get; private set; }
        public string UserName { get; protected set; }
        public string PasswordHash { get; protected set; }
        public decimal Balance { get; protected set; }
        public int ZipCode { get; protected set; }

        // Kræves af din arkitektur: afledte klasser bestemmer brugertype
        public abstract UserType UserType { get; }

        // Reservation til bud
        public decimal ReservedAmount { get; protected set; }  // <-- beskyttet setter, så subklasser kan ændre
        public decimal AvailableBalance => Balance - ReservedAmount;

        protected User(string userName, string password, int zipCode, decimal initialBalance)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Brugernavn må ikke være tomt.");
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Adgangskode må ikke være tom.");

            Id = ++_idCounter;
            UserName = userName;
            PasswordHash = HashPassword(password);
            ZipCode = zipCode;
            Balance = initialBalance;
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

        public bool VerifyPassword(string password) => PasswordHash == HashPassword(password);

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

        // ---------- Reservation ----------
        public virtual bool Reserve(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Beløbet skal være større end 0.");
            if (AvailableBalance < amount) return false;

            ReservedAmount += amount;
            return true;
        }

        public virtual void Release(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Beløbet skal være større end 0.");
            if (ReservedAmount < amount) throw new InvalidOperationException("Kan ikke frigive mere end reserveret.");

            ReservedAmount -= amount;
        }

        // ---------- Notifications ----------
        public virtual void NotifyAboutBid(Auction auction, decimal amount)
        {
            Console.WriteLine($"🔔 Notifikation til {UserName}: Der er afgivet et bud på {amount} kr. for {auction.Vehicle.Name}");
        }

        public override string ToString()
        {
            return $"{UserName} (Saldo: {Balance} kr., Reserveret: {ReservedAmount} kr., Postnr.: {ZipCode})";
        }
    }
}
