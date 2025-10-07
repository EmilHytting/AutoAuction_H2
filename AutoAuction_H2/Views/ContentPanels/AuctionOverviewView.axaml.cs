using Avalonia.Controls;
using Avalonia.Input;
using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.ViewModels;

namespace AutoAuction_H2.Views.ContentPanels
{
    public partial class AuctionOverviewView : UserControl
    {
        public AuctionOverviewView()
        {
            InitializeComponent();
        }

        private void DataGrid_OnContextRequested(object? sender, ContextRequestedEventArgs e)
        {
            if (sender is DataGrid grid && grid.SelectedItem is AuctionEntity auction)
            {
                if (DataContext is AuctionOverviewViewModel vm)
                    vm.OpenAuctionDetail(auction);
            }
        }

        private void DataGrid_OnDoubleTapped(object? sender, TappedEventArgs e)
        {
            if (sender is DataGrid grid && grid.SelectedItem is AuctionEntity auction)
            {
                if (DataContext is AuctionOverviewViewModel vm)
                    vm.OpenAuctionDetail(auction);
            }
        }
    }
}
