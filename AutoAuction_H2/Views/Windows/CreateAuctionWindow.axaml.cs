using Avalonia.Controls;
using AutoAuction_H2.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AutoAuction_H2.Views.Windows;

public partial class CreateAuctionWindow : Window
{
    // ✅ Parameterløs ctor til Avalonia
    public CreateAuctionWindow()
    {
        InitializeComponent();

        var vm = App.Services.GetRequiredService<CreateAuctionViewModel>();
        DataContext = vm;
        vm.AuctionCreated += (s, e) => Close();
    }

    // ✅ DI-venlig ctor (valgfri, kan bruges manuelt)
    public CreateAuctionWindow(CreateAuctionViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
        vm.AuctionCreated += (s, e) => Close();
    }

    private void Cancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) => Close();
}
