using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System.Collections;
using System.Windows.Input;

namespace AutoAuction_H2.Views.Auction.Controls;

public partial class AuctionTable : UserControl
{
    public static readonly StyledProperty<IEnumerable?> ItemsProperty =
        AvaloniaProperty.Register<AuctionTable, IEnumerable?>(nameof(Items));

    public static readonly StyledProperty<ICommand?> ActionCommandProperty =
        AvaloniaProperty.Register<AuctionTable, ICommand?>(nameof(ActionCommand));

    public static readonly StyledProperty<string?> ActionTextProperty =
        AvaloniaProperty.Register<AuctionTable, string?>(nameof(ActionText), "Byd");

    public IEnumerable? Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public ICommand? ActionCommand
    {
        get => GetValue(ActionCommandProperty);
        set => SetValue(ActionCommandProperty, value);
    }

    public string? ActionText
    {
        get => GetValue(ActionTextProperty);
        set => SetValue(ActionTextProperty, value);
    }

    public AuctionTable()
    {
        InitializeComponent();
        // Do not override DataContext -- allow it to inherit from parent view
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void OnGridDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (sender is DataGrid grid && ActionCommand is not null)
        {
            if (grid.SelectedItem is { } item && ActionCommand.CanExecute(item))
                ActionCommand.Execute(item);
        }
    }
}
