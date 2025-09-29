using Avalonia.Controls;
using Avalonia.Interactivity;
using AutoAuction_H2.ViewModels;

namespace AutoAuction_H2.Views.Windows;

public partial class UserProfileWindow : Window
{
    public UserProfileWindow()
    {
        InitializeComponent();

        // Simple view model with current session data
        DataContext = new UserProfileViewModel
        {
            Username = LoginViewModel.CurrentUsername ?? "Unknown",
            Balance = LoginViewModel.CurrentBalance,
            YourAuctionsCount = 2,
            AuctionsWonCount = 1
        };
    }

    private void Close_Click(object? sender, RoutedEventArgs e) => Close();

    private async void ChangePassword_Click(object? sender, RoutedEventArgs e)
    {
        var dlg = new ChangePasswordWindow();

        if (dlg.DataContext is ChangePasswordViewModel vm)
        {
            void Handler(object? s, System.EventArgs e)
            {
                vm.PasswordChanged -= Handler;
                dlg.Close();
            }
            vm.PasswordChanged += Handler;
        }

        await dlg.ShowDialog(this);
    }
}
