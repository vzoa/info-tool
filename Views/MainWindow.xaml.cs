// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WinRT.Interop;
using ZoaInfoTool.ViewModels;
using ZoaInfoTool.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoaInfoTool;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    private Button? SelectedButton { get; set; }
    private SolidColorBrush UnselectedButtonBrush { get; set; }
    private SolidColorBrush SelectedButtonBrush { get; set; }
    private SolidColorBrush HoverButtonBrush { get; set; }
    private Dictionary<Button, Type> ButtonPageLookup { get; set; }
    private ClockViewModel ClockViewModel { get; set; }

    private AppWindow m_AppWindow;

    public MainWindow()
    {
        this.InitializeComponent();

        // Get the clock view model and pass to title bar
        ClockViewModel = App.Current.Services.GetRequiredService<ClockViewModel>();

        // Get base colors from Theme dictionary defined in App.xaml
        UnselectedButtonBrush = (SolidColorBrush)Application.Current.Resources["ButtonBackground"];
        SelectedButtonBrush = (SolidColorBrush)Application.Current.Resources["ButtonBackgroundPressed"];
        HoverButtonBrush = (SolidColorBrush)Application.Current.Resources["ButtonBackgroundPointerOver"];

        m_AppWindow = GetAppWindowForCurrentWindow();

        // Check to see if customization is supported.
        // Currently only supported on Windows 11.
        if (AppWindowTitleBar.IsCustomizationSupported())
        {
            var titleBar = m_AppWindow.TitleBar;
            titleBar.ExtendsContentIntoTitleBar = true;
            AppTitleBar.Loaded += AppTitleBar_Loaded;
            AppTitleBar.SizeChanged += AppTitleBar_SizeChanged;

            titleBar.ButtonBackgroundColor = titleBar.ButtonHoverBackgroundColor = titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = titleBar.ButtonInactiveForegroundColor = Colors.RoyalBlue;
            titleBar.ButtonHoverForegroundColor = Colors.White;
        }

        // Create lookup dictionary for buttons and associated main frame navigation pages
        ButtonPageLookup = new Dictionary<Button, Type>()
        {
            { datisBtn, typeof(DatisPage) },
            { rwRoutesBtn, typeof(RwRoutesPage) },
            { skyVectorBtn, typeof(SkyVectorPage) },
            { loaBtn, typeof(LoaPage) },
            { tecBtn, typeof(TecPage) },
            { chartsBtn, typeof(ChartsPage) },
            { codesBtn, typeof(CodesPage) }
        };

        // Set to default startup page
        mainFrame.Navigate(typeof(StartupPage));
    }

    private void NavBtn_Click(object sender, RoutedEventArgs _)
    {
        // Change the current selected button's color back to unselected and reset hover color
        if (SelectedButton is not null)
        {
            SelectedButton.Background = UnselectedButtonBrush;
            SelectedButton.Resources["ButtonBackgroundPointerOver"] = HoverButtonBrush;
        }

        // Set the new button's background and hover color and set to selected
        var btn = (Button)sender;
        btn.Background = SelectedButtonBrush;
        btn.Resources["ButtonBackgroundPointerOver"] = SelectedButtonBrush;
        SelectedButton = btn;

        // Navigate based on selected button. Contains check not necessary since buttons and lookup
        // dictionary are hardcoded, but it's another layer of exception protection
        if (ButtonPageLookup.TryGetValue(SelectedButton, out Type pageType))
        {
            mainFrame.Navigate(pageType, null, new SuppressNavigationTransitionInfo());
            //mainFrame.Navigate(ButtonPageLookup[SelectedButton]);
        }
    }

    private void AppTitleBar_Loaded(object sender, RoutedEventArgs e)
    {
        // Check to see if customization is supported.
        // Currently only supported on Windows 11.
        if (AppWindowTitleBar.IsCustomizationSupported())
        {
            SetDragRegionForCustomTitleBar(m_AppWindow);
        }
    }

    private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        // Check to see if customization is supported.
        // Currently only supported on Windows 11.
        if (AppWindowTitleBar.IsCustomizationSupported()
            && m_AppWindow.TitleBar.ExtendsContentIntoTitleBar)
        {
            // Update drag region if the size of the title bar changes.
            SetDragRegionForCustomTitleBar(m_AppWindow);
        }
    }

    private AppWindow GetAppWindowForCurrentWindow()
    {
        IntPtr hWnd = WindowNative.GetWindowHandle(this);
        WindowId wndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
        return AppWindow.GetFromWindowId(wndId);
    }

    [DllImport("Shcore.dll", SetLastError = true)]
    internal static extern int GetDpiForMonitor(IntPtr hmonitor, Monitor_DPI_Type dpiType, out uint dpiX, out uint dpiY);

    internal enum Monitor_DPI_Type : int
    {
        MDT_Effective_DPI = 0,
        MDT_Angular_DPI = 1,
        MDT_Raw_DPI = 2,
        MDT_Default = MDT_Effective_DPI
    }

    private double GetScaleAdjustment()
    {
        IntPtr hWnd = WindowNative.GetWindowHandle(this);
        WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        DisplayArea displayArea = DisplayArea.GetFromWindowId(wndId, DisplayAreaFallback.Primary);
        IntPtr hMonitor = Win32Interop.GetMonitorFromDisplayId(displayArea.DisplayId);

        // Get DPI.
        int result = GetDpiForMonitor(hMonitor, Monitor_DPI_Type.MDT_Default, out uint dpiX, out uint _);
        if (result != 0)
        {
            throw new Exception("Could not get DPI for monitor.");
        }

        uint scaleFactorPercent = (uint)(((long)dpiX * 100 + (96 >> 1)) / 96);
        return scaleFactorPercent / 100.0;
    }

    private void SetDragRegionForCustomTitleBar(AppWindow appWindow)
    {
        // Check to see if customization is supported.
        // Currently only supported on Windows 11.
        if (AppWindowTitleBar.IsCustomizationSupported()
            && appWindow.TitleBar.ExtendsContentIntoTitleBar)
        {
            double scaleAdjustment = GetScaleAdjustment();

            RightPaddingColumn.Width = new GridLength(appWindow.TitleBar.RightInset / scaleAdjustment);
            LeftPaddingColumn.Width = new GridLength(appWindow.TitleBar.LeftInset / scaleAdjustment);

            List<Windows.Graphics.RectInt32> dragRectsList = new();

            Windows.Graphics.RectInt32 dragRectL;
            dragRectL.X = (int)((LeftPaddingColumn.ActualWidth) * scaleAdjustment);
            dragRectL.Y = 0;
            dragRectL.Height = (int)(AppTitleBar.ActualHeight * scaleAdjustment);
            dragRectL.Width = (int)((IconColumn.ActualWidth
                                    + TitleColumn.ActualWidth
                                    + LeftDragColumn.ActualWidth) * scaleAdjustment);
            dragRectsList.Add(dragRectL);

            Windows.Graphics.RectInt32 dragRectR;
            dragRectR.X = (int)((LeftPaddingColumn.ActualWidth
                                + IconColumn.ActualWidth
                                + TitleTextBlock.ActualWidth
                                + LeftDragColumn.ActualWidth
                                + SearchColumn.ActualWidth) * scaleAdjustment);
            dragRectR.Y = 0;
            dragRectR.Height = (int)(AppTitleBar.ActualHeight * scaleAdjustment);
            dragRectR.Width = (int)(RightDragColumn.ActualWidth * scaleAdjustment);
            dragRectsList.Add(dragRectR);

            Windows.Graphics.RectInt32[] dragRects = dragRectsList.ToArray();

            appWindow.TitleBar.SetDragRectangles(dragRects);
        }
    }
}
