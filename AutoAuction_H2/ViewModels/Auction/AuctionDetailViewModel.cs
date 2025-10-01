using AutoAuction_H2.Models;
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
            ErrorMessage = "Bud skal være højere end nuværende bud.";
            return;
        }
        try
        {
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
}
