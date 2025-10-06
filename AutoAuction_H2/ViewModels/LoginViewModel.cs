using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoAuction_H2.Models.Entities;


namespace AutoAuction_H2.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private const string ApiBaseUrl = "https://localhost:44334/";
        public event Action? LoggedIn;

        [ObservableProperty] private string username = "";
        [ObservableProperty] private string password = "";
        [ObservableProperty] private string confirmPassword = "";
        [ObservableProperty] private bool isCreatingUser = false;
        [ObservableProperty] private bool isPrivatUser = true;
        [ObservableProperty] private string cvrNumber = "";
        [ObservableProperty] private string cprNumber = "";

        [ObservableProperty] private string errorMessage = "";
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        partial void OnErrorMessageChanged(string value) =>
            OnPropertyChanged(nameof(HasError));

        [RelayCommand]
        private async Task LoginAsync()
        {
            var app = AppState.Instance;
            app.UserId = 15;
            app.UserName = "Admin1";
            app.Balance = 5000;
            app.UserType = 1;
             LoggedIn?.Invoke();
            return;

            //try
            //{
            //    // Hash password på klienten
            //    var passwordBytes = Encoding.UTF8.GetBytes(Password);
            //    var clientHash = Convert.ToBase64String(SHA256.HashData(passwordBytes));

            //    var loginRequest = new
            //    {
            //        UserName = Username,
            //        Password = clientHash
            //    };

            //    using var client = new HttpClient { BaseAddress = new Uri(ApiBaseUrl) };
            //    var response = await client.PostAsJsonAsync("api/Auth/login", loginRequest);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var json = await response.Content.ReadAsStringAsync();
            //        using var doc = JsonDocument.Parse(json);

            //        // Parse API response
            //        var app = AppState.Instance;
            //        app.UserId = doc.RootElement.GetProperty("userId").GetInt32();
            //        app.Message = doc.RootElement.GetProperty("message").GetString() ?? "";
            //        app.UserName = doc.RootElement.GetProperty("userName").GetString() ?? "";
            //        app.Balance = doc.RootElement.GetProperty("balance").GetDecimal();
            //        app.UserType = doc.RootElement.GetProperty("userType").GetInt32();
            //        app.CreditLimit = doc.RootElement.GetProperty("CreditLimit").GetInt32();

            //        ErrorMessage = "";
            //        LoggedIn?.Invoke();
            //    }
            //    else
            //    {
            //        var error = await response.Content.ReadAsStringAsync();
            //        ErrorMessage = $"❌ Login fejlede: {error}";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ErrorMessage = $"❌ Fejl under login: {ex.Message}";
            //}
        }

        [RelayCommand]
        private async Task CreateUserAsync()
        {
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "❌ Password og gentag password matcher ikke";
                return;
            }

            try
            {
                var passwordBytes = Encoding.UTF8.GetBytes(Password);
                var clientHash = Convert.ToBase64String(SHA256.HashData(passwordBytes));

                var createUserRequest = new
                {
                    UserName = Username,
                    Password = clientHash,
                    Balance = 0m,
                    UserType = IsPrivatUser ? 0 : 1,
                    CreditLimit = IsPrivatUser ? 0m : 20000m
                };

                using var client = new HttpClient { BaseAddress = new Uri(ApiBaseUrl) };
                var response = await client.PostAsJsonAsync("api/Users", createUserRequest);

                if (response.IsSuccessStatusCode)
                {
                    ErrorMessage = "";
                    IsCreatingUser = false;

                    // Ryd form
                    Password = "";
                    ConfirmPassword = "";
                    Username = "";
                    CprNumber = "";
                    CvrNumber = "";
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    ErrorMessage = $"❌ Brugeroprettelse fejlede: {error}";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"❌ Fejl under oprettelse: {ex.Message}";
            }
        }

        [RelayCommand]
        private void ShowCreateUser()
        {
            ErrorMessage = "";
            IsCreatingUser = true;
            ConfirmPassword = "";
        }

        [RelayCommand]
        private void CancelCreateUser()
        {
            ErrorMessage = "";
            IsCreatingUser = false;
            Password = "";
            ConfirmPassword = "";
            Username = "";
        }
    }
}
