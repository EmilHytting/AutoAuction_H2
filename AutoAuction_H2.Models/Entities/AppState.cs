using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AutoAuction_H2.Models.Entities
{
    public class AppState : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // 🔹 Singleton-instans
        private static AppState? _instance;
        public static AppState Instance => _instance ??= new AppState();

        private string _userName = string.Empty;
        private decimal _balance;
        private decimal _reservedAmount;   // 👈 nyt felt
        private int _userId;
        private int _userType;
        private string _message;
        private int _yourAuctionsCount;
        private int _auctionsWonCount;
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
            set { _balance = value; OnChanged(); OnChanged(nameof(AvailableBalance)); }
        }

        public decimal ReservedAmount
        {
            get => _reservedAmount;
            set { _reservedAmount = value; OnChanged(); OnChanged(nameof(AvailableBalance)); }
        }

        // 👇 Computed property
        public decimal AvailableBalance => Balance - ReservedAmount;

        public int UserType
        {
            get => _userType;
            set { _userType = value; OnChanged(); }
        }

        public int YourAuctionsCount
        {
            get => _yourAuctionsCount;
            set { _yourAuctionsCount = value; OnChanged(); }
        }

        public int AuctionsWonCount
        {
            get => _auctionsWonCount;
            set { _auctionsWonCount = value; OnChanged(); }
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
