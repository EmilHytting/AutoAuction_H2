using AutoAuction_H2.Models.Entities;
using Avalonia.Controls;
using Avalonia.Input;
using System;

namespace AutoAuction_H2.Views.Auction
{
    public partial class BidDialogWindow : Window
    {
        private readonly AuctionEntity _auction;

        public decimal EnteredBid => BidInput.Value ?? 0;

        public BidDialogWindow(AuctionEntity auction)
        {
            InitializeComponent();
            _auction = auction;

            // Minimum skal være enten MinPrice eller CurrentBid + 1
            var minimum = auction.CurrentBid > 0
                ? Math.Max(auction.MinPrice, auction.CurrentBid + 1)
                : auction.MinPrice;

            BidInput.Minimum = minimum;
            BidInput.Value = minimum; // startværdi
        }

        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                BeginMoveDrag(e);
        }

        private void Cancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
            => Close(null);

        private void Bid_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
            => Close(EnteredBid);
    }
}
