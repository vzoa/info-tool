using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ZoaInfoTool.Models;
using ZoaInfoTool.Services.Interfaces;

namespace ZoaInfoTool.ViewModels
{
    public partial class LoaViewModel : ObservableObject
    {
        private ILoaRulesService LoaFetcher;
        private List<LoaRule> LoaRules;

        [ObservableProperty]
        private string departureAirport;

        [ObservableProperty]
        private string arrivalAirport;

        public ObservableCollection<LoaRule> MatchedLoaRules;

        public LoaViewModel(ILoaRulesService loaFetcher)
        {
            LoaFetcher = loaFetcher;
            MatchedLoaRules = new ObservableCollection<LoaRule>();
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            LoaRules = await LoaFetcher.FetchLoaRulesAsync();
        }

        [RelayCommand]
        private void MatchLoaRules()
        {
            MatchedLoaRules.Clear();

            if (!String.IsNullOrEmpty(DepartureAirport) && !String.IsNullOrEmpty(ArrivalAirport))
            {
                // Add K to start of airport ID if needed
                string checkedDeparture = (DepartureAirport.Length == 3 ? "K" : "") + DepartureAirport.ToUpper();
                string checkedArrival = (ArrivalAirport.Length == 3 ? "K" : "") + ArrivalAirport.ToUpper();

                // TODO: check also that departure airport is in ZOA

                foreach (var rule in LoaRules)
                {
                    if (rule.DepartureAirportRegex.IsMatch(checkedDeparture) && rule.ArrivalAirportRegex.IsMatch(checkedArrival))
                    {
                        MatchedLoaRules.Add(rule);
                    }
                }
            }
        }
    }
}
