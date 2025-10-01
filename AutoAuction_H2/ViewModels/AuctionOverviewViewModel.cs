using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoAuction_H2.ViewModels;
using System.Collections.ObjectModel;
using AutoAuction_H2.Models;
using CommunityToolkit.Mvvm.Input;

namespace AutoAuction_H2.ViewModels
{
    public partial class AuctionOverviewViewModel : ViewModelBase
    {
        public ObservableCollection<AuctionItem> CurrentAuctions { get; } = new()
        {
            new AuctionItem { Id = 1, Name = "Mercedes C-klasse", Year = 2018, CurrentBid = 250000, ClosingDate = DateTime.Now.AddDays(7) },
            new AuctionItem { Id = 2, Name = "BMW X5", Year = 2020, CurrentBid = 400000, ClosingDate = DateTime.Now.AddDays(3) },
            new AuctionItem { Id = 3, Name = "Porsche 911", Year = 2015, CurrentBid = 900000, ClosingDate = DateTime.Now.AddDays(14) },
            new AuctionItem { Id = 4, Name = "Ford Mustang", Year = 2022, CurrentBid = 550000, ClosingDate = DateTime.Now.AddDays(2) },
            new AuctionItem { Id = 5, Name = "Skoda Octavia", Year = 2019, CurrentBid = 180000, ClosingDate = DateTime.Now.AddDays(5) },
            new AuctionItem { Id = 6, Name = "Tesla Model 3", Year = 2021, CurrentBid = 380000, ClosingDate = DateTime.Now.AddDays(1) },
        };

        public ObservableCollection<AuctionItem> YourAuctions { get; } = new()
        {
            new AuctionItem { Id = 10, Name = "Volvo V70", Year = 1998, CurrentBid = 15000, ClosingDate = DateTime.Now.AddDays(9) },
            new AuctionItem { Id = 11, Name = "Audi A4", Year = 2010, CurrentBid = 45000, ClosingDate = DateTime.Now.AddDays(11) },
            new AuctionItem { Id = 12, Name = "Volkswagen Golf", Year = 2005, CurrentBid = 22000, ClosingDate = DateTime.Now.AddDays(6) },
            new AuctionItem { Id = 13, Name = "Ford Focus", Year = 2012, CurrentBid = 30000, ClosingDate = DateTime.Now.AddDays(4) },
            new AuctionItem { Id = 14, Name = "Toyota Corolla", Year = 2008, CurrentBid = 28000, ClosingDate = DateTime.Now.AddDays(13) },
        };

        public IRelayCommand<AuctionItem> OpenBuyerDetailCommand { get; }
        public IRelayCommand<AuctionItem> OpenSellerDetailCommand { get; }

        public AuctionOverviewViewModel()
        {
            OpenBuyerDetailCommand = new RelayCommand<AuctionItem>(OpenBuyer);
            OpenSellerDetailCommand = new RelayCommand<AuctionItem>(OpenSeller);
        }

        public event EventHandler<AutoAuction_H2.ViewModels.ViewModelBase>? RequestNavigate;

        private void OpenBuyer(AuctionItem? item)
        {
            if (item is null) return;
            var vm = new AutoAuction_H2.ViewModels.Auction.AuctionDetailViewModel(item, isSeller: false);
            RequestNavigate?.Invoke(this, vm);
        }

        private void OpenSeller(AuctionItem? item)
        {
            if (item is null) return;
            var vm = new AutoAuction_H2.ViewModels.Auction.AuctionDetailViewModel(item, isSeller: true);
            RequestNavigate?.Invoke(this, vm);
        }
    }
}
