using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        private readonly AuthService _authService;
        private readonly UserService _userService;
        public event Action? LoggedIn;

        [ObservableProperty] private string username = "";
        [ObservableProperty] private string password = "";
        [ObservableProperty] private string confirmPassword = "";
        [ObservableProperty] private bool isCreatingUser = false;
        [ObservableProperty] private bool isPrivatUser = true;
        [ObservableProperty] private string cvrNumber = "";
        [ObservableProperty] private string cprNumber = "";
        [ObservableProperty] private string errorMessage = "";
        [ObservableProperty] private int zipCode = 0;

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        public LoginViewModel(AuthService authService, UserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        partial void OnErrorMessageChanged(string value) =>
            OnPropertyChanged(nameof(HasError));

        // -------------------------
        // LOGIN
        // -------------------------
        [RelayCommand]
        private async Task LoginAsync()
        {
            try
            {
                var (success, error, userId, userName, balance, userType) =
                    await _authService.LoginAsync(Username, Password);

                if (success)
                {
                    var app = AppState.Instance;
                    app.UserId = userId;
                    app.UserName = userName ?? "";
                    app.Balance = balance;
                    app.UserType = userType;
                    app.Message = "✅ Login succesfuldt";

                    ErrorMessage = "";
                    LoggedIn?.Invoke();
                }
                else
                {
                    ErrorMessage = $"❌ Login fejlede: {error}";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"❌ Fejl under login: {ex.Message}";
            }
        }

        // -------------------------
        // CREATE USER
        // -------------------------
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
                // 🔑 Hash password én gang på klienten
                var hash1 = HashHelper.Hash(Password);

                if (IsPrivatUser)
                {
                    var (success, error) = await _userService.CreatePrivateUserAsync(
                        Username,
                        hash1,
                        ZipCode,
                        0m,
                        CprNumber
                    );

                    if (success)
                        ResetFields();
                    else
                        ErrorMessage = $"❌ Brugeroprettelse fejlede: {error}";
                }
                else
                {
                    var (success, error) = await _userService.CreateCorporateUserAsync(
                        Username,
                        hash1,
                        ZipCode,
                        0m,
                        CvrNumber,
                        20000m
                    );

                    if (success)
                        ResetFields();
                    else
                        ErrorMessage = $"❌ Brugeroprettelse fejlede: {error}";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"❌ Fejl under oprettelse: {ex.Message}";
            }
        }

        // -------------------------
        // HELPER: Hash
        // -------------------------
        public static class HashHelper
        {
            public static string Hash(string input)
            {
                using var sha256 = SHA256.Create();
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        // -------------------------
        // HELPER: Reset input fields
        // -------------------------
        private void ResetFields()
        {
            ErrorMessage = "";
            IsCreatingUser = false;
            Password = ConfirmPassword = Username = CprNumber = CvrNumber = "";
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
