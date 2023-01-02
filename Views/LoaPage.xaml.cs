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
public sealed partial class LoaPage : Page
{
    public LoaViewModel ViewModel => (LoaViewModel)DataContext;
    public LoaPage()
    {
        this.InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<LoaViewModel>();

        // Add the Enter button handler to both text boxes
        DepAirportTb.KeyDown += KeyHandlers.NewOnEnterCommandHandler(ViewModel.MatchLoaRulesCommand);
        ArrAirportTb.KeyDown += KeyHandlers.NewOnEnterCommandHandler(ViewModel.MatchLoaRulesCommand);

        // Set focus on the Departure textbox once the page has fully loaded
        Loaded += (_, _) => DepAirportTb.Focus(FocusState.Programmatic);

        // Command viewmodel to fetch data on page load
        Loaded += (_, _) => ViewModel.InitializeAsyncCommand.Execute(null);
    }
}
