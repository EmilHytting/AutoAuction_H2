using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using AutoAuction_H2.Views.ContentPanels;
using AutoAuction_H2.ViewModels;

namespace AutoAuction_H2.Views.UIElements;

public partial class UserProfileCard : UserControl
{
    private TextBlock? _usernameTextBlock;
    private TextBlock? _balanceTextBlock;

    public UserProfileCard()
    {
        InitializeComponent();
        this.AttachedToVisualTree += OnAttachedToVisualTree;
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        _usernameTextBlock = this.FindControl<TextBlock>("UsernameText");
        if (_usernameTextBlock != null)
            _usernameTextBlock.Text = LoginViewModel.CurrentUsername ?? "Brugernavn";

        _balanceTextBlock = this.FindControl<TextBlock>("BalanceText");
        if (_balanceTextBlock != null)
            _balanceTextBlock.Text = $"Saldo: {LoginViewModel.CurrentBalance:N0} kr";
    }

    private void Logout_Click(object? sender, RoutedEventArgs e)
    {
        if (VisualRoot is Window window && window is MainWindow mainWindow)
        {
            var loginVm = new LoginViewModel();
            loginVm.LoggedIn += mainWindow.ShowMainView;
            var loginView = new LoginView { DataContext = loginVm };
            mainWindow.MainContent.Content = loginView;
        }
    }
}