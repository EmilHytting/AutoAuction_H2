using AutoAuction_H2.Models.Entities;

namespace AutoAuction_H2.Models.Interfaces
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

        // Reservation/tilg�ngeligt bel�b
        decimal ReservedAmount { get; }
        decimal AvailableBalance { get; }

        // Pengeh�ndtering
        bool Withdraw(decimal amount);
        void Deposit(decimal amount);

        // Reservation
        bool Reserve(decimal amount);
        void Release(decimal amount);

        // Notifikation
        void NotifyAboutBid(Auction auction, decimal amount);
    }
}
