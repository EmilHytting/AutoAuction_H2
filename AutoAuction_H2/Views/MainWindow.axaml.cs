using AutoAuction_H2.ViewModels;
using AutoAuction_H2.Views.ContentPanels;
using Avalonia.Controls;
using Avalonia.Input;

namespace AutoAuction_H2.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _mainVm;
        private readonly LoginView _loginView;
        private readonly LoginViewModel _loginVm;

        public MainWindow(MainViewModel mainVm, LoginView loginView, LoginViewModel loginVm)
        {
            InitializeComponent();

            _mainVm = mainVm;
            _loginView = loginView;
            _loginVm = loginVm;

            // bind login viewmodel til view
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
            var mainView = new MainView
            {
                DataContext = _mainVm
            };

            MainContent.Content = mainView;
        }
    }
}
