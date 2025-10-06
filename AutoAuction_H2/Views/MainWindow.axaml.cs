using AutoAuction_H2.ViewModels;
using AutoAuction_H2.Views.ContentPanels;
using Avalonia.Controls;
using Avalonia.Input;
using Microsoft.Extensions.DependencyInjection;

namespace AutoAuction_H2.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _mainVm;
        private readonly LoginViewModel _loginVm;
        private readonly LoginView _loginView;

        public MainWindow(MainViewModel mainVm, LoginViewModel loginVm, LoginView loginView)
        {
            InitializeComponent();

            _mainVm = mainVm;
            _loginVm = loginVm;
            _loginView = loginView;

            _loginVm.LoggedIn += ShowMainView;
            _loginView.DataContext = _loginVm;

            MainContent.Content = _loginView;
        }

        private void Border_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                BeginMoveDrag(e);
        }

        public void ShowMainView()
        {
            var mainView = App.Services.GetRequiredService<MainView>();
            mainView.DataContext = _mainVm;
            MainContent.Content = mainView;
        }
    }
}
