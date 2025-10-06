using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels
{
    public partial class AuctionDetailViewModel : ViewModelBase
    {
        private readonly AuctionService _auctionService;

        [ObservableProperty] private AuctionEntity item;
        [ObservableProperty] private bool isSeller;
        [ObservableProperty] private string? errorMessage;

        public bool IsBuyer => !IsSeller;

        public ObservableCollection<string> BidHistory { get; } = new();

        public IRelayCommand BackCommand { get; }
        public IAsyncRelayCommand<decimal> PlaceBidCommand { get; }

        public event EventHandler? Closed;

        // ✅ AuctionService injiceres via DI
        public AuctionDetailViewModel(AuctionEntity item, bool isSeller, AuctionService auctionService)
        {
            _auctionService = auctionService;
            this.item = item;
            this.isSeller = isSeller;

            BackCommand = new RelayCommand(() => Closed?.Invoke(this, EventArgs.Empty));
            PlaceBidCommand = new AsyncRelayCommand<decimal>(PlaceBidAsync);
        }

        private async Task PlaceBidAsync(decimal amount)
        {
            ErrorMessage = null;

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

                // Lokal UI-opdatering
                Item.CurrentBid = amount;
                BidHistory.Insert(0, $"{DateTime.Now:t} {AppState.Instance.UserName}: {amount:N0} kr");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
