using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace AutoAuction_H2.Views
{
    public partial class WindowButtonPanelView : UserControl
    {
        public WindowButtonPanelView()
        {
            InitializeComponent();
        }

        private void CloseImage_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (this.VisualRoot is Window window)
                window.Close();
        }

        private void MinimizeImage_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (this.VisualRoot is Window window)
                window.WindowState = WindowState.Minimized;
        }
    }
}
