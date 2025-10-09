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
using Microsoft.EntityFrameworkCore;
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

            // Hent base-URL fra AppState
            var baseUri = new Uri(AppState.Instance.ApiBaseUrl);

            // Registrer HttpClient med base address
            services.AddSingleton(new HttpClient { BaseAddress = baseUri });
            services.AddScoped<UserService>();
            // ---------- Services ----------
            services.AddScoped<AuthService>();
            services.AddScoped<AuctionService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<VehicleFactory>();


            // ---------- ViewModels ----------
            services.AddTransient<LoginViewModel>();
            services.AddTransient<HomeScreenViewModel>();
            services.AddTransient<AuctionOverviewViewModel>();
            services.AddTransient<CreateAuctionViewModel>();
            services.AddSingleton<UserProfileViewModel>();
            services.AddTransient<PrivateCarsViewModel>();
            services.AddTransient<ProfessionalCarsViewModel>();
            services.AddTransient<TrucksViewModel>();
            services.AddTransient<BusesViewModel>();
            services.AddTransient<MyBidsViewModel>();
            services.AddTransient<MySalesViewModel>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<AuctionDetailViewModel>();
            services.AddTransient<LeftPanelViewModel>();

            // ---------- Views ----------
            // MainWindow bør være Singleton, da den er root
            services.AddSingleton<MainWindow>();
            services.AddTransient<LoginView>();
            services.AddTransient<MainView>();
            services.AddTransient<UserProfileView>();

            // ❌ UserProfileWindow skal ikke være i DI (oprettes manuelt når den bruges)
            // services.AddTransient<UserProfileWindow>();

            services.AddTransient<ChangePasswordWindow>();
            services.AddTransient<CreateAuctionWindow>();

            // ---------- Build DI ----------
            _serviceProvider = services.BuildServiceProvider();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Hent navigation service
                var nav = Services.GetRequiredService<INavigationService>();

                // Start med HomeScreenViewModel
                nav.NavigateTo<HomeScreenViewModel>();

                // Sæt MainWindow med MainViewModel, som holder navigation
                desktop.MainWindow = Services.GetRequiredService<MainWindow>();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
