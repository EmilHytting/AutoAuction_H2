using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;
using AutoAuction_H2.Models.Entities;

namespace AutoAuction_H2.Converters
{
    public class BidStatusConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int highestBidderId)
            {
                return highestBidderId == AppState.Instance.UserId
                    ? Brushes.Green
                    : Brushes.Red;
            }
            return Brushes.Gray;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    public class BidStatusTextConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is int highestBidderId)
            {
                return highestBidderId == AppState.Instance.UserId
                    ? "Førende"
                    : "Overbudt";
            }
            return "Ukendt";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
