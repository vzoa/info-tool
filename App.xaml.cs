// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
//using Windows.Storage;
using ZoaInfoTool.Services;
using ZoaInfoTool.Services.Interfaces;
using ZoaInfoTool.ViewModels;
using ZoaInfoTool.Utils;
using Windows.Graphics;

namespace ZoaInfoTool;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    public IServiceProvider Services { get; }
    public new static App Current => (App)Application.Current;
    public static ISettingsService Settings { get; private set; }
    private AppWindow MainAppWindowInterop { get; set; }

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        Services = ConfigureServices();
        this.InitializeComponent();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Services & Repositories
        services.AddHttpClient();
        services.AddSingleton<IAtisService, ClowdDatisFetchService>();
        services.AddSingleton<IRouteSummaryService, FlightAwareRouteService>();
        services.AddSingleton<IChartService, FaaChartService>();
        services.AddSingleton<IAirlineIcaoService, GithubAirlineIcaoService>();
        services.AddSingleton<IAircraftIcaoService, GithubAircraftIcaoService>();
        services.AddSingleton<ILoaRulesService, GithubLoaService>();
        services.AddSingleton<IAliasRouteService, GithubAliasRouteService>();
        services.AddSingleton<IAirportIcaoService, VatSpyDataService>();
        services.AddSingleton<ISettingsService, SettingsService>();

        // Viewmodels
        services.AddTransient<DatisViewModel>();
        services.AddTransient<RwRouteViewModel>();
        services.AddTransient<SkyVectorViewModel>();
        services.AddTransient<ChartViewModel>();
        services.AddTransient<IcaoCodesViewModel>();
        services.AddTransient<LoaViewModel>();
        services.AddTransient<AliasRouteViewModel>();
        services.AddTransient<ClockViewModel>();

        return services.BuildServiceProvider();
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        m_window = new MainWindow();

        // Get interop window and save
        IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(m_window);
        WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
        MainAppWindowInterop = AppWindow.GetFromWindowId(windowId);

        //Set icon
        MainAppWindowInterop.SetIcon(@"Assets\Ico-256x256.ico");

        // Check if saved settings exist for window size and resize if found. If not, use 800x600 default
        Settings = App.Current.Services.GetRequiredService<ISettingsService>();
        await Settings.LoadFromFileAsync();
        SizeInt32 size = new(800, 600);
        if (Settings.Values.TryGetValue("WindowWidth", out string width) && Settings.Values.TryGetValue("WindowHeight", out string height))
        {
            size = new SizeInt32(int.Parse(width), int.Parse(height));
        }
        MainAppWindowInterop.Resize(size);

        // Check if saved settings exist for window position. If so, move window to saved position
        if (Settings.Values.TryGetValue("WindowX", out string x) && Settings.Values.TryGetValue("WindowY", out string y))
        {
            MainAppWindowInterop.Move(new PointInt32(int.Parse(x), int.Parse(y)));
        }

        // Register handlers to save the window size and position for future startup whenever user changes
        m_window.SizeChanged += SaveSize;
        MainAppWindowInterop.Changed += SavePosition;

        // Set title and start
        m_window.Title = "ZOA Info";
        m_window.Activate();
    }

    private void SaveSize(object _, WindowSizeChangedEventArgs args)
    {
        Settings.Values["WindowWidth"] = args.Size.Width.ToString();
        Settings.Values["WindowHeight"] = args.Size.Height.ToString();
    }

    private void SavePosition(object sender, AppWindowChangedEventArgs args)
    {
        if (args.DidPositionChange)
        {
            Settings.Values["WindowX"] = MainAppWindowInterop.Position.X.ToString();
            Settings.Values["WindowY"] = MainAppWindowInterop.Position.Y.ToString();
        }
    }

    private Window m_window;
}
