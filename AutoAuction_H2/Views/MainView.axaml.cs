using AutoAuction_H2.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace AutoAuction_H2.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }


    private void UserControl_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            // Get the top-level window
            if (this.VisualRoot is Window window)
            {
                window.BeginMoveDrag(e);
            }
        }
    }
}
