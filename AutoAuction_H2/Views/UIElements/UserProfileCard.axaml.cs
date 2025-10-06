using AutoAuction_H2.ViewModels;
using AutoAuction_H2.Views.ContentPanels;
using AutoAuction_H2.Views.Windows;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;

namespace AutoAuction_H2.Views.UIElements
{
    public partial class UserProfileCard : UserControl
    {
        private readonly UserProfileViewModel _vm;

        public UserProfileCard(UserProfileViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
        }

        private void Logout_Click(object? sender, RoutedEventArgs e)
        {
            if (VisualRoot is Window window && window is MainWindow mainWindow)
            {
                var loginVm = App.Services.GetRequiredService<LoginViewModel>();
                loginVm.LoggedIn += mainWindow.ShowMainView;

                mainWindow.MainContent.Content = new LoginView { DataContext = loginVm };
            }
        }

        private async void OpenProfile_Click(object? sender, RoutedEventArgs e)
        {
            var win = App.Services.GetRequiredService<UserProfileWindow>();
            if (VisualRoot is Window owner)
                await win.ShowDialog(owner);
            else
                win.Show();
        }
    }
}
