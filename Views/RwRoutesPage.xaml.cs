// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using ZoaInfoTool.Models;
using ZoaInfoTool.Utils;
using ZoaInfoTool.ViewModels;


namespace ZoaInfoTool.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class RwRoutesPage : Page
{
    public RwRouteViewModel ViewModel => (RwRouteViewModel)DataContext;

    public RwRoutesPage()
    {
        this.InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<RwRouteViewModel>();

        // Add copy handler to the list view
        RoutesListView.KeyDown += RoutesListView_KeyDown;

        // Add the Enter button handler to both text boxes
        DepAirportTb.KeyDown += KeyHandlers.NewOnEnterCommandHandler(ViewModel.FetchRoutesCommand);
        ArrAirportTb.KeyDown += KeyHandlers.NewOnEnterCommandHandler(ViewModel.FetchRoutesCommand);

        // Set focus on the Departure textbox once the page has fully loaded
        Loaded += (sender, e) => DepAirportTb.Focus(FocusState.Programmatic);
    }

    // Hander to copy selected Route to clipboard when user hits C on a row
    private void RoutesListView_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.C)
        {
            var selected = (RouteSummary)RoutesListView.SelectedItem;
            if (selected is not null)
            {
                var dataPackage = new DataPackage();
                dataPackage.SetText(selected.Route);
                Clipboard.SetContent(dataPackage);
            }
        }
    }
}
