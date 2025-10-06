using CommunityToolkit.Mvvm.ComponentModel;
using AutoAuction_H2.Models.Entities;



namespace AutoAuction_H2.ViewModels
{
    public partial class UserProfileViewModel : ObservableObject
    {
        [ObservableProperty] private string userName;
        [ObservableProperty] private decimal balance;
        [ObservableProperty] private int userType;
        [ObservableProperty] private decimal creditLimit;
        [ObservableProperty] private int yourAuctionsCount;
        [ObservableProperty] private int auctionsWonCount;

        public string UserTypeText => UserType == 0 ? "Privat" : "Professionel";

        public UserProfileViewModel()
        {
            var app = AppState.Instance;

            userName = app.UserName;
            balance = app.Balance;
            userType = app.UserType;
            creditLimit = app.CreditLimit;

            // Lyt efter ændringer i AppState
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
    }
}
