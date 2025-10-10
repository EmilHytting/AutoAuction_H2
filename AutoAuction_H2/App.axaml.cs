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
        private static ServiceProvider? _serviceProvider;

        public static ServiceProvider Services =>
            _serviceProvider ?? throw new InvalidOperationException("Services not initialized");

        public override void OnFrameworkInitializationCompleted()
        {
            var services = new ServiceCollection();

            // ---------- Services ----------
            var baseUri = new Uri(AppState.Instance.ApiBaseUrl);
            services.AddSingleton(new HttpClient { BaseAddress = baseUri });
            services.AddScoped<UserService>();
            services.AddScoped<AuthService>();
            services.AddScoped<AuctionService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<VehicleFactory>();

            // ---------- ViewModels ----------
            services.AddTransient<LoginViewModel>();
            services.AddTransient<HomeScreenViewModel>();
            services.AddTransient<AuctionOverviewViewModel>();
            services.AddTransient<CreateAuctionViewModel>();
            services.AddSingleton<UserProfileViewModel>();        // Small profile card
            services.AddTransient<UserProfileWindowViewModel>();  // Full profile window
            services.AddTransient<PrivateCarsViewModel>();
            services.AddTransient<ProfessionalCarsViewModel>();
            services.AddTransient<TrucksViewModel>();
            services.AddTransient<BusesViewModel>();
            services.AddTransient<MyBidsViewModel>();
            services.AddTransient<MySalesViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<AuctionDetailViewModel>();
            services.AddTransient<LeftPanelViewModel>();
            services.AddTransient<ChangePasswordViewModel>();

            // ---------- Views ----------
            services.AddSingleton<MainWindow>(); // Root window should be singleton
            services.AddTransient<LoginView>();
            services.AddTransient<MainView>();
            services.AddTransient<UserProfileView>();
            services.AddTransient<UserProfileWindow>();  // ✅ Add this back, managed via DI
            services.AddTransient<ChangePasswordWindow>();
            services.AddTransient<CreateAuctionWindow>();

            services.AddSingleton(AppState.Instance);  // register the existing singleton

            // ---------- Build DI ----------
            _serviceProvider = services.BuildServiceProvider();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var nav = Services.GetRequiredService<INavigationService>();

                // Start with HomeScreen
                nav.NavigateTo<HomeScreenViewModel>();

                // MainWindow with DI-injected MainViewModel
                desktop.MainWindow = Services.GetRequiredService<MainWindow>();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
