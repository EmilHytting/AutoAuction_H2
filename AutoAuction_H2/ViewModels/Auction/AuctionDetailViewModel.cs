using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace AutoAuction_H2.ViewModels
{
    public partial class AuctionDetailViewModel : ViewModelBase
    {
        private readonly AuctionService _auctionService;
        private readonly INavigationService _navigation;

        [ObservableProperty] private AuctionEntity item;
        [ObservableProperty] private bool isSeller;
        [ObservableProperty] private string? errorMessage;

        public bool IsBuyer => !IsSeller;

        // ✅ Liste til budhistorik
        public ObservableCollection<string> BidHistory { get; } = new();

        public IRelayCommand BackCommand { get; }
        public IAsyncRelayCommand<decimal> PlaceBidCommand { get; }

        public event EventHandler? Closed;

        public AuctionDetailViewModel(AuctionEntity item, bool isSeller, AuctionService auctionService, INavigationService navigation)
        {
            _auctionService = auctionService;
            _navigation = navigation;

            this.item = item;
            this.isSeller = isSeller;

            BackCommand = new RelayCommand(OnBack);
            PlaceBidCommand = new AsyncRelayCommand<decimal>(PlaceBidAsync);

            // Tilføj initial bid til historik
            if (item.CurrentBid > 0)
                BidHistory.Add($"{DateTime.Now:t} Startbud: {item.CurrentBid:N0} kr");
        }

        private async Task PlaceBidAsync(decimal amount)
        {
            ErrorMessage = null;

            // ❌ bud under minimumspris
            if (amount < Item.MinPrice)
            {
                ErrorMessage = $"Buddet skal være mindst {Item.MinPrice:N0} kr.";
                return;
            }

            // ❌ bud under eller lig med nuværende
            if (amount <= Item.CurrentBid)
            {
                ErrorMessage = "Buddet skal være højere end det nuværende.";
                return;
            }

            try
            {
                var (success, error) = await _auctionService.PlaceBidAsync(Item.Id, AppState.Instance.UserId, amount);

                if (!success)
                {
                    ErrorMessage = error ?? "Buddet blev afvist af serveren.";
                    return;
                }

                // ✅ Opdater CurrentBid
                Item.CurrentBid = amount;

                // ✅ Tilføj til historik
                BidHistory.Insert(0, $"{DateTime.Now:t} {AppState.Instance.UserName}: {amount:N0} kr");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }


        private void OnBack()
        {
            // Gå tilbage til oversigten
            _navigation.NavigateTo<AuctionOverviewViewModel>();
        }
    }
}
