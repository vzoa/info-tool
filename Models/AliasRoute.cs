namespace ZoaInfoTool.Models
{
    public class AliasRoute
    {
        public string DepartureAirport { get; set; }
        public int? DepartureRunway { get; set; }
        public string ArrivalAirport { get; set; }
        public int? ArrivalRunway { get; set; }
        public string Route { get; set; }
        public RouteType RouteType { get; set; }

        public AliasRoute(string departureAirport, int? departureRunway, string arrivalAirport, int? arrivalRunway, string route, RouteType routeType)
        {
            DepartureAirport = departureAirport;
            DepartureRunway = departureRunway;
            ArrivalAirport = arrivalAirport;
            ArrivalRunway = arrivalRunway;
            Route = route;
            RouteType = routeType;
        }

        public static RouteType StringToType(string typeStr)
        {
            return typeStr.ToUpper() switch
            {
                "J" => RouteType.Jet,
                "T" => RouteType.Turboprop,
                "P" => RouteType.Prop,
                _ => RouteType.Any
            };
        }
    }

    public enum RouteType
    {
        Jet,
        Turboprop,
        Prop,
        Any
    }
}
