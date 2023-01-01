using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
    }

    [RelayCommand]
    public async void InitializeAsync()
    {
        if (DataIsReady) return;

        var airlineTask = AirlineIcaoFetcher.FetchAirlineIcaoCodesAsync();
        var aircraftTask = AircraftIcaoFetcher.FetchAircraftIcaoCodesAsync();
        var airportTask = AirportIcaoFetcher.FetchAirportsAsync();

        await Task.WhenAll(airlineTask, aircraftTask, airportTask);
        AirlineCodeLookupDict = airlineTask.Result;
        AircraftCodeLookupDict = aircraftTask.Result;
        AirportCodeLookupDict = airportTask.Result;
        DataIsNotReady = false;
        DataIsReady = true;
    }

    [RelayCommand]
    public void LookupAirline()
    {
        if (AirportCodeLookupDict is null) return;

        AirlineFound = AirlineCodeLookupDict.TryGetValue(AirlineIcaoCodeInput.ToUpper(), out Airline value);
        if (AirlineFound) Airline = value;
    }

    [RelayCommand]
    public void LookupAirport()
    {
        if (AirportCodeLookupDict is null) return;

        AirportFound = AirportCodeLookupDict.TryGetValue(AirportIcaoCodeInput.ToUpper(), out Airport value);
        if (AirportFound) Airport = value;
    }

    [RelayCommand]
    public void LookupAircraft()
    {
        Aircrafts.Clear();

        if (AircraftCodeLookupDict is null) return;

        if (AircraftCodeLookupDict.TryGetValue(AircraftIcaoCodeInput.ToUpper(), out List<Aircraft> values))
        {
            foreach (var aircraft in values)
            {
                Aircrafts.Add(aircraft);
            }
        }
    }
}
