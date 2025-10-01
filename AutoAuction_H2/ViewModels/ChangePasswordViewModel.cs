using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels;

public partial class ChangePasswordViewModel : ObservableObject
{
    [ObservableProperty] private string currentPassword = string.Empty;
    [ObservableProperty] private string newPassword = string.Empty;
    [ObservableProperty] private string confirmPassword = string.Empty;
    [ObservableProperty] private string? errorMessage;

    public IAsyncRelayCommand SaveCommand { get; }

    public ChangePasswordViewModel()
    {
        SaveCommand = new AsyncRelayCommand(SaveAsync);
    }

    private async Task SaveAsync()
    {
        ErrorMessage = null;

        if (string.IsNullOrWhiteSpace(LoginViewModel.CurrentUsername))
        {
            ErrorMessage = "Ingen bruger er logget ind.";
            return;
        }

        if (string.IsNullOrWhiteSpace(CurrentPassword) || string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            ErrorMessage = "Udfyld venligst alle felter.";
            return;
        }
        if (NewPassword != ConfirmPassword)
        {
            ErrorMessage = "Den nye adgangskode og bekræftelsen er ikke ens.";
            return;
        }
        if (NewPassword.Length < 6)
        {
            ErrorMessage = "Adgangskoden skal mindst være 6 tegn.";
            return;
        }

        try
        {
            // Hash passwords (SHA256) - server tilføjer efterfølgende sin egen salt og hasher igen
            static string Hash(string s)
            {
                var bytes = Encoding.UTF8.GetBytes(s);
                return Convert.ToBase64String(SHA256.HashData(bytes));
            }

            var request = new
            {
                Username = LoginViewModel.CurrentUsername,
                CurrentPasswordHash = Hash(CurrentPassword),
                NewPasswordHash = Hash(NewPassword)
            };

            using var client = new HttpClient { BaseAddress = new Uri("https://localhost:44372/") };
            var response = await client.PutAsJsonAsync("api/Auth/ChangePassword", request);

            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                ErrorMessage = string.IsNullOrWhiteSpace(msg) ? "Kunne ikke ændre adgangskode. Tjek nuværende adgangskode og prøv igen." : msg;
                return;
            }

            // Success: close via event
            PasswordChanged?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Der opstod en fejl: {ex.Message}";
        }
    }

    public event EventHandler? PasswordChanged;
}
