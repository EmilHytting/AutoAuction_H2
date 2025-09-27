using CommunityToolkit.Mvvm.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoAuction_H2.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json.Serialization.Metadata;

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
