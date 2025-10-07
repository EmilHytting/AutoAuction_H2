using AutoAuction_H2.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using Xunit.Sdk;
using AutoAuction_H2.Models.Entities;

namespace AutoAuction_H2.ViewModels;

public partial class ChangePasswordViewModel : ObservableObject
{
    private readonly AuthService _authService;

    [ObservableProperty] private string currentPassword = string.Empty;
    [ObservableProperty] private string newPassword = string.Empty;
    [ObservableProperty] private string confirmPassword = string.Empty;
    [ObservableProperty] private string? errorMessage;
    [ObservableProperty] private string? successMessage;

    public IAsyncRelayCommand SaveCommand { get; }

    public ChangePasswordViewModel(AuthService authService)
    {
        _authService = authService;
        SaveCommand = new AsyncRelayCommand(SaveAsync);
    }

    private async Task SaveAsync()
    {
        ErrorMessage = null;
        SuccessMessage = null;

        if (AppState.Instance.UserId <= 0)
        {
            ErrorMessage = "Ingen bruger er logget ind.";
            return;
        }

        if (NewPassword != ConfirmPassword)
        {
            ErrorMessage = "Den nye adgangskode og bekræftelsen er ikke ens.";
            return;
        }

        // ✅ Hent også username fra AppState
        var (success, error) = await _authService.ChangePasswordAsync(
            AppState.Instance.UserId,
            NewPassword,
            AppState.Instance.UserName);

        if (!success)
        {
            ErrorMessage = $"❌ {error}";
            return;
        }

        SuccessMessage = "✅ Adgangskoden blev ændret.";
        PasswordChanged?.Invoke(this, EventArgs.Empty);
    }



    public event EventHandler? PasswordChanged;
}
