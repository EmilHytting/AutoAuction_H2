using Avalonia.Controls;
using Avalonia.Input;
using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.ViewModels;

namespace AutoAuction_H2.Views.ContentPanels
{
    public partial class MyBidsView : UserControl
    {
        public MyBidsView()
        {
            InitializeComponent();
        }
        
        private void DataGrid_OnDoubleTapped(object? sender, TappedEventArgs e)
        {
            if (DataContext is MyBidsViewModel vm &&
                sender is DataGrid grid &&
                grid.SelectedItem is AuctionEntity auction)
            {
                vm.OpenAuctionDetail(auction);
            }
        }

        private void DataGrid_OnContextRequested(object? sender, ContextRequestedEventArgs e)
        {
            if (DataContext is MyBidsViewModel vm &&
                sender is DataGrid grid &&
                grid.SelectedItem is AuctionEntity auction)
            {
                vm.OpenAuctionDetail(auction);
            }
        }

    }
}
