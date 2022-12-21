using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using CommunityToolkit.Mvvm.Collections;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZoaInfoTool.Models;
using ZoaInfoTool.Services.Interfaces;

namespace ZoaInfoTool.Services
{
    class FaaChartService : IChartService
    {
        private MemoryCache ChartsCache { get; set; }
        private MemoryCacheEntryOptions CacheEntryOptions { get; set; }
        private readonly HttpClient _httpClient;

        public FaaChartService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            // Create cache with default TTL from Constants model, which is 24 hours
            ChartsCache = new MemoryCache(new MemoryCacheOptions());
            CacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(Constants.ChartsCacheTtlSeconds)
            };
        }

        public async Task<List<Chart>> FetchChartsAsync(string airportFaaId)
        {
            if (ChartsCache.TryGetValue(airportFaaId, out List<Chart> charts))
            {
                return charts;
            }
            else
            {
                var interimDict = new Dictionary<string, Chart>();

                try
                {
                    string responseBody = await _httpClient.GetStringAsync(MakeUrl(airportFaaId));
                    var parser = new HtmlParser();
                    IDocument document = await parser.ParseDocumentAsync(responseBody);

                    // Root element for all charts
                    var chartsSection = document.GetElementById("charts");

                    // Airport Diagram is just a single link
                    var airportDiagramLink = chartsSection?.QuerySelector(":scope > .chartLink")?.QuerySelector("a");
                    if (airportDiagramLink != null)
                    {
                        var airportDiagramName = airportDiagramLink.TextContent;
                        var airportDiagramUrls = new List<Tuple<int, string>> { new Tuple<int, string>(1, airportDiagramLink.GetAttribute("href")) };
                        interimDict.Add(airportDiagramName, new Chart(airportDiagramName, ChartType.AirportDiagram, airportDiagramUrls));
                    }

                    // Now, loop through each Chart Type section (each is a div with class = "row")
                    var typeSections = chartsSection?.QuerySelectorAll("div.row");
                    foreach (var typeSection in typeSections)
                    {
                        // Get the chart type for the section
                        var typeNode = typeSection.QuerySelector("h3");
                        string typeString = typeNode?.TextContent;
                        ChartType type = ChartTypeFromString(typeString);

                        // Get all the links in the section
                        var links = typeNode?.ParentElement?.QuerySelectorAll("a");
                        if (links != null)
                        {
                            // Loop through each link and check if it's a "continued" chart page. If so, find main chart and add link
                            foreach (var link in links)
                            {
                                string linkName = link.TextContent;
                                string linkUrl = link.GetAttribute("href");
                                string chartName = IsContinuedChart(linkName) ? SplitContinuedChartName(linkName).Item1 : linkName;

                                if (interimDict.TryGetValue(chartName, out Chart value))
                                {
                                    Chart existingChart = value;
                                    int lastInt = existingChart.PdfLinks.Last().Item1;
                                    existingChart?.PdfLinks?.Add(new Tuple<int, string>(lastInt + 1, linkUrl));
                                }
                                else
                                {
                                    var linkList = new List<Tuple<int, string>> { new Tuple<int, string>(1, linkUrl) };
                                    var c = new Chart(chartName, type, linkList);
                                    interimDict.Add(chartName, c);
                                }
                            }
                        }

                    }

                    // Sort the PDF links before returning. Have to do it in a weird LINQ way to get the right ordering
                    List<Chart> returnList = interimDict.Values.ToList();
                    foreach (var chart in returnList)
                    {
                        chart.PdfLinks ??= chart.PdfLinks?.OrderBy(p => p.Item2.Length).ThenBy(p => p).ToList();
                    }

                    // Cache and return
                    ChartsCache.Set(airportFaaId, returnList, CacheEntryOptions);
                    return returnList;
                }
                catch (HttpRequestException e)
                {
                    throw e;
                }
                catch (NullReferenceException e)
                {
                    throw e;
                }
            }
        }

        private static string MakeUrl(string airportFaaId)
        {
            return Constants.FaaAirportInfoBaseUrl + "airportId=" + airportFaaId;
        }

        private static ChartType ChartTypeFromString(string? chartType)
        {
            return chartType switch
            {
                "Minimums" => ChartType.AirportMinimums,
                "Standard Terminal Arrival (STAR) Charts" => ChartType.STAR,
                "Departure Procedure (DP) Charts" => ChartType.DP,
                "Instrument Approach Procedure (IAP) Charts" => ChartType.IAP,
                "Hot Spots" => ChartType.HotSpots,
                _ => ChartType.Unknown,
            };
        }

        private static bool IsContinuedChart(string chartName)
        {
            return chartName.Contains(", CONT."); // TODO: could use same Regex as below
        }

        private static (string, int) SplitContinuedChartName(string chartName)
        {
            string pattern = @"(.+), CONT\.([0-9])";
            var m = Regex.Match(chartName, pattern);
            return (m.Groups[1].Value, int.Parse(m.Groups[2].Value) + 1);
        }
    }
}
