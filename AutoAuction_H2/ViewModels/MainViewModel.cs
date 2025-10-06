using System.ComponentModel;
using AutoAuction_H2.Services;
using AutoAuction_H2.Views.UIElements;

namespace AutoAuction_H2.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigation;
        private readonly AuctionService _auctionService;

        public UserProfileCard UserProfileCard { get; }
        public LeftPanelViewModel LeftPanel { get; }

        public MainViewModel(INavigationService navigation, AuctionService auctionService, UserProfileCard userProfileCard)
        {
            _navigation = navigation;
            _auctionService = auctionService;
            UserProfileCard = userProfileCard;

            if (_navigation is INotifyPropertyChanged npc)
            {
                npc.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(_navigation.CurrentViewModel))
                        OnPropertyChanged(nameof(CurrentContent));
                };
            }

            LeftPanel = new LeftPanelViewModel(_navigation, _auctionService);

            // Start på Home
            _navigation.NavigateTo(new HomeScreenViewModel(_auctionService));
        }

        public ViewModelBase CurrentContent => _navigation.CurrentViewModel;
    }
}
