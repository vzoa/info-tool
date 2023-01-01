using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZoaInfoTool.Models;
using ZoaInfoTool.Services.Interfaces;

namespace ZoaInfoTool.Services;

public class GithubLoaService : ILoaRulesService
{
    private readonly HttpClient _httpClient;

    public GithubLoaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<LoaRule>> FetchLoaRulesAsync()
    {
        string responseBody = await _httpClient.GetStringAsync(Constants.LoaRulesCsvUrl);

        using var csv = new CsvReader(new StringReader(responseBody), CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<LoaRuleMap>();
        return csv.GetRecords<LoaRule>().ToList<LoaRule>();
    }
}

internal class LoaRuleMap : ClassMap<LoaRule>
{
    public LoaRuleMap()
    {
        Map(m => m.DepartureAirportRegex).Convert(args => new Regex(args.Row.GetField("Departure_Regex")));
        Map(m => m.ArrivalAirportRegex).Convert(args => new Regex(args.Row.GetField("Arrival_Regex")));
        Map(m => m.Route).Name("Route");
        Map(m => m.IsRnavRequired).Name("RNAV Required");
        Map(m => m.Notes).Name("Notes");
    }
}
