using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using System;

namespace ZoaInfoTool.ViewModels;

public partial class ClockViewModel : ObservableObject
{
    [ObservableProperty]
    private string clockString;

    private readonly DispatcherTimer dispatcherTimer;
    private static readonly string _format = "HH:mm:ss";

    public ClockViewModel()
    {
        ClockString = DateTime.UtcNow.ToString(_format);
        dispatcherTimer = new DispatcherTimer();
        dispatcherTimer.Tick += DispatcherTimer_Tick;
        dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
        dispatcherTimer.Start();
    }

    private void DispatcherTimer_Tick(object sender, object e)
    {
        ClockString = DateTime.UtcNow.ToString(_format);
    }
}
