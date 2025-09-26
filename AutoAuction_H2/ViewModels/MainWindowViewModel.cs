using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoAuction_H2.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private object currentView;

        public MainWindowViewModel()
        {
            // Start with the login screen
            var loginVm = new LoginViewModel();
            loginVm.LoggedIn += OnLoggedIn;
            CurrentView = loginVm;
        }

        private void OnLoggedIn()
        {
            // Switch to MainViewModel when login succeeds
            CurrentView = new MainViewModel();
        }
    }
}
