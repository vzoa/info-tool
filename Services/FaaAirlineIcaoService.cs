using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ZoaInfoTool.Models;
using ZoaInfoTool.Services.Interfaces;

namespace ZoaInfoTool.Services
{
    public class FaaAirlineIcaoService : IAirlineIcaoService
    {
        private readonly HttpClient _httpClient;

        public FaaAirlineIcaoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Dictionary<string, Airline>> FetchAirlineIcaoCodesAsync()
        {
            try
            {
                var returnDict = new Dictionary<string, Airline>();

                string responseBody = await _httpClient.GetStringAsync(Constants.FaaContractionsUrl);
                var parser = new HtmlParser();
                IDocument document = await parser.ParseDocumentAsync(responseBody);

                var tables = document.QuerySelector("#main")?.QuerySelectorAll("table");
                foreach (var table in tables)
                {
                    var rows = table.QuerySelector("tbody").QuerySelectorAll("tr");
                    foreach (var row in rows)
                    {
                        var columns = row.QuerySelectorAll("td");
                        string icaoCode = columns[0].TextContent.Trim();
                        string callsign = columns[3].TextContent.Trim();
                        string name = columns[1].TextContent.Trim();

                        returnDict.Add(icaoCode, new Airline(icaoCode, callsign, name));
                    }
                }

                return returnDict;
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }
    }
}
