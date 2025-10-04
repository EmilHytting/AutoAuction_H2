using System;

namespace AutoAuction_H2.Models
{
    public class PrivateUser : User
    {
        public string CprNumber { get; private set; }

        public PrivateUser(string userName, string password, int zipCode, decimal initialBalance, string cprNumber)
            : base(userName, password, zipCode, initialBalance, UserType.Private)
        {
            if (string.IsNullOrWhiteSpace(cprNumber))
                throw new ArgumentException("CPR-nummer må ikke være tomt.");
            if (cprNumber.Length != 10)
                throw new ArgumentException("CPR-nummer skal bestå af 10 cifre.");

            CprNumber = cprNumber;
        }

        private PrivateUser() { } // EF Core

        public override string ToString()
        {
            return base.ToString() + $" | Privat bruger (CPR: {CprNumber})";
        }
    }
}
