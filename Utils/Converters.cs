using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using ZoaInfoTool.Models;

namespace ZoaInfoTool.Utils;

public static class Converters
{
    public static GridLength GridLengthTrueEqualsStar(bool value)
    {
        return value ? new GridLength(1, GridUnitType.Star) : GridLength.Auto;
    }

    public static GridLength GridLengthTrueEqualsAuto(bool value)
    {
        return value ? GridLength.Auto : new GridLength(1, GridUnitType.Star);
    }
}

public class AliasRunwayToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        int? runway = (int?)value;
        return runway is not null ? runway.ToString() : "Any";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

public class ChartTypetoStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return (ChartType)value switch
        {
            ChartType.AirportDiagram => "Airport Diagram",
            ChartType.AirportMinimums => "Minimums",
            ChartType.HotSpots => "Hot Spots",
            ChartType.STAR => "STAR",
            ChartType.DP => "SID",
            ChartType.IAP => "Approaches",
            ChartType.Unknown => "Unknown",
            _ => "Unknown"
        };
    }

    // ConvertBack is not implemented for a OneWay binding.
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
