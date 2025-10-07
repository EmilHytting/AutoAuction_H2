using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Services;
using AutoAuction_H2.Views.Windows;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;


namespace AutoAuction_H2.ViewModels
{
    public partial class UserProfileViewModel : ViewModelBase
    {
        [ObservableProperty] private string userName;
        [ObservableProperty] private decimal balance;
        [ObservableProperty] private int userType;
        [ObservableProperty] private decimal creditLimit;
        [ObservableProperty] private int yourAuctionsCount;
        [ObservableProperty] private int auctionsWonCount;

        public string UserTypeText => UserType == 0 ? "Privat" : "Professionel";

        // Commands
        public IRelayCommand LogoutCommand { get; }
        public IAsyncRelayCommand OpenProfileCommand { get; }

        private readonly INavigationService _navigation;

        public UserProfileViewModel(INavigationService navigation)
        {
            _navigation = navigation;

            LogoutCommand = new RelayCommand(DoLogout);
            OpenProfileCommand = new AsyncRelayCommand(DoOpenProfile);

            var app = AppState.Instance;

            // Init values fra AppState
            userName = app.UserName ?? "Ukendt";
            balance = app.Balance;
            userType = app.UserType;
            creditLimit = app.CreditLimit;

            // Lyt på AppState ændringer
            app.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(AppState.UserName))
                    UserName = app.UserName;

                if (e.PropertyName == nameof(AppState.Balance))
                    Balance = app.Balance;

                if (e.PropertyName == nameof(AppState.UserType))
                {
                    UserType = app.UserType;
                    OnPropertyChanged(nameof(UserTypeText));
                }

                if (e.PropertyName == nameof(AppState.CreditLimit))
                    CreditLimit = app.CreditLimit;
            };
        }

        private void DoLogout()
        {
            // Naviger tilbage til login
            _navigation.NavigateTo<LoginViewModel>();
        }

        private async Task DoOpenProfile()
        {
            var window = new UserProfileWindow();

            if (App.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
                && desktop.MainWindow is Window owner)
            {
                await window.ShowDialog(owner);
            }
            else
            {
                window.Show();
            }
        }
    }
}
