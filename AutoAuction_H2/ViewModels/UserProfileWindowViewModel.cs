using AutoAuction_H2.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoAuction_H2.ViewModels;

public partial class UserProfileWindowViewModel : ViewModelBase
{
    [ObservableProperty] private string userName;
    [ObservableProperty] private decimal balance;
    [ObservableProperty] private int userType;
    [ObservableProperty] private decimal creditLimit;
    [ObservableProperty] private int yourAuctionsCount;
    [ObservableProperty] private int auctionsWonCount;

    public string UserTypeText => UserType == 0 ? "Privat" : "Professionel";

    public UserProfileWindowViewModel(AppState app)
    {
        userName = app.UserName ?? "Ukendt";
        balance = app.Balance;
        userType = app.UserType;
        creditLimit = app.CreditLimit;
        yourAuctionsCount = app.YourAuctionsCount;
        auctionsWonCount = app.AuctionsWonCount;

        app.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(AppState.YourAuctionsCount))
                YourAuctionsCount = app.YourAuctionsCount;
            if (e.PropertyName == nameof(AppState.AuctionsWonCount))
                AuctionsWonCount = app.AuctionsWonCount;
        };
    }

}
