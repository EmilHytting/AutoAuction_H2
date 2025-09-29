using AutoAuction_H2.ViewModels;

namespace AutoAuction_H2.ViewModels;

public class UserProfileViewModel : ViewModelBase
{
    public string Username { get; set; } = "";
    public decimal Balance { get; set; }
    public int YourAuctionsCount { get; set; }
    public int AuctionsWonCount { get; set; }
}
