using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZoaInfoTool.Models;
using ZoaInfoTool.Services.Interfaces;

namespace ZoaInfoTool.ViewModels;

public partial class DatisViewModel : ObservableObject
{
    private DispatcherQueue Dispatcher { get; set; }
    private IAtisService AtisFetcher { get; set; }
    private Dictionary<string, Airport> AllAirportDictionary { get; set; }
    private bool _loopRunning = false;

    public ObservableCollection<string> DisplayedAirportNames { get; private set; }
    private bool _onlyZoaAirports = true;

    [ObservableProperty]
    private string dropdownPlaceholderText = "Loading airports...";

    [ObservableProperty]
    private bool dropdownEnabled = false;

    [ObservableProperty]
    private string atisText = "";

    [ObservableProperty]
    private string atisTitle = "D-ATIS";

    [ObservableProperty]
    private bool btnDepartureVisibility = false;

    [ObservableProperty]
    private bool btnArrivalVisibility = false;

    [ObservableProperty]
    private bool btnDepartureEnabled = false;

    [ObservableProperty]
    private bool btnArrivalEnabled = true;

    [ObservableProperty]
    private string selectedAirport;

    public DatisViewModel(IAtisService atisFetcher)
    {
        AtisFetcher = atisFetcher;
        Dispatcher = DispatcherQueue.GetForCurrentThread();
        DisplayedAirportNames = new ObservableCollection<string>();
    }

    [RelayCommand]
    public async void StartUpdateLoop()
    {
        if (_loopRunning) return;
        _loopRunning = true;

        await Task.Run(async () =>
        {
            while (true)
            {
                // Fetch all the ATIS and build new lookup dict
                var newAtis = await AtisFetcher.GetAllAsync();
                Dictionary<string, Airport> newAirportDict = new();
                foreach (var atis in newAtis)
                {
                    if (newAirportDict.ContainsKey(atis.Airport))
                    {
                        newAirportDict[atis.Airport].AtisList.Add(atis);
                    }
                    else
                    {
                        var newAirport = new Airport(atis.Airport, new List<Atis> { atis });
                        newAirportDict.Add(newAirport.Name, newAirport);
                    }
                }

                // Store new fetched dictionary as a class member, along with filtered list
                AllAirportDictionary = newAirportDict;
                var visibleAirportNames = FilterVisibleAirportNames(AllAirportDictionary);

                // If the list of airports has changed, update the dropdown on the UI Thread
                // so that notifications are captured by UI. Using hash sets to be order invariant.
                // Also update visible ATIS every time we get new data
                var existingAirportsHash = new HashSet<string>(DisplayedAirportNames);
                var newAirportsHash = new HashSet<string>(visibleAirportNames);
                if (!existingAirportsHash.SetEquals(newAirportsHash))
                {
                    Dispatcher.TryEnqueue(() =>
                    {
                        UpdateDropdownList(visibleAirportNames);
                        UpdateVisibleAtis();
                    });
                }

                // Wait to loop with some delay (default 60 seconds)
                await Task.Delay(Constants.AtisUpdateDelayMilliseconds);
            }
        });
    }

    partial void OnSelectedAirportChanged(string value)
    {
        UpdateVisibleAtis(value);
    }

    private void UpdateVisibleAtis(string? selectedAirport = null)
    {
        // Do nothing if we haven't selected any airport now or in the past
        if (SelectedAirport is null && selectedAirport is null) return;

        // Find the ATISes for the selected airport
        List<Atis> atisList = AllAirportDictionary[SelectedAirport].AtisList;

        if (atisList.Count == 1)
        {
            AtisText = AtisTextBodyFromFullAtis(atisList[0].Text);
            AtisTitle = AtisTitleFromFullAtis(atisList[0].Text);

            // Hide Dep/Arr buttons
            BtnDepartureVisibility = false;
            BtnArrivalVisibility = false;
        }
        else if (atisList.Count == 2)
        {
            // If we just changed to this airport, default to showing the Departure
            string fullText = AllAirportDictionary[SelectedAirport].AtisList
                .Where(a => a.Type == AtisType.Departure)
                .First()
                .Text;

            AtisText = AtisTextBodyFromFullAtis(fullText);
            AtisTitle = AtisTitleFromFullAtis(fullText);

            // Show Dep/Arr buttons
            BtnDepartureVisibility = BtnArrivalVisibility = true;

            // Make Dep button disabled (selected) and Arr button enabled
            BtnDepartureEnabled = false;
            BtnArrivalEnabled = true;
        }
    }

    [RelayCommand]
    private void DepArrToggle()
    {
        var depFullText = AllAirportDictionary[SelectedAirport].AtisList
            .Where(a => a.Type == AtisType.Departure)
            .First()
            .Text;

        var arrFullText = AllAirportDictionary[SelectedAirport].AtisList
            .Where(a => a.Type == AtisType.Arrival)
            .First()
            .Text;

        // If Departure button is disabled, that means we're currently showing the departure ATIS, so change to arrival
        AtisText = !BtnDepartureEnabled ? AtisTextBodyFromFullAtis(arrFullText) : AtisTextBodyFromFullAtis(depFullText);

        // Update ATIS title
        AtisTitle = !BtnDepartureEnabled ? AtisTitleFromFullAtis(arrFullText) : AtisTitleFromFullAtis(depFullText);

        // Swap button states
        BtnDepartureEnabled = !BtnDepartureEnabled;
        BtnArrivalEnabled = !BtnArrivalEnabled;
    }

    private static String AtisTextBodyFromFullAtis(string fullAtisText)
    {
        var atisSplits = fullAtisText.Split(". ").ToList();
        var noTitle = string.Join(". ", atisSplits.GetRange(1, atisSplits.Count - 1));
        var regex = new Regex(Regex.Escape(". "));
        return regex.Replace(noTitle, ".\n\n", 1);
    }

    private static String AtisTitleFromFullAtis(string fullAtisText)
    {
        var atisSplits = fullAtisText.Split(". ").ToList();
        return atisSplits[0].Replace("ARR/DEP ", "");
    }

    private void UpdateDropdownList(IEnumerable<string> airports)
    {
        DisplayedAirportNames.Clear();
        foreach (var airport in airports)
        {
            DisplayedAirportNames.Add(airport);
        }
        DropdownPlaceholderText = "Select an Airport";
        if (!DropdownEnabled) DropdownEnabled = true;
    }

    private IEnumerable<string> FilterVisibleAirportNames(IEnumerable<KeyValuePair<string, Airport>> allAirportDictionary)
    {
        return _onlyZoaAirports ?
            allAirportDictionary.Where(a => Constants.DatisZoaAirports.Contains(a.Key)).Select(x => x.Key)
            : allAirportDictionary.Select(x => x.Key);
    }

    [RelayCommand]
    private void AirportListToggle()
    {
        _onlyZoaAirports = !_onlyZoaAirports;
        UpdateDropdownList(FilterVisibleAirportNames(AllAirportDictionary));
    }
}
