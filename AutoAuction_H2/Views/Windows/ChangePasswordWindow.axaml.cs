using AutoAuction_H2.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;

namespace AutoAuction_H2.Views.Windows
{
    public partial class ChangePasswordWindow : Window
    {
        private readonly ChangePasswordViewModel _vm;

        public ChangePasswordWindow()
        {
            InitializeComponent();

            // ✅ Hent ViewModel fra DI
            _vm = App.Services.GetRequiredService<ChangePasswordViewModel>();
            DataContext = _vm;
        }

        private void Cancel_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
