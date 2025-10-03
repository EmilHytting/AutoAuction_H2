using System;
using System.Collections.Generic;
using System.Linq;
using AutoAuction_H2.Models;

namespace AutoAuction_H2.Services
{
    public static class DomainRules
    {
        // Check at user kan købe en vare til given pris samt virksomhedsbrugere kan bruge kredit
        public static bool CanBuy(User buyer, decimal price)
        {
            if (buyer is CorporateUser company)
                return company.Saldo + company.Credit >= price;
            if (buyer is PrivateUser privateUser)
                return privateUser.Saldo >= price;
            return false;
        }
    }
}
