using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZoaInfoTool.Models;
using ZoaInfoTool.Services.Interfaces;

namespace ZoaInfoTool.Services
{
    public partial class VatSpyDataService : IAirportIcaoService
    {
        private readonly HttpClient _httpClient;
        public VatSpyDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Dictionary<string, Airport>> FetchAirportsAsync()
        {
            var returnDict = new Dictionary<string, Airport>();
            string responseBody = await _httpClient.GetStringAsync(Constants.VatSpyDataProjectUrl);

            using (var reader = new StringReader(responseBody))
            {
                bool inAirportsSection = false;
                for (string line = reader.ReadLine(); line is not null; line = reader.ReadLine())
                {
                    // Skip if line is empty or a commented line starting with ;
                    if (line.Trim() == "" || line.StartsWith(";"))
                    {
                        continue;
                    }

                    // Check if we're at a new section
                    if (SectionHeaderRegex().IsMatch(line))
                    {
                        // Section header found. Check if it's the Airports section
                        string header = SectionHeaderRegex().Match(line).Groups[1].Value;
                        inAirportsSection = header.ToUpper() == "AIRPORTS";
                        continue; // Skip parsing current line because we know it's just a header, no data
                    }

                    // If we're in the Airports section, parse lines as airports
                    if (inAirportsSection)
                    {
                        // ICAO|Airport Name|Latitude Decimal|Longitude Decimal|IATA/LID|FIR|IsPseudo
                        string[] fields = line.Split('|');
                        string icao = fields[0].Trim();
                        string name = fields[1].Trim();
                        double latitude = double.Parse(fields[2].Trim());
                        double longitude = double.Parse(fields[3].Trim());
                        string iata = fields[4].Trim();
                        string lid = iata;
                        string fir = fields[5].Trim();
                        bool isPseudo = int.Parse(fields[6].Trim().Substring(0, 1)) == 1;

                        if (!isPseudo)
                        {
                            // A few Australian airports have multiple entries, so we need to use TryAdd to not throw exception
                            returnDict.TryAdd(icao, new Airport(icao, iata, lid, name, fir, latitude, longitude));
                        }
                    }
                }
            }

            return returnDict;
        }

        [GeneratedRegex(@"\[(.*)\]")]
        private static partial Regex SectionHeaderRegex();
    }
}
