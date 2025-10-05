using AutoAuction_H2.ViewModels;
using AutoAuction_H2.Views.ContentPanels;
using AutoAuction_H2.Views.Windows;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AutoAuction_H2.Views.UIElements
{
    public partial class UserProfileCard : UserControl
    {
        public UserProfileCard()
        {
            InitializeComponent();
            DataContext = new UserProfileViewModel();
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
            var win = new UserProfileWindow();
            if (VisualRoot is Window owner)
                await win.ShowDialog(owner);
            else
                win.Show();
        }
    }
}
