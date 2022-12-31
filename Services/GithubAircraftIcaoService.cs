using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ZoaInfoTool.Models;
using ZoaInfoTool.Services.Interfaces;

namespace ZoaInfoTool.Services;

public class GithubAircraftIcaoService : IAircraftIcaoService
{
    private readonly HttpClient _httpClient;

    public GithubAircraftIcaoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Dictionary<string, List<Aircraft>>> FetchAircraftIcaoCodesAsync()
    {
        var returnDict = new Dictionary<string, List<Aircraft>>();
        string responseBody = await _httpClient.GetStringAsync(Constants.AircraftCsvUrl);

        using (var csv = new CsvReader(new StringReader(responseBody), CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<CsvAircraftMap>();
            var records = csv.GetRecords<Aircraft>();

            foreach (var record in records)
            {
                if (!returnDict.TryAdd(record.IcaoId, new List<Aircraft> { record }))
                {
                    returnDict[record.IcaoId].Add(record);
                }
            }
        }
        return returnDict;
    }
}

internal class CsvAircraftMap : ClassMap<Aircraft>
{
    public CsvAircraftMap()
    {
        Map(m => m.IcaoId).Name("Type Designator");
        Map(m => m.Manufacturer).Name("Manufacturer");
        Map(m => m.Model).Name("Model");
        Map(m => m.Description).Name("Description");
        Map(m => m.EngineType).Name("Engine Type");
        Map(m => m.EngineCount).Name("Engine Count");
        Map(m => m.FaaWakeTurbulenceCategory).Name("WTC");
    }
}
