using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AutoAuction_H2.Views.Auction;

public partial class BidDialogWindow : Window
{
    public BidDialogWindow()
    {
        InitializeComponent();
    }

    public BidDialogWindow(decimal min)
    {
        InitializeComponent();
        if (this.FindControl<NumericUpDown>("BidInput") is { } updown)
        {
            updown.Minimum = min;
            updown.Value = min;
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            BeginMoveDrag(e);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e) => Close(null);
    private void Bid_Click(object? sender, RoutedEventArgs e)
    {
        if (this.FindControl<NumericUpDown>("BidInput") is { } updown && updown.Value is decimal d)
            Close(d);
        else
            Close(null);
    }
}
