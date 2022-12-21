using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ZoaInfoTool.Models;
using ZoaInfoTool.Services.Interfaces;

namespace ZoaInfoTool.ViewModels
{
    public partial class AliasRouteViewModel : ObservableObject
    {
        private IAliasRouteService AliasRouteFetcher { get; set; }
        private Dictionary<string, List<AliasRoute>> AliasRoutesLookupDict { get; set; }

        [ObservableProperty]
        private string departureAirport;

        [ObservableProperty]
        private string arrivalAirport;

        public ObservableCollection<AliasRoute> MatchedAliasRoutes;

        public AliasRouteViewModel(IAliasRouteService aliasRouteFetcher)
        {
            AliasRouteFetcher = aliasRouteFetcher;
            MatchedAliasRoutes = new ObservableCollection<AliasRoute>();
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            AliasRoutesLookupDict = await AliasRouteFetcher.FetchAliasRoutesAsync();
        }

        [RelayCommand]
        private void MatchAliasRoutes()
        {
            if (DepartureAirport is null || ArrivalAirport is null)
            {
                return;
            }
            
            MatchedAliasRoutes.Clear();

            string departureLookup = departureAirport.Length == 4 ? departureAirport.Substring(1).ToUpper() : departureAirport.ToUpper();
            string arrivalLookup = arrivalAirport.Length == 4 ? arrivalAirport.Substring(1).ToUpper() : arrivalAirport.ToUpper();

            if (AliasRoutesLookupDict.TryGetValue(departureLookup, out List<AliasRoute> outList))
            {
                foreach (var aliasRoute in outList)
                {
                    if (aliasRoute.ArrivalAirport == arrivalLookup)
                    {
                        MatchedAliasRoutes.Add(aliasRoute);
                    }
                }
            }
        }
    }
}
