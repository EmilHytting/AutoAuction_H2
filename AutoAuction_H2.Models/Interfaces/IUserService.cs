using System.Collections.Generic;
using System.Threading.Tasks;
using AutoAuction_H2.Models.Entities;

namespace AutoAuction_H2.Models.Interfaces
{
    public interface IUserService
    {
        // --- Async versioner ---
        Task<PrivateUser> CreatePrivateUserAsync(string userName, string password, int zipCode, decimal initialBalance, string cprNumber);
        Task<CorporateUser> CreateCorporateUserAsync(string userName, string password, int zipCode, decimal initialBalance, string cvrNumber, decimal credit);

        Task<User?> AuthenticateAsync(string userName, string password);
        Task<User?> FindByUserNameAsync(string userName);
        Task<IEnumerable<User>> GetAllUsersAsync();

        Task<bool> UpdatePasswordAsync(string userName, string newPassword);
        Task<bool> UpdateZipCodeAsync(string userName, int newZipCode);
        Task<bool> UpdateBalanceAsync(string userName, decimal newBalance);

        Task<bool> DeleteUserAsync(string userName);

        // --- Synkron wrappers til gammel kode (default implementations) ---
        PrivateUser CreatePrivateUser(string userName, string password, int zipCode, decimal initialBalance, string cprNumber)
            => CreatePrivateUserAsync(userName, password, zipCode, initialBalance, cprNumber).GetAwaiter().GetResult();

        CorporateUser CreateCorporateUser(string userName, string password, int zipCode, decimal initialBalance, string cvrNumber, decimal credit)
            => CreateCorporateUserAsync(userName, password, zipCode, initialBalance, cvrNumber, credit).GetAwaiter().GetResult();

        User? Authenticate(string userName, string password)
            => AuthenticateAsync(userName, password).GetAwaiter().GetResult();

        User? FindByUserName(string userName)
            => FindByUserNameAsync(userName).GetAwaiter().GetResult();

        IEnumerable<User> GetAllUsers()
            => GetAllUsersAsync().GetAwaiter().GetResult();

        bool UpdatePassword(string userName, string newPassword)
            => UpdatePasswordAsync(userName, newPassword).GetAwaiter().GetResult();

        bool UpdateZipCode(string userName, int newZipCode)
            => UpdateZipCodeAsync(userName, newZipCode).GetAwaiter().GetResult();

        bool UpdateBalance(string userName, decimal newBalance)
            => UpdateBalanceAsync(userName, newBalance).GetAwaiter().GetResult();

        bool DeleteUser(string userName)
            => DeleteUserAsync(userName).GetAwaiter().GetResult();
    }
}
