using AutoAuction_H2.Models;
using Avalonia.Controls.Primitives;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoAuction_H2.ViewModels.Auction;

namespace AutoAuction_H2.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public ObservableCollection<Vehicle> Vehicles { get; } = new ObservableCollection<Vehicle>();

    [ObservableProperty]
    private ViewModelBase? currentContent;

    public IRelayCommand ShowAuctionOverviewCommand { get; }
    public IRelayCommand ShowAuctionDetailCommand { get; }
    public IRelayCommand ShowHomeCommand { get; }

    public MainViewModel()
    {
        CurrentContent = new HomeScreenViewModel(); // this is correct
        ShowAuctionOverviewCommand = new RelayCommand(ShowAuctionOverview);
        ShowAuctionDetailCommand = new RelayCommand(ShowAuctionDetailSample);
        ShowHomeCommand = new RelayCommand(ShowHomeScreen); ;

    }

    private void ShowAuctionOverview()
    {
        var vm = new AuctionOverviewViewModel();
        vm.RequestNavigate += Vm_RequestNavigate;
        CurrentContent = vm;
    }
    private void ShowHomeScreen()
    {
        CurrentContent = new HomeScreenViewModel();
    }

    private void Vm_RequestNavigate(object? sender, ViewModelBase e)
    {
        if (e is AuctionDetailViewModel detail)
        {
            detail.Closed += (_, __) => ShowAuctionOverview();
        }
        CurrentContent = e;
    }

    // Opens a sample AuctionDetailView inside ContentPanelView so sizing fits the app
    private void ShowAuctionDetailSample()
    {
        var sample = new Models.AuctionItem
        {
            Id = 99,
            Name = "Demo auktion",
            Year = 2021,
            CurrentBid = 123_000,
            ClosingDate = System.DateTime.Now.AddDays(7)
        };
        var vm = new AuctionDetailViewModel(sample, isSeller: false);
        vm.Closed += (_, __) => ShowAuctionOverview();
        CurrentContent = vm;
    }
}
