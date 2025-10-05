using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AutoAuction_H2
{
    public class AppState : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // 🔹 Singleton-instans
        private static AppState? _instance;
        public static AppState Instance => _instance ??= new AppState();

        private string _userName = string.Empty;
        private decimal _balance;
        private int _userId;
        private int _userType;
        private string _message;
        private string _apiBaseUrl = "https://localhost:44334/";
        private int _creditLimit;

        public int CreditLimit
        {
            get => _creditLimit;
            set { _creditLimit = value; OnChanged(); }
        }

        public string Message
        {
            get => _message;
            set { _message = value; OnChanged(); }
        }
        public int UserId
        {
            get => _userId;
            set { _userId = value; OnChanged(); }
        }

        public string UserName
        {
            get => _userName;
            set { _userName = value; OnChanged(); }
        }

        public decimal Balance
        {
            get => _balance;
            set { _balance = value; OnChanged(); }
        }

        public int UserType
        {
            get => _userType;
            set { _userType = value; OnChanged(); }
        }

        public string ApiBaseUrl
        {
            get => _apiBaseUrl;
            set { _apiBaseUrl = value; OnChanged(); }
        }

        private void OnChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
