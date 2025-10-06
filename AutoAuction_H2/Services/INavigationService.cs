using AutoAuction_H2.ViewModels;

public interface INavigationService
{
    ViewModelBase CurrentViewModel { get; }
    void NavigateTo<TViewModel>() where TViewModel : ViewModelBase, new();
    void NavigateTo(ViewModelBase viewModel);
}
