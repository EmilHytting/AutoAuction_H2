using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AutoAuction_H2.ViewModels;

namespace AutoAuction_H2.Views;

public partial class HomeScreenView : UserControl
{
    public HomeScreenView()
    {
        InitializeComponent();
        // DataContext is provided via DataTemplates; remove hardcoded assignment
        // DataContext = new HomeScreenViewModel();
    }
}