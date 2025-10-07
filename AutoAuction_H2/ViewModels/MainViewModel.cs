using AutoAuction_H2.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoAuction_H2.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigation;

        public UserProfileViewModel UserProfileViewModel { get; }
        public LeftPanelViewModel LeftPanelViewModel { get; }

        public MainViewModel(INavigationService navigation,
                             UserProfileViewModel userProfileVm,
                             LeftPanelViewModel leftPanelVm)
        {
            _navigation = navigation;
            UserProfileViewModel = userProfileVm;
            LeftPanelViewModel = leftPanelVm;

            _navigation.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(INavigationService.CurrentViewModel))
                    OnPropertyChanged(nameof(CurrentContent));
            };

            _navigation.NavigateTo<HomeScreenViewModel>();
        }

        public ViewModelBase? CurrentContent => _navigation.CurrentViewModel;
        public string? SearchText { get; set; }
    }

}
