using AutoAuction_H2.ViewModels;
using AutoAuction_H2.Views.ContentPanels;
using Avalonia.Controls;
using Avalonia.Input;

namespace AutoAuction_H2.Views
{
    public partial class MainWindow : Window
    {
        private readonly LoginView _loginView;
        private readonly LoginViewModel _loginVm;
        private readonly MainView _mainView;
        private readonly MainViewModel _mainVm;

        public MainWindow(MainView mainView, MainViewModel mainVm, LoginView loginView, LoginViewModel loginVm)
        {
            InitializeComponent();

            _mainView = mainView;
            _mainVm = mainVm;
            _loginView = loginView;
            _loginVm = loginVm;

            // login → mainview
            _loginVm.LoggedIn += ShowMainView;

            _loginView.DataContext = _loginVm;
            MainContent.Content = _loginView;
        }

        private void Border_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                BeginMoveDrag(e);
        }

        private void ShowMainView()
        {
            // Her binder vi hele MainView (som indeholder LeftPanel + ContentPanel)
            _mainView.DataContext = _mainVm;
            MainContent.Content = _mainView;

            // Sørg for at navigation starter på Home
            _mainVm.LeftPanelViewModel.ShowHomeCommand.Execute(null);
        }
    }
}
