using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoAuction_H2.ViewModels;
using System.Collections.ObjectModel;
using AutoAuction_H2.Models;

namespace AutoAuction_H2.ViewModels
{
    public partial class AuctionOverviewViewModel : ViewModelBase
    {
        public ObservableCollection<AuctionItem> CurrentAuctions { get; } = new()
        {
            new AuctionItem { Id = 1, Name = "Mercedes C-klasse", Year = 2018, CurrentBid = 250000 },
            new AuctionItem { Id = 2, Name = "BMW X5", Year = 2020, CurrentBid = 400000 },
            new AuctionItem { Id = 3, Name = "Porsche 911", Year = 2015, CurrentBid = 900000 },
            new AuctionItem { Id = 4, Name = "Ford Mustang", Year = 2022, CurrentBid = 550000 },
            new AuctionItem { Id = 5, Name = "Skoda Octavia", Year = 2019, CurrentBid = 180000 },
            new AuctionItem { Id = 6, Name = "Tesla Model 3", Year = 2021, CurrentBid = 380000 },
        };

        public ObservableCollection<AuctionItem> YourAuctions { get; } = new()
        {
            new AuctionItem { Id = 10, Name = "Volvo V70", Year = 1998, CurrentBid = 15000 },
            new AuctionItem { Id = 11, Name = "Audi A4", Year = 2010, CurrentBid = 45000 },
            new AuctionItem { Id = 12, Name = "Volkswagen Golf", Year = 2005, CurrentBid = 22000 },
            new AuctionItem { Id = 13, Name = "Ford Focus", Year = 2012, CurrentBid = 30000 },
            new AuctionItem { Id = 14, Name = "Toyota Corolla", Year = 2008, CurrentBid = 28000 },
        };
    }
}
