using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels;

public partial class CreateAuctionViewModel : ViewModelBase
{
    [ObservableProperty] private string name = string.Empty;
    [ObservableProperty] private int? mileage;
    [ObservableProperty] private string regNumber = string.Empty;
    [ObservableProperty] private int? year;

    public IReadOnlyList<int> Years { get; } = Enumerable.Range(1990, DateTime.Now.Year - 1990 + 1).Reverse().ToList();

    public IReadOnlyList<string> VehicleTypes { get; } = new[] { "Bil", "Truck", "Bus", "Varevogn" };
    [ObservableProperty] private string? selectedVehicleType = "Bil";

    [ObservableProperty] private decimal? height;
    [ObservableProperty] private decimal? length;
    [ObservableProperty] private decimal? weight;
    [ObservableProperty] private decimal? engineSize;
    [ObservableProperty] private bool towBar;

    [ObservableProperty] private decimal? startingBid;
    [ObservableProperty] private DateTimeOffset? closeDate = DateTimeOffset.Now.Date.AddDays(7);

    [ObservableProperty] private string? errorMessage;

    public IAsyncRelayCommand CreateCommand { get; }

    public CreateAuctionViewModel()
    {
        CreateCommand = new AsyncRelayCommand(CreateAsync);
    }

    private async Task CreateAsync()
    {
        ErrorMessage = null;
        if (string.IsNullOrWhiteSpace(Name)) { ErrorMessage = "Udfyld navn."; return; }
        if (Year is null) { ErrorMessage = "Vælg årgang."; return; }
        if (StartingBid is null || StartingBid <= 0) { ErrorMessage = "Angiv startbud."; return; }
        if (CloseDate is null || CloseDate <= DateTimeOffset.Now.Date) { ErrorMessage = "Vælg slutdato frem i tiden."; return; }

        // TODO: Kald API for at oprette auktion
        await Task.Delay(100); // placeholder
        AuctionCreated?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? AuctionCreated;
}
