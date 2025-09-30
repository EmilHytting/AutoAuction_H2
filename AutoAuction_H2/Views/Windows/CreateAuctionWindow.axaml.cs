using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using AutoAuction_H2.ViewModels;

namespace AutoAuction_H2.Views.Windows;

public partial class CreateAuctionWindow : Window
{
    public CreateAuctionWindow()
    {
        InitializeComponent();
        var vm = new CreateAuctionViewModel();
        vm.AuctionCreated += OnAuctionCreated;
        DataContext = vm;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async void OnAuctionCreated(object? sender, System.EventArgs e)
    {
        var toast = new ToastWindow();
        await toast.ShowAsync("Auktion oprettet", this);
        Close();
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e) => Close();
}
