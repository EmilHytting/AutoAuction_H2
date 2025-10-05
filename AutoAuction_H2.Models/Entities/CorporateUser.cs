using System;
using AutoAuction_H2.Models.Interfaces;

namespace AutoAuction_H2.Models.Entities
{
    public class CorporateUser : User
    {
        public string CvrNumber { get; private set; }
        public decimal Credit { get; private set; }

        public override UserType UserType => UserType.Corporate; // USER TYPE 1

        public CorporateUser(
            string userName,
            string password,
            int zipCode,
            decimal initialBalance,
            string cvrNumber,
            decimal credit)
            : base(userName, password, zipCode, initialBalance)
        {
            if (string.IsNullOrWhiteSpace(cvrNumber))
                throw new ArgumentException("CVR-nummer m� ikke v�re tomt.");
            if (cvrNumber.Length != 8)
                throw new ArgumentException("CVR-nummer skal best� af 8 cifre.");
            if (credit < 0)
                throw new ArgumentException("Kredit m� ikke v�re negativ.");

            CvrNumber = cvrNumber;
            Credit = credit;
        }

        private CorporateUser() { } // EF Core

        // Firmaer kan g� i minus op til kreditten
        public override bool Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Bel�bet skal v�re st�rre end 0.");

            // tager h�jde for reserverede bel�b
            if ((Balance - ReservedAmount) - amount < -Credit)
                return false;

            Balance -= amount;
            return true;
        }

        // Firmaer kan reservere inkl. kredit
        public override bool Reserve(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Bel�bet skal v�re st�rre end 0.");

            var room = Balance + Credit - ReservedAmount;
            if (room < amount) return false;

            ReservedAmount += amount; // protected set i base g�r dette muligt
            return true;
        }

        public override string ToString()
        {
            return base.ToString() + $" | Firma (CVR: {CvrNumber}, Kredit: {Credit} kr.)";
        }
    }
}
