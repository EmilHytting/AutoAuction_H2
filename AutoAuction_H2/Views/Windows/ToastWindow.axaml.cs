using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;

namespace AutoAuction_H2.Views.Windows;

public partial class ToastWindow : Window
{
    public ToastWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public async Task ShowAsync(string message, Window? owner = null, int milliseconds = 2000)
    {
        if (this.FindControl<TextBlock>("MessageText") is { } tb)
            tb.Text = message;

        if (owner != null)
        {
            Position = new PixelPoint(owner.Position.X + 20, owner.Position.Y + 20);
        }

        Show();
        await Task.Delay(milliseconds);
        Close();
    }
}
