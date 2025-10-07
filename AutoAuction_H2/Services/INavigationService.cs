using System.ComponentModel;

namespace AutoAuction_H2.ViewModels
{
    public interface INavigationService : INotifyPropertyChanged
    {
        ViewModelBase? CurrentViewModel { get; }

        void NavigateTo(ViewModelBase viewModel);
        void NavigateTo<TViewModel>() where TViewModel : ViewModelBase;
    }
}
