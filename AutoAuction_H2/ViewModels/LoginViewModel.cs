using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        public event Action? LoggedIn;

        [ObservableProperty] private string username = "";
        [ObservableProperty] private string password = "";
        [ObservableProperty] private string confirmPassword = "";
        [ObservableProperty] private bool isCreatingUser = false;

        [RelayCommand]
        private async Task LoginAsync()
        {
            // Compute SHA256 hash of password
            var passwordBytes = Encoding.UTF8.GetBytes(Password);
            var passwordHash = Convert.ToBase64String(SHA256.HashData(passwordBytes));

            var loginRequest = new { Username, PasswordHash = passwordHash };

            using var client = new HttpClient { BaseAddress = new Uri("https://localhost:44372/") };
            var response = await client.PostAsJsonAsync("api/auth/login", loginRequest);


            if (response.IsSuccessStatusCode)
            {
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
            // Hash the password (client-side)
            var passwordBytes = Encoding.UTF8.GetBytes(PasswordHash);
            var clientHash = Convert.ToBase64String(SHA256.HashData(passwordBytes));

            var createUserRequest = new { Username, PasswordHash = clientHash };

            using var client = new HttpClient { BaseAddress = new Uri("https://localhost:44372/") };
            var response = await client.PostAsJsonAsync("api/auth/create", createUserRequest);

            if (response.IsSuccessStatusCode)
            {
                // Reset form
                IsCreatingUser = false;
                Password = "";
                ConfirmPassword = "";
                Username = "";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"User creation failed: {error}");
            }
        }

        [RelayCommand]
        private void ShowCreateUser()
        {
            IsCreatingUser = true;
            ConfirmPassword = ""; // reset previous value
        }
    }
}
