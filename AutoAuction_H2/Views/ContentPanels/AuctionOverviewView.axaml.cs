using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AutoAuction_H2.Views.ContentPanels;

public partial class AuctionOverviewView : UserControl
{
    public AuctionOverviewView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
