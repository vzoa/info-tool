using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ZoaInfoTool.Models;
using ZoaInfoTool.Services.Interfaces;

namespace ZoaInfoTool.ViewModels
{
    public partial class RwRouteViewModel : ObservableObject
    {
        public ObservableCollection<RouteSummary> Routes { get; set; }

        [ObservableProperty]
        private string departureAirport;

        [ObservableProperty]
        private string arrivalAirport;

        private IRouteSummaryService RouteFetcher { get; set; }

        public RwRouteViewModel(IRouteSummaryService routeFetcher)
        {
            RouteFetcher = routeFetcher;
            Routes = new ObservableCollection<RouteSummary>();
        }

        [RelayCommand]
        private async void FetchRoutes()
        {
            List<RouteSummary> fetchedRoutes = await RouteFetcher.FetchRouteSummariesAsync(departureAirport, arrivalAirport);
            Routes.Clear();
            foreach (var route in fetchedRoutes)
            {
                Routes.Add(route);
            }
        }
    }
}
