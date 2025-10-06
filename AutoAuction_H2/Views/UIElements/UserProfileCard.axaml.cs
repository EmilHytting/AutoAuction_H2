using AutoAuction_H2.Services;
using AutoAuction_H2.ViewModels;
using AutoAuction_H2.Views.ContentPanels;
using AutoAuction_H2.Views.Windows;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AutoAuction_H2.Views.UIElements
{
    public partial class UserProfileCard : UserControl
    {
        private readonly AuthService _authService;
        private readonly UserProfileViewModel _vm;
        private readonly UserProfileWindow _profileWindow;
        private readonly LoginView _loginView;
        private readonly LoginViewModel _loginVm;

        public UserProfileCard(
            AuthService authService,
            UserProfileViewModel vm,
            UserProfileWindow profileWindow,
            LoginView loginView,
            LoginViewModel loginVm)
        {
            InitializeComponent();

            _authService = authService;
            _vm = vm;
            _profileWindow = profileWindow;
            _loginView = loginView;
            _loginVm = loginVm;

            DataContext = _vm;
        }

        private void Logout_Click(object? sender, RoutedEventArgs e)
        {
            if (VisualRoot is Window window && window is MainWindow mainWindow)
            {
                _loginVm.LoggedIn += mainWindow.ShowMainView;
                _loginView.DataContext = _loginVm;

                mainWindow.MainContent.Content = _loginView;
            }
        }

        private async void OpenProfile_Click(object? sender, RoutedEventArgs e)
        {
            if (VisualRoot is Window owner)
                await _profileWindow.ShowDialog(owner);
            else
                _profileWindow.Show();
        }
    }
}
