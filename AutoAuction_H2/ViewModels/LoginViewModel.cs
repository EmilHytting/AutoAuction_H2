using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using AutoAuction_H2.Services;
using AutoAuction_H2.Models.Entities;

namespace AutoAuction_H2.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        private readonly AuthService _authService;
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

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
        }

        partial void OnErrorMessageChanged(string value) =>
            OnPropertyChanged(nameof(HasError));

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
                // Kald dit UsersController endpoint via HttpClient eller en UserService
                var createUserRequest = new
                {
                    UserName = Username,
                    Password = Password, // AuthService dobbelthasher på server
                    Balance = 0m,
                    UserType = IsPrivatUser ? 0 : 1,
                    CreditLimit = IsPrivatUser ? 0m : 20000m
                };

                var response = await _authService.RegisterUserAsync(createUserRequest);

                if (response.success)
                {
                    ErrorMessage = "";
                    IsCreatingUser = false;
                    Password = ConfirmPassword = Username = CprNumber = CvrNumber = "";
                }
                else
                {
                    ErrorMessage = $"❌ Brugeroprettelse fejlede: {response.error}";
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
