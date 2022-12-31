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
public sealed partial class CodesPage : Page
{
    public IcaoCodesViewModel ViewModel => (IcaoCodesViewModel)DataContext;
    public CodesPage()
    {
        this.InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<IcaoCodesViewModel>();

        AircraftTb.KeyDown += KeyHandlers.NewOnEnterCommandHandler(ViewModel.LookupAircraftCommand);
        AirlineTb.KeyDown += KeyHandlers.NewOnEnterCommandHandler(ViewModel.LookupAirlineCommand);
        AirportTb.KeyDown += KeyHandlers.NewOnEnterCommandHandler(ViewModel.LookupAirportCommand);

        // Set focus on the Airline textbox once the page has fully loaded
        AirlineTb.IsEnabledChanged += (sender, e) => { AirlineTb.Focus(FocusState.Programmatic); };
        Loaded += (sender, e) => { AirlineTb.Focus(FocusState.Programmatic); };
    }
}
