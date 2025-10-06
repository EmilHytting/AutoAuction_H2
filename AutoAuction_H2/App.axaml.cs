using AutoAuction_H2.Services;
using AutoAuction_H2.ViewModels;
using AutoAuction_H2.Views;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Linq;
using Avalonia;

namespace AutoAuction_H2
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;
        public static IServiceProvider Services { get; private set; } = default!;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var services = new ServiceCollection();

            var httpClient = new HttpClient();

            // Services
            services.AddSingleton<AuthService>();
            services.AddSingleton<AuctionService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // ViewModels
            services.AddTransient<HomeScreenViewModel>();
            services.AddTransient<AuctionOverviewViewModel>();
            services.AddTransient<CreateAuctionViewModel>();
            services.AddTransient<UserProfileViewModel>();
            services.AddTransient<ChangePasswordViewModel>();

            // Shell
            services.AddSingleton<MainViewModel>();

            _serviceProvider = services.BuildServiceProvider();
            Services = _serviceProvider;

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                DisableAvaloniaDataAnnotationValidation();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = Services.GetRequiredService<MainViewModel>()
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new MainView
                {
                    DataContext = Services.GetRequiredService<MainViewModel>()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}
