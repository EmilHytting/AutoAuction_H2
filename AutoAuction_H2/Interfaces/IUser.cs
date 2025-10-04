using System;

namespace AutoAuction_H2.Models
{
    public enum UserType
    {
        Private,
        Corporate
    }

    public interface IUser
    {
        int Id { get; }
        string UserName { get; }
        decimal Balance { get; }
        int ZipCode { get; }
        UserType UserType { get; }

        bool Withdraw(decimal amount);
        void Deposit(decimal amount);
        void NotifyAboutBid(Auction auction, decimal amount);
    }
}
