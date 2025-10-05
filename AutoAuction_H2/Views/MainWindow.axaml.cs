using AutoAuction_H2.Services;
using AutoAuction_H2.ViewModels;
using AutoAuction_H2.Views.ContentPanels;
using Avalonia.Controls;
using Avalonia.Input;

namespace AutoAuction_H2.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();



            // Opret login view + viewmodel
            var loginVm = new LoginViewModel();
            loginVm.LoggedIn += ShowMainView;

            var loginView = new LoginView { DataContext = loginVm };
            MainContent.Content = loginView;
        }

        private void Border_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                BeginMoveDrag(e);
        }

        public void ShowMainView()
        {
            // Login er gennemført → vis hovedview
            var mainView = new MainView();

            // ✅ Brug én fælles AuctionService baseret på AppState
            var auctionService = App.AuctionService;

            // ✅ ViewModel får service injiceret
            var vm = new MainViewModel(auctionService);

            mainView.DataContext = vm;
            MainContent.Content = mainView;
        }
    }
}
