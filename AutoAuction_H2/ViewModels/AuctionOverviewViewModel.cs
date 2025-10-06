using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AutoAuction_H2.ViewModels
{
    public class AuctionOverviewViewModel : ViewModelBase
    {
        private readonly AuctionService _auctionService;
        public AuctionOverviewViewModel(AuctionService service) => _auctionService = service;
    }
}
