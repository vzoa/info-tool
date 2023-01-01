using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZoaInfoTool.Models;
using ZoaInfoTool.Services.Interfaces;

namespace ZoaInfoTool.Services;

public partial class GithubAliasRouteService : IAliasRouteService
{
    private readonly HttpClient _httpClient;

    public GithubAliasRouteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Dictionary<string, List<AliasRoute>>> FetchAliasRoutesAsync()
    {
        var returnDict = new Dictionary<string, List<AliasRoute>>();
        string responseBody = await _httpClient.GetStringAsync(Constants.AliasTxtUrl);

        using (var reader = new StringReader(responseBody))
        {
            for (string line = reader.ReadLine(); line is not null; line = reader.ReadLine())
            {
                if (!AmRteRegex().IsMatch(line)) continue;
                
                var commandMatch = CommandNameRegex().Match(line);
                var routeMatch = RouteRegex().Match(line);
                if (commandMatch.Success && routeMatch.Success)
                {
                    string departureAirport = commandMatch.Groups[1].Value.ToUpper();
                    int? departureRunway = commandMatch.Groups[2].Value == "" ? null : int.Parse(commandMatch.Groups[2].Value);
                    string arrivalAirport = commandMatch.Groups[3].Value.ToUpper();
                    int? arrivalRunway = commandMatch.Groups[4].Value == "" ? null : int.Parse(commandMatch.Groups[4].Value);

                    RouteType type = commandMatch.Groups[5] is null ? RouteType.Any : AliasRoute.StringToType(commandMatch.Groups[5].Value);
                    string route = routeMatch.Groups[1].Value.Trim();

                    var newAliasRoute = new AliasRoute(departureAirport, departureRunway, arrivalAirport, arrivalRunway, route, type);
                    if (returnDict.TryGetValue(departureAirport, out List<AliasRoute> list))
                    {
                        list.Add(newAliasRoute);
                    }
                    else
                    {
                        returnDict.Add(departureAirport, new List<AliasRoute> { newAliasRoute });
                    }
                }
            }
        }

        return returnDict;
    }

    [GeneratedRegex(@"\.am rte")]
    private static partial Regex AmRteRegex();

    [GeneratedRegex(@"([a-zA-Z0-9]{3})([0-9]{0,2})([a-zA-Z0-9]{3})([0-9]{0,2})([TPJtpj]?)")]
    private static partial Regex CommandNameRegex();

    [GeneratedRegex(@"\.am rte([^\$]*)(\$route)?")]
    private static partial Regex RouteRegex();
}
