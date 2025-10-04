using System.Collections.Generic;
using AutoAuction_H2.Models;

namespace AutoAuction_H2.Services
{
    public interface IUserService
    {
        PrivateUser CreatePrivateUser(string userName, string password, int zipCode, decimal initialBalance, string cprNumber);
        CorporateUser CreateCorporateUser(string userName, string password, int zipCode, decimal initialBalance, string cvrNumber, decimal credit);

        User? Authenticate(string userName, string password);
        User? FindByUserName(string userName);
        IEnumerable<User> GetAllUsers();

        // 🔥 Ny CRUD
        bool DeleteUser(string userName);
        bool UpdatePassword(string userName, string newPassword);
        bool UpdateZipCode(string userName, int newZipCode);
        bool UpdateBalance(string userName, decimal newBalance);
    }
}
