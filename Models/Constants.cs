namespace ZoaInfoTool.Models;

public class Constants
{
    public const string FlightAwareIfrRouteBaseUrl = "https://flightaware.com/analysis/route.rvt?";
    public const string FaaAirportInfoBaseUrl = "https://nfdc.faa.gov/nfdcApps/services/ajv5/airportDisplay.jsp?";
    public const string VatSpyDataProjectUrl = "https://raw.githubusercontent.com/vatsimnetwork/vatspy-data-project/master/VATSpy.dat";

    public const string AtisApiBaseUrl = "https://datis.clowd.io/api/";
    public const string AtisAllAirports = "all";
    public const int AtisUpdateDelayMilliseconds = 60000; // 1 minute

    public const string SkyVectorBaseUrl = "https://skyvector.com/?fpl=";

    public const string FaaContractionsUrl = "https://www.faa.gov/air_traffic/publications/atpubs/cnt_html/chap3_section_3.html";

    public const string AircraftCsvFilePath = "/Assets/Data/aircraft.csv";

    public const string AirlinesCsvUrl = "https://raw.githubusercontent.com/kengreim/Zoa-Info-Data/main/airlines.csv";
    public const string LoaRulesCsvUrl = "https://raw.githubusercontent.com/kengreim/Zoa-Info-Data/main/loa.csv";
    public const string AliasTxtUrl = "https://raw.githubusercontent.com/kengreim/Zoa-Info-Data/main/ZOA_Alias.txt";
    public const string AircraftCsvUrl = "https://raw.githubusercontent.com/kengreim/Zoa-Info-Data/main/aircraft.csv";

    public const int ChartsCacheTtlSeconds = 86400; // 1 day
    public const int RoutesCacheTtlSeconds = 3600; // 1 hour
}
