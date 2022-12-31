using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ZoaInfoTool.Models;
using ZoaInfoTool.Services.Interfaces;

namespace ZoaInfoTool.ViewModels;

public partial class LoaViewModel : ObservableObject
{
    private ILoaRulesService LoaFetcher;
    private List<LoaRule> LoaRules;
    private bool _isInitialized = false;

    [ObservableProperty]
    private string departureAirport;

    [ObservableProperty]
    private string arrivalAirport;

    public ObservableCollection<LoaRule> MatchedLoaRules;

    public LoaViewModel(ILoaRulesService loaFetcher)
    {
        LoaFetcher = loaFetcher;
        MatchedLoaRules = new ObservableCollection<LoaRule>();
    }

    [RelayCommand]
    private async void InitializeAsync()
    {
        if (!_isInitialized)
        {
            LoaRules = await LoaFetcher.FetchLoaRulesAsync();
            _isInitialized = true;
        }
    }

    [RelayCommand]
    private void MatchLoaRules()
    {
        MatchedLoaRules.Clear();

        if (String.IsNullOrEmpty(departureAirport) || String.IsNullOrEmpty(arrivalAirport)) return;

        // Add K to start of airport ID if needed
        string sanitizedDeparture = (DepartureAirport.Length == 3 ? "K" : "") + DepartureAirport.ToUpper();
        string sanitizedArrival = (ArrivalAirport.Length == 3 ? "K" : "") + ArrivalAirport.ToUpper();

        // TODO: check also that departure airport is in ZOA
        foreach (var rule in LoaRules)
        {
            if (rule.DepartureAirportRegex.IsMatch(sanitizedDeparture) && rule.ArrivalAirportRegex.IsMatch(sanitizedArrival))
            {
                MatchedLoaRules.Add(rule);
            }
        }
    }
}
