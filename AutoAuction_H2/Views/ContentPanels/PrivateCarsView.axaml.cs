using AutoAuction_H2.ViewModels;
using Avalonia.Controls;
using Avalonia.Input;

namespace AutoAuction_H2.Views.ContentPanels
{
    public partial class PrivateCarsView : UserControl
    {
        public PrivateCarsView()
        {
            InitializeComponent();
        }

        private void DataGrid_OnContextRequested(object? sender, ContextRequestedEventArgs e)
        {
            if (sender is DataGrid grid && grid.SelectedItem is AuctionEntity auction)
            {
                if (DataContext is PrivateCarsViewModel vm)
                    vm.OpenAuctionDetail(auction);
            }
        }

        private void DataGrid_OnDoubleTapped(object? sender, TappedEventArgs e)
        {
            if (sender is DataGrid grid && grid.SelectedItem is AuctionEntity auction)
            {
                if (DataContext is PrivateCarsViewModel vm)
                    vm.OpenAuctionDetail(auction);
            }
        }
    }
}
