using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Models.Persistence;
using AutoAuction_H2.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels;

public partial class CreateAuctionViewModel : ViewModelBase
{
    // -------- Basis info --------
    [ObservableProperty] private string name = string.Empty;
    [ObservableProperty] private int? mileage;
    [ObservableProperty] private string regNumber = string.Empty;
    [ObservableProperty] private int? year;
    [ObservableProperty] private bool towBar;
    [ObservableProperty] private double? motorSize;
    [ObservableProperty] private double? fuelEfficiency;
    [ObservableProperty] private int fuelType;

    public IReadOnlyList<int> Years { get; } =
        Enumerable.Range(1990, DateTime.Now.Year - 1990 + 1).Reverse().ToList();

    public IReadOnlyList<string> VehicleTypes { get; } =
        new[] { "Bil", "Truck", "Bus", "Varevogn" };

    [ObservableProperty] private string? selectedVehicleType = "Bil";

    // -------- Bil --------
    [ObservableProperty] private int? seats;
    [ObservableProperty] private int? trunkSize;
    [ObservableProperty] private bool isofix;

    // -------- Varevogn --------
    [ObservableProperty] private bool safetyBar;
    [ObservableProperty] private double? loadCapacity;

    // -------- Truck --------
    [ObservableProperty] private double? height;
    [ObservableProperty] private double? length;
    [ObservableProperty] private double? width;

    // -------- Bus --------
    [ObservableProperty] private int? doors;
    [ObservableProperty] private bool hasToilet;

    // -------- Auktion --------
    [ObservableProperty] private decimal? startingBid;
    [ObservableProperty] private DateTimeOffset? closeDate = DateTimeOffset.Now.Date.AddDays(7);
    [ObservableProperty] private TimeSpan closeTime = new(12, 0, 0);
    [ObservableProperty] private string? errorMessage;
    [ObservableProperty] private string? successMessage;

    public DateTime EndTime => (CloseDate ?? DateTimeOffset.Now).Date.Add(CloseTime);

    // -------- Visibility helpers --------
    partial void OnSelectedVehicleTypeChanged(string? value)
    {
        OnPropertyChanged(nameof(IsCar));
        OnPropertyChanged(nameof(IsTruck));
        OnPropertyChanged(nameof(IsBus));
        OnPropertyChanged(nameof(IsVan));
    }

    public bool IsCar => SelectedVehicleType == "Bil";
    public bool IsTruck => SelectedVehicleType == "Truck";
    public bool IsBus => SelectedVehicleType == "Bus";
    public bool IsVan => SelectedVehicleType == "Varevogn";

    // -------- Commands --------
    public IAsyncRelayCommand CreateCommand { get; }

    private readonly AuctionService _auctionService;
    private readonly VehicleFactory _vehicleFactory;

    public CreateAuctionViewModel(AuctionService auctionService, VehicleFactory vehicleFactory)
    {
        _auctionService = auctionService;
        _vehicleFactory = vehicleFactory;
        CreateCommand = new AsyncRelayCommand(CreateAsync);
    }

    private async Task CreateAsync()
    {
        ErrorMessage = null;
        SuccessMessage = null;

        // ---- Validation ----
        if (string.IsNullOrWhiteSpace(Name)) { ErrorMessage = "Udfyld navn."; return; }
        if (string.IsNullOrWhiteSpace(RegNumber)) { ErrorMessage = "Udfyld registreringsnummer."; return; }
        if (Year is null) { ErrorMessage = "Vælg årgang."; return; }
        if (MotorSize is null) { ErrorMessage = "Angiv motorstørrelse."; return; }
        if (StartingBid is null || StartingBid <= 0) { ErrorMessage = "Angiv startbud."; return; }

        // ---- Vehicle via factory ----
        VehicleEntity vehicle;
        try
        {
            vehicle = _vehicleFactory.Create(SelectedVehicleType, this);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Fejl i køretøjsoprettelse: {ex.Message}";
            return;
        }

        // ---- Auction build ----
        var auction = new AuctionEntity
        {
            Vehicle = vehicle,
            SellerId = AppState.Instance.UserId, // ✅ Brugerens ID fra login/session
            MinPrice = StartingBid ?? 0,
            CurrentBid = 0,
            IsSold = false,
            EndTime = EndTime
        };

        try
        {
            var (success, error) = await _auctionService.CreateAuctionAsync(auction);

            if (!success)
            {
                ErrorMessage = error ?? "Kunne ikke oprette auktion.";
                return;
            }

            SuccessMessage = "✅ Auktion oprettet!";
            AuctionCreated?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Fejl: {ex.Message}";
        }
    }

    public event EventHandler? AuctionCreated;
}
