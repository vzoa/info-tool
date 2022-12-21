using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ZoaInfoTool.Models;
using ZoaInfoTool.Services.Interfaces;

namespace ZoaInfoTool.Services
{
    public class ClowdDatisFetchService : IAtisService
    {
        public string ApiBaseUrl { get; }
        private readonly HttpClient _client;
        private static JsonSerializerOptions _jsonOptions;

        public ClowdDatisFetchService(HttpClient httpClient)
        {
            _client = httpClient;
            ApiBaseUrl = Constants.AtisApiBaseUrl;
            _client.BaseAddress = new Uri(ApiBaseUrl);
            _jsonOptions = new JsonSerializerOptions
            {
                Converters =
                {
                    new AtisTypeEnumConverter()
                },
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<List<Atis>> GetAllAsync()
        {
            try
            {
                return await _client.GetFromJsonAsync<List<Atis>>(Constants.AtisAllAirports, _jsonOptions);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class AtisTypeEnumConverter : JsonConverter<AtisType>
    {
        public override AtisType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string enumValue = reader.GetString();
            return enumValue switch
            {
                "combined" => AtisType.Combined,
                "dep" => AtisType.Departure,
                "arr" => AtisType.Arrival,
                _ => throw new JsonException(),
            };
        }

        public override void Write(Utf8JsonWriter writer, AtisType value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case AtisType.Combined:
                    writer.WriteStringValue("combined");
                    break;
                case AtisType.Departure:
                    writer.WriteStringValue("dep");
                    break;
                case AtisType.Arrival:
                    writer.WriteStringValue("arr");
                    break;
                default:
                    throw new JsonException();
            }
        }
    }
}
