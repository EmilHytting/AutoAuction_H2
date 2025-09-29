using AutoAuction_H2.Converters;
using AutoAuction_H2.Models;
using AutoAuction_H2.ViewModels;
using AutoAuction_H2.Views.ContentPanels;
using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace AutoAuction_H2.Views
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            // Create LoginViewModel and LoginView
            var loginVm = new LoginViewModel();
            loginVm.LoggedIn += ShowMainView;

            var loginView = new LoginView();
            loginView.DataContext = loginVm;

            MainContent.Content = loginView; // Show login first
        }

        private void Border_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                this.BeginMoveDrag(e);
        }

        public async void ShowMainView()
        {
            var mainView = new MainView();

            // 1️⃣ Create your ViewModel
            var vm = new MainViewModel();

            // 2️⃣ Assign it to the view
            mainView.DataContext = vm;

            // 3️⃣ Load vehicles
            var vehicles = await GetVehiclesAsync();
            foreach (var v in vehicles)
                vm.Vehicles.Add(v);

            // 4️⃣ Show the view
            MainContent.Content = mainView;
        }


        public async Task<List<Vehicle>> GetVehiclesAsync()
        {
            using var httpClient = new HttpClient();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new VehicleConverter() } // <-- add the custom converter
            };

            var response = await httpClient.GetStringAsync("https://localhost:44372/api/Vehicles");
            var vehicles = JsonSerializer.Deserialize<List<Vehicle>>(response, options);

            return vehicles ?? new List<Vehicle>();
            Debug.WriteLine()
        }




    }

}

