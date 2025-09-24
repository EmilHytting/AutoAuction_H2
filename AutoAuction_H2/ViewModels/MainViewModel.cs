using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoAuction_H2.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";
}