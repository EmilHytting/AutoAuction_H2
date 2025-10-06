using AutoAuction_H2.Services;
using AutoAuction_H2.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;


namespace AutoAuction_H2.Views.Windows;

public partial class ChangePasswordWindow : Window
{
    public ChangePasswordWindow(AuthService authService)
    {
        InitializeComponent();

        // ✅ Inject AuthService fra App
        DataContext = new ChangePasswordViewModel(authService);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e) => Close();
}
