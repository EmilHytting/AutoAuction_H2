using AutoAuction_H2.Models;
using AutoAuction_H2.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels.Auction;

public partial class AuctionDetailViewModel : AutoAuction_H2.ViewModels.ViewModelBase
{
    [ObservableProperty] private AuctionItem item;
    [ObservableProperty] private bool isSeller;
    [ObservableProperty] private string? errorMessage;

    public bool IsBuyer => !IsSeller;

    public ObservableCollection<string> BidHistory { get; } = new();

    public IRelayCommand BackCommand { get; }
    public IAsyncRelayCommand PlaceBidCommand { get; }
    public IAsyncRelayCommand AcceptBidCommand { get; }

    public event EventHandler? Closed;

    public AuctionDetailViewModel(AuctionItem item, bool isSeller)
    {
        this.item = item;
        this.isSeller = isSeller;

        BackCommand = new RelayCommand(() => Closed?.Invoke(this, EventArgs.Empty));
        PlaceBidCommand = new AsyncRelayCommand<decimal>(PlaceBidAsync);
        AcceptBidCommand = new AsyncRelayCommand(AcceptAsync);
    }

    private async Task PlaceBidAsync(decimal amount)
    {
        ErrorMessage = null;
        if (amount <= Item.CurrentBid)
        {
            ErrorMessage = "Bid must be higher than current bid.";
            return;
        }
        try
        {
            //DOMAIN RULES VALIDATION 
            User buyer = GetCurrentUser();
            if (buyer != null && !DomainRules.CanBuy(buyer, amount))
            {
                ErrorMessage = "Ingen tilstrækkelig saldo eller kredit.";
                return;
            }
            // TODO: call API to place bid
            await Task.Delay(200);
            Item = new AuctionItem { Id = Item.Id, Name = Item.Name, Year = Item.Year, CurrentBid = amount, ClosingDate = Item.ClosingDate };
            BidHistory.Insert(0, $"{DateTime.Now:t} {LoginViewModel.CurrentUsername}: {amount:N0} kr");
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    private async Task AcceptAsync()
    {
        ErrorMessage = null;
        try
        {
            // TODO: call API to accept
            await Task.Delay(200);
            Closed?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    // Tilføj en metode til at hente den aktuelle bruger
    private User? GetCurrentUser()
    {
        // TODO: Returner den aktuelle bruger fra systemet
        // Fx via LoginViewModel eller en bruger-service
        return null;
    }
}
