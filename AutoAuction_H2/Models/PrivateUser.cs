using AutoAuction_H2.Interfaces;
using System;

namespace AutoAuction_H2.Models;

public class PrivateUser : User, ISeller, IBuyer
{
    public string CPRnummer { get; set; } = string.Empty;

    // Default constructor
    public PrivateUser() { }

    // Parameterized constructor for database or manual initialization
    public PrivateUser(int id, string userName, string password, decimal saldo, int zipCode, string cprNummer)
        : base(id, userName, password, saldo, zipCode)
    {
        CPRnummer = cprNummer;
    }

    // Balance must not go below zero
    public bool CanBuy(decimal pris)
    {
        return Saldo - pris >= 0;
    }

    // Method to notify seller about a bid on a vehicle
    public void GetNotificationAboutBid(object vehicle, decimal bid)
    {
        System.Console.WriteLine($"Bid on {vehicle}: {bid} kr.");
    }

    // Override ToString for better display
    public override string ToString()
    {
        return $"PrivateUser: ID={ID}, UserName={UserName}, Saldo={Saldo}, ZipCode={ZipCode}, CPRnummer={CPRnummer}";
    }
}