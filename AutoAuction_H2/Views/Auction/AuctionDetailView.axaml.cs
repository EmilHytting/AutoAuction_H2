using AutoAuction_H2.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AutoAuction_H2.Views.Auction;

public partial class AuctionDetailView : UserControl
{
    public AuctionDetailView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async void MakeBid_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not AuctionDetailViewModel vm)
            return;

        var dlg = new BidDialogWindow(vm.Item);

        if (VisualRoot is Window owner)
        {
            var amount = await dlg.ShowDialog<decimal?>(owner);
            if (amount is decimal d)
                await vm.PlaceBidCommand.ExecuteAsync(d);
        }
        else
        {
            dlg.Show();
        }
    }

}
