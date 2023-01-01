// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ZoaInfoTool.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoaInfoTool.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class DatisPage : Page
{
    public DatisViewModel ViewModel => (DatisViewModel)DataContext;

    public DatisPage()
    {
        this.InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<DatisViewModel>();

        Loaded += (_, _) => AirportsComboBox.Focus(FocusState.Programmatic);

        // Command viewmodel to fetch data on page load
        Loaded += (_, _) => ViewModel.StartUpdateLoopCommand.Execute(null);
    }

    private void ToggleSwitch_Toggled(object s, RoutedEventArgs e) => ViewModel.AirportListToggleCommand.Execute(null);
}
