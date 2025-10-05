using Avalonia.Controls;
using Avalonia.Interactivity;
using AutoAuction_H2.ViewModels;

namespace AutoAuction_H2.Views.Windows;

public partial class ChangePasswordWindow : Window
{
    public ChangePasswordWindow()
    {
        InitializeComponent();

        // ✅ Inject AuthService fra App
        DataContext = new ChangePasswordViewModel(App.AuthService);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e) => Close();
}
