using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ZoaInfoTool.Models;
using ZoaInfoTool.Services.Interfaces;

namespace ZoaInfoTool.ViewModels;

public partial class IcaoCodesViewModel : ObservableObject
{
    private IAirlineIcaoService AirlineIcaoFetcher { get; set; }
    private IAircraftIcaoService AircraftIcaoFetcher { get; set; }
    private IAirportIcaoService AirportIcaoFetcher { get; set; }
    private Dictionary<string, Airline> AirlineCodeLookupDict { get; set; }
    private Dictionary<string, Airport> AirportCodeLookupDict { get; set; }
    private Dictionary<string, List<Aircraft>> AircraftCodeLookupDict { get; set; }

    [ObservableProperty]
    private string airlineIcaoCodeInput;

    [ObservableProperty]
    private string airportIcaoCodeInput;

    [ObservableProperty]
    private string aircraftIcaoCodeInput;

    [ObservableProperty]
    private Airline airline;

    [ObservableProperty]
    private bool airlineFound;

    [ObservableProperty]
    private Airport airport;

    [ObservableProperty]
    private bool airportFound;

    public ObservableCollection<Aircraft> Aircrafts { get; set; }

    [ObservableProperty]
    private bool dataIsReady = false;

    [ObservableProperty]
    private bool dataIsNotReady = true;

    public IcaoCodesViewModel(IAirlineIcaoService airlineFetcher, IAircraftIcaoService aircraftFetcher, IAirportIcaoService airportFetcher)
    {
        AirlineIcaoFetcher = airlineFetcher;
        AircraftIcaoFetcher = aircraftFetcher;
        AirportIcaoFetcher = airportFetcher;
        Aircrafts = new ObservableCollection<Aircraft>();
        InitializeAsync();
    }

    public async void InitializeAsync()
    {
        AirlineCodeLookupDict = await AirlineIcaoFetcher.FetchAirlineIcaoCodesAsync();
        AircraftCodeLookupDict = await AircraftIcaoFetcher.FetchAircraftIcaoCodesAsync();
        AirportCodeLookupDict = await AirportIcaoFetcher.FetchAirportsAsync();
        DataIsNotReady = false;
        DataIsReady = true;
    }

    [RelayCommand]
    public void LookupAirline()
    {
        if (AirlineCodeLookupDict is not null)
        {
            if (AirlineCodeLookupDict.TryGetValue(AirlineIcaoCodeInput.ToUpper(), out Airline value))
            {
                Airline = value;
                AirlineFound = true;
            }
            else
            {
                AirlineFound = false;
            }
        }
    }

    [RelayCommand]
    public void LookupAirport()
    {
        if (AirportCodeLookupDict is not null)
        {
            if (AirportCodeLookupDict.TryGetValue(AirportIcaoCodeInput.ToUpper(), out Airport value))
            {
                Airport = value;
                AirportFound = true;
            }
            else
            {
                AirportFound = false;
            }
        }
    }

    [RelayCommand]
    public void LookupAircraft()
    {
        Aircrafts.Clear();
        if (AircraftCodeLookupDict is not null)
        {
            if (AircraftCodeLookupDict.TryGetValue(AircraftIcaoCodeInput.ToUpper(), out List<Aircraft> values))
            {
                foreach (var aircraft in values)
                {
                    Aircrafts.Add(aircraft);
                }
            }
        }
    }
}
