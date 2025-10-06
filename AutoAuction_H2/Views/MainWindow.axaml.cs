using AutoAuction_H2.ViewModels;
using Avalonia; // 👈 dette er vigtigt for Application.Current
using Avalonia.Controls;
using Avalonia.Input;
using Microsoft.Extensions.DependencyInjection;

namespace AutoAuction_H2.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _mainVm;

        public MainWindow()
        {
            InitializeComponent();

            // Hent MainViewModel fra DI containeren
            _mainVm = App.Services.GetRequiredService<MainViewModel>();

            // Start med login-view
            var loginVm = App.Services.GetRequiredService<LoginViewModel>();
            loginVm.LoggedIn += ShowMainView;

            MainContent.Content = new LoginView { DataContext = loginVm };
        }

        private void Border_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                BeginMoveDrag(e);
        }

        public void ShowMainView()
        {
            var mainView = new MainView
            {
                DataContext = _mainVm
            };

            MainContent.Content = mainView;
        }
    }
}
