using AutoAuction_H2.Services;
using AutoAuction_H2.ViewModels;
using AutoAuction_H2.Views.ContentPanels;
using AutoAuction_H2.Views.Windows;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace AutoAuction_H2.Views.UIElements
{
    public partial class UserProfileCard : UserControl
    {
        private readonly AuthService _authService;

        public UserProfileCard()
        {
            InitializeComponent();
            _authService = App.Services.GetRequiredService<AuthService>();
            DataContext = App.Services.GetRequiredService<UserProfileViewModel>();
        }

        private void Logout_Click(object? sender, RoutedEventArgs e)
        {
            if (VisualRoot is Window window && window is MainWindow mainWindow)
            {
                var loginVm = new LoginViewModel();
                loginVm.LoggedIn += mainWindow.ShowMainView;

                mainWindow.MainContent.Content = new LoginView { DataContext = loginVm };
            }
        }

        private async void OpenProfile_Click(object? sender, RoutedEventArgs e)
        {
            var win = new UserProfileWindow(_authService);
            if (VisualRoot is Window owner)
                await win.ShowDialog(owner);
            else
                win.Show();
        }
    }

}
