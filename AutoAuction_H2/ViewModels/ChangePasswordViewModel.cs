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
            // Hash passwords (SHA256)
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
            var response = await client.PostAsJsonAsync("api/auth/change-password", request);

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Kunne ikke ændre adgangskode. Tjek nuværende adgangskode og prøv igen.";
                return;
            }

            // Success: try close window via WeakReferenceMessenger or event
            PasswordChanged?.Invoke(this, EventArgs.Empty);
        }
        catch
        {
            ErrorMessage = "Der opstod en fejl. Prøv igen senere.";
        }
    }

    public event EventHandler? PasswordChanged;
}
