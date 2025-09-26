using Avalonia.Controls;
using Avalonia.Input;
using AutoAuction_H2.Views.ContentPanels;
using AutoAuction_H2.ViewModels;

namespace AutoAuction_H2.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
  
        // Create LoginViewModel and LoginView
        var loginVm = new LoginViewModel();
        loginVm.LoggedIn += ShowMainView; // subscribe to LoggedIn event

        var loginView = new LoginView();
        loginView.DataContext = loginVm; // bind ViewModel

        MainContent.Content = loginView; // show login first
    }

    private void Border_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            this.BeginMoveDrag(e);
    }

    public void ShowMainView()
    {
        MainContent.Content = new MainView(); // swap to MainView
    }
}
