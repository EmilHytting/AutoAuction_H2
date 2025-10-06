using AutoAuction_H2.Models.Entities;
using AutoAuction_H2.Services;
using AutoAuction_H2.ViewModels;
using AutoAuction_H2.Views;
using AutoAuction_H2.Views.ContentPanels;
using AutoAuction_H2.Views.UIElements;
using AutoAuction_H2.Views.Windows;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace AutoAuction_H2
{
    public partial class App : Application
    {
        private static ServiceProvider? _serviceProvider; // <-- felt

        // ✅ Gør Services tilgængelig globalt
        public static ServiceProvider Services =>
            _serviceProvider ?? throw new InvalidOperationException("Services not initialized");

        public override void OnFrameworkInitializationCompleted()
        {
            var services = new ServiceCollection();

            // Hent base-URL fra AppState
            var baseUri = new Uri(AppState.Instance.ApiBaseUrl);

            // Registrer HttpClient med base address
            services.AddSingleton(new HttpClient { BaseAddress = baseUri });

            // Services
            services.AddSingleton<AuthService>();
            services.AddSingleton<AuctionService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<HomeScreenViewModel>();
            services.AddTransient<AuctionOverviewViewModel>();
            services.AddTransient<CreateAuctionViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<UserProfileViewModel>();
            // Content Panels
            services.AddTransient<PrivateCarsViewModel>();
            services.AddTransient<ProfessionalCarsViewModel>();
            services.AddTransient<TrucksViewModel>();
            services.AddTransient<BusesViewModel>();
            services.AddTransient<MyBidsViewModel>();
            services.AddTransient<MySalesViewModel>();


            // Views
            services.AddTransient<MainWindow>();
            services.AddTransient<LoginView>();
            services.AddTransient<MainView>();
            services.AddTransient<UserProfileCard>();
            services.AddTransient<UserProfileWindow>();
            services.AddTransient<ChangePasswordWindow>();

            // Build provider og gem i statisk felt
            _serviceProvider = services.BuildServiceProvider();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = Services.GetRequiredService<MainWindow>();
            }

            base.OnFrameworkInitializationCompleted();
        }

    }
}
