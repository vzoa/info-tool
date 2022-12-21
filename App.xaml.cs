// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using Windows.Graphics;
using Windows.Storage;
using ZoaInfoTool.Services;
using ZoaInfoTool.Services.Interfaces;
using ZoaInfoTool.ViewModels;

namespace ZoaInfoTool
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider Services { get; }
        public new static App Current => (App)Application.Current;
        private AppWindow MainAppWindowInterop { get; set; }
        private ApplicationDataContainer LocalSettings { get; set; } = ApplicationData.Current.LocalSettings;
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

            // Viewmodels
            services.AddTransient<DatisViewModel>();
            services.AddTransient<RwRouteViewModel>();
            services.AddTransient<SkyVectorViewModel>();
            services.AddTransient<ChartViewModel>();
            services.AddTransient<IcaoCodesViewModel>();
            services.AddTransient<LoaViewModel>();
            services.AddTransient<AliasRouteViewModel>();

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();

            // Get interop window and save
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(m_window);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            MainAppWindowInterop = AppWindow.GetFromWindowId(windowId);

            // Check if saved settings exist for window size and resize if found. If not, use 800x600 default
            SizeInt32 size = new(800, 600);
            if (LocalSettings.Values.TryGetValue("WindowWidth", out var width) && LocalSettings.Values.TryGetValue("WindowHeight", out var height))
            {
                size = new SizeInt32((int)width, (int)height);
            }
            MainAppWindowInterop.Resize(size);

            // Check if saved settings exist for window position. If so, move window to saved position
            if (LocalSettings.Values.TryGetValue("WindowX", out var x) && LocalSettings.Values.TryGetValue("WindowY", out var y))
            {
                MainAppWindowInterop.Move(new PointInt32((int)x, (int)y));
            }

            // Register handler to save the window size for future startup whenever user changes
            m_window.SizeChanged += SaveSize;

            // Register handler on interop window to save window position for future startup
            MainAppWindowInterop.Changed += SavePosition;

            // Set title and start
            m_window.Title = "ZOA Info";
            m_window.Activate();
        }

        private void SaveSize(object sender, WindowSizeChangedEventArgs args)
        {
            LocalSettings.Values["WindowWidth"] = (int)args.Size.Width;
            LocalSettings.Values["WindowHeight"] = (int)args.Size.Height;
        }

        private void SavePosition(object sender, AppWindowChangedEventArgs args)
        {
            if (args.DidPositionChange)
            {
                LocalSettings.Values["WindowX"] = MainAppWindowInterop.Position.X;
                LocalSettings.Values["WindowY"] = MainAppWindowInterop.Position.Y;
            }
        }

        private Window m_window;
    }
}
