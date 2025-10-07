using AutoAuction_H2.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace AutoAuction_H2.Views.ContentPanels;

public partial class TrucksView : UserControl
{
    public TrucksView()
    {
        InitializeComponent();
    }

    private void DataGrid_OnContextRequested(object? sender, ContextRequestedEventArgs e)
    {
        if (sender is DataGrid grid && grid.SelectedItem is AuctionEntity auction)
        {
            if (DataContext is TrucksViewModel vm)
                vm.OpenAuctionDetail(auction);
        }
    }

    private void DataGrid_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (sender is DataGrid grid && grid.SelectedItem is AuctionEntity auction)
        {
            if (DataContext is TrucksViewModel vm)
                vm.OpenAuctionDetail(auction);
        }
    }
}

