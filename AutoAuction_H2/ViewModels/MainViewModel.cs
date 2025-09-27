using AutoAuction_H2.Models;
using Avalonia.Controls.Primitives;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AutoAuction_H2.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public ObservableCollection<Vehicle> Vehicles { get; } = new ObservableCollection<Vehicle>();
}
