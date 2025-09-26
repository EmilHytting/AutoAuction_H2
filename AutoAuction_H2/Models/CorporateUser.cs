using AutoAuction_H2.Interfaces;
using System;

namespace AutoAuction_H2.Models;

public class CorporateUser : User, ISeller, IBuyer
{
    public string CVRnummer { get; set; } = string.Empty;
    public decimal Credit { get; set; }

    // Default constructor
    public CorporateUser() { }

    // Parameterized constructor for database or manual initialization
    public CorporateUser(int id, string userName, string password, decimal saldo, int zipCode, string cvrNummer, decimal credit)
        : base(id, userName, password, saldo, zipCode)
    {
        CVRnummer = cvrNummer;
        Credit = credit;
    }

    // Balance can go below zero up to credit
    public bool CanBuy(decimal pris)
    {
        return Saldo - pris >= -Credit;
    }

    // Method to notify seller about a bid on a vehicle
    public void GetNotificationAboutBid(object vehicle, decimal bid)
    {
        System.Console.WriteLine($"Bid on {vehicle}: {bid} kr.");
    }

    // Override ToString for better display
    public override string ToString()
    {
        return $"CorporateUser: ID={ID}, UserName={UserName}, Saldo={Saldo}, ZipCode={ZipCode}, CVRnummer={CVRnummer}, Credit={Credit}";
    }
}