using Avalonia.Controls;
using Avalonia.Interactivity;
using AutoAuction_H2.Views.Windows;
using Avalonia.Markup.Xaml;

namespace AutoAuction_H2.Views.Auction.Controls;

public partial class AuctionSidebar : UserControl
{
    public AuctionSidebar()
    {
        InitializeComponent();
    }

    private async void Create_Click(object? sender, RoutedEventArgs e)
    {
        var win = new CreateAuctionWindow();
        if (VisualRoot is Window owner)
            await win.ShowDialog(owner);
        else
            win.Show();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
