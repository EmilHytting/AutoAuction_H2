using AutoAuction_H2.ViewModels;
using Avalonia.Controls;
using Avalonia.Input;

namespace AutoAuction_H2.Views.ContentPanels
{

    public partial class BusesView : UserControl
    {
        public BusesView()
        {
            InitializeComponent();
        }

        private void DataGrid_OnContextRequested(object? sender, ContextRequestedEventArgs e)
        {
            if (sender is DataGrid grid && grid.SelectedItem is AuctionEntity auction)
            {
                if (DataContext is BusesViewModel vm)
                    vm.OpenAuctionDetail(auction);
            }
        }

        private void DataGrid_OnDoubleTapped(object? sender, TappedEventArgs e)
        {
            if (sender is DataGrid grid && grid.SelectedItem is AuctionEntity auction)
            {
                if (DataContext is BusesViewModel vm)
                    vm.OpenAuctionDetail(auction);
            }
        }
    }
}