// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ZoaInfoTool.Utils;
using ZoaInfoTool.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoaInfoTool.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SkyVectorPage : Page
{
    public SkyVectorViewModel ViewModel => (SkyVectorViewModel)DataContext;

    public SkyVectorPage()
    {
        this.InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<SkyVectorViewModel>();

        // Add enter key handler to the Route textbox only
        RouteTb.KeyDown += KeyHandlers.NewOnEnterCommandHandler(ViewModel.MakeUrlCommand);

        // Set focus on the Departure textbox once the page has fully loaded
        Loaded += (sender, e) => DepAirportTb.Focus(FocusState.Programmatic);

        // Delete cookies to start fresh, but have to wait until CoreWebView2 object has started
        WebView.CoreWebView2Initialized += (o, e) => WebView.CoreWebView2.CookieManager.DeleteAllCookies();
    }
}
