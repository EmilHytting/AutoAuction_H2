using AutoAuction_H2.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System;

namespace AutoAuction_H2.Models;

public abstract class User : IUser
{
    public int ID { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public decimal Saldo { get; set; }
    public int ZipCode { get; set; }

    // Default constructor
    public User() { }

    // Parameterized constructor
    public User(int id, string userName, string password, decimal saldo, int zipCode)
    {
        ID = id;
        UserName = userName;
        PasswordHash = HashPassword(password);
        Saldo = saldo;
        ZipCode = zipCode;
    }

    public static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    public static bool ValidateUserName(string userName)
    {
        return !string.IsNullOrWhiteSpace(userName) && userName.Length >= 3;
    }

    public override string ToString()
    {
        return $"User: ID={ID}, UserName={UserName}, Saldo={Saldo}, ZipCode={ZipCode}";
    }
}