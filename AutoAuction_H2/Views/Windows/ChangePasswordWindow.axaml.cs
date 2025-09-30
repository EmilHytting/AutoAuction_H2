using Avalonia.Controls;
using Avalonia.Interactivity;
using AutoAuction_H2.ViewModels;

namespace AutoAuction_H2.Views.Windows;

public partial class ChangePasswordWindow : Window
{
    public ChangePasswordWindow()
    {
        InitializeComponent();
        DataContext = new ChangePasswordViewModel();
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e) => Close();
}
