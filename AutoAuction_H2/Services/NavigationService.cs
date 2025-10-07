using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace AutoAuction_H2.ViewModels
{
    public class NavigationService : INavigationService, INotifyPropertyChanged
    {
        private ViewModelBase? _currentViewModel;
        public ViewModelBase? CurrentViewModel
        {
            get => _currentViewModel;
            private set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentViewModel)));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void NavigateTo(ViewModelBase viewModel)
        {
            CurrentViewModel = viewModel;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            var vm = App.Services.GetRequiredService<TViewModel>();
            NavigateTo(vm);
        }
    }
}
