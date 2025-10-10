using AutoAuction_H2.ViewModels;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace AutoAuction_H2.Views.UIElements
{
    public partial class UserProfileView : UserControl
    {
        public UserProfileView()
        {
            InitializeComponent();
            DataContext = App.Services.GetRequiredService<UserProfileViewModel>();
        }
    }
}
