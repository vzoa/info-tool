namespace ZoaInfoTool.Models;

public class RouteSummary
{
    public string DepartureAirport { get; set; }
    public string ArrivalAirport { get; set; }
    public string Route { get; set; }
    public string AltitudeRange { get; set; }
    public int RouteFrequency { get; set; }

    public RouteSummary(string departureAirport, string arrivalAirport, string route, string altitudeRange, int routeFrequency)
    {
        DepartureAirport = departureAirport;
        ArrivalAirport = arrivalAirport;
        Route = route;
        AltitudeRange = altitudeRange;
        RouteFrequency = routeFrequency;
    }
}
