using AutoAuction_H2.Models;
using AutoAuction_H2.Models.AutoAuction_H2.Models;
using Avalonia.Controls.Primitives;
using System;
using System.Diagnostics;

namespace AutoAuction_H2.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";
}
}
