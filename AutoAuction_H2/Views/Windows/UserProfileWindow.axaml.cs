using AutoAuction_H2.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;

namespace AutoAuction_H2.Views.Windows
{
    public partial class UserProfileWindow : Window
    {
        private readonly UserProfileViewModel _vm;

        public UserProfileWindow(UserProfileViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
        }

        private void Close_Click(object? sender, RoutedEventArgs e) => Close();

        private async void ChangePassword_Click(object? sender, RoutedEventArgs e)
        {
            var dlg = App.Services.GetRequiredService<ChangePasswordWindow>();

            if (dlg.DataContext is ChangePasswordViewModel vm)
            {
                void Handler(object? s, System.EventArgs e2)
                {
                    vm.PasswordChanged -= Handler;
                    dlg.Close();
                }
                vm.PasswordChanged += Handler;
            }

            await dlg.ShowDialog(this);
        }
    }
}
