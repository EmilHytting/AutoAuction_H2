using AutoAuction_H2.Models;
using Avalonia.Controls.Primitives;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AutoAuction_H2.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public ObservableCollection<Vehicle> Vehicles { get; } = new ObservableCollection<Vehicle>();

    [ObservableProperty]
    private ViewModelBase? currentContent;

    public IRelayCommand ShowAuctionOverviewCommand { get; }

    public MainViewModel()
    {
        ShowAuctionOverviewCommand = new RelayCommand(ShowAuctionOverview);
        // Optionally set a default viewmodel
        // CurrentContent = new SomeDefaultViewModel();
    }

    private void ShowAuctionOverview()
    {
        CurrentContent = new AuctionOverviewViewModel();
    }
}
