using System.Text.Json.Serialization;
using ZoaInfoTool.Services;

namespace ZoaInfoTool.Models;

public class Atis
{
    public string Airport { get; set; }

    [JsonConverter(typeof(AtisTypeEnumConverter))]
    public AtisType Type { get; set; }

    [JsonPropertyName("datis")]
    public string Text { get; set; }

    public Atis(string airport, AtisType type, string text)
    {
        Airport = airport;
        Type = type;
        Text = text;
    }
}

public enum AtisType
{
    Combined,
    Departure,
    Arrival
}
