using AutoAuction_H2.ViewModels;

public class NavigationService : INavigationService
{
    private ViewModelBase _currentViewModel;

    public ViewModelBase CurrentViewModel => _currentViewModel;

    public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase, new()
    {
        _currentViewModel = new TViewModel();
    }

    public void NavigateTo(ViewModelBase viewModel)
    {
        _currentViewModel = viewModel;
    }
}
