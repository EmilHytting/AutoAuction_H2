using System;

namespace AutoAuction_H2.Models
{
    public class CorporateUser : User
    {
        public string CvrNumber { get; private set; }
        public decimal Credit { get; private set; }

        public CorporateUser(string userName, string password, int zipCode, decimal initialBalance, string cvrNumber, decimal credit)
            : base(userName, password, zipCode, initialBalance, UserType.Corporate)
        {
            if (string.IsNullOrWhiteSpace(cvrNumber))
                throw new ArgumentException("CVR-nummer må ikke være tomt.");
            if (cvrNumber.Length != 8)
                throw new ArgumentException("CVR-nummer skal bestå af 8 cifre.");
            if (credit < 0)
                throw new ArgumentException("Kredit må ikke være negativ.");

            CvrNumber = cvrNumber;
            Credit = credit;
        }

        private CorporateUser() { } // EF Core

        // Overwrite balance rules: corporate can go into minus (up to credit)
        public override bool Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Beløbet skal være større end 0.");

            if (Balance - amount < -Credit)
                return false;

            Balance -= amount;
            return true;
        }

        public override string ToString()
        {
            return base.ToString() + $" | Firma (CVR: {CvrNumber}, Kredit: {Credit} kr.)";
        }
    }
}
