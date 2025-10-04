using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        public static string? CurrentUsername { get; private set; }
        public static decimal CurrentBalance { get; private set; }
        public event Action? LoggedIn;

        [ObservableProperty] private string username = "";
        [ObservableProperty] private string password = "";
        [ObservableProperty] private string confirmPassword = "";
        [ObservableProperty] private bool isCreatingUser = false;
        [ObservableProperty] private bool isPrivatUser = true;
        [ObservableProperty] private string cvrNumber = "";
        [ObservableProperty] private string cprNumber = "";

        [RelayCommand]
        private async Task LoginAsync()
        {
            LoggedIn?.Invoke();
            return;
            // Compute SHA256 hash of password
            var passwordBytes = Encoding.UTF8.GetBytes(Password);
            var passwordHash = Convert.ToBase64String(SHA256.HashData(passwordBytes));

            var loginRequest = new { Username, PasswordHash = passwordHash };

            using var client = new HttpClient { BaseAddress = new Uri("https://localhost:44372/") };
            var response = await client.PostAsJsonAsync("api/auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                CurrentUsername = Username;

                // Hent saldo fra API (forventet JSON: { "balance": 1234.56 })
                try
                {
                    var balanceResponse = await client.GetAsync($"api/account/balance?username={Username}");
                    if (balanceResponse.IsSuccessStatusCode)
                    {
                        var json = await balanceResponse.Content.ReadAsStringAsync();
                        using var doc = JsonDocument.Parse(json);
                        if (doc.RootElement.TryGetProperty("balance", out var balanceProp) && balanceProp.TryGetDecimal(out var balance))
                            CurrentBalance = balance;
                        else
                            CurrentBalance = 0;
                    }
                    else
                    {
                        CurrentBalance = 0;
                    }
                }
                catch
                {
                    CurrentBalance = 0;
                }

                LoggedIn?.Invoke();
            }
            else
            {
                // Show error message
            }
        }

        [RelayCommand]
        private async Task CreateUserAsync()
        {
            if (Password != ConfirmPassword)
            {
                // Show some error message in your UI
                return;
            }
            string PasswordHash = Password;
            var passwordBytes = Encoding.UTF8.GetBytes(PasswordHash);
            var clientHash = Convert.ToBase64String(SHA256.HashData(passwordBytes));

            var createUserRequest = new { Username, PasswordHash = clientHash };
            using var client = new HttpClient { BaseAddress = new Uri("https://localhost:44372/") };
            var response = await client.PostAsJsonAsync("api/auth/create", createUserRequest);

            if (response.IsSuccessStatusCode)
            {
                // Reset formen
                IsCreatingUser = false;
                Password = "";
                ConfirmPassword = "";
                Username = "";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Bruger oprettelse fejlede: {error}");
            }
        }

        [RelayCommand]
        private void ShowCreateUser()
        {
            IsCreatingUser = true;
            ConfirmPassword = "";
        }

        [RelayCommand]
        private void CancelCreateUser()
        {
            IsCreatingUser = false;
            Password = "";
            ConfirmPassword = "";
            Username = "";
        }
    }
}
