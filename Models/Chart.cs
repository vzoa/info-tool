using System;
using System.Collections.Generic;

namespace ZoaInfoTool.Models;

public enum ChartType
{
    AirportDiagram,
    AirportMinimums,
    STAR,
    DP,
    IAP,
    HotSpots,
    Unknown
}

public class Chart
{
    public string Name { get; set; }
    public ChartType Type { get; set; }
    public List<Tuple<int, string>>? PdfLinks { get; set; }

    public Chart(string name, ChartType type, List<Tuple<int, string>>? pdfLinks = null)
    {
        Name = name;
        Type = type;
        PdfLinks = pdfLinks;
    }
}
