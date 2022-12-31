namespace ZoaInfoTool.Models;

public class Airline
{
    public string IcaoId { get; set; }
    public string? Callsign { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }

    public Airline() { }

    public Airline(string icaoID, string? callsign, string name)
    {
        IcaoId = icaoID;
        Callsign = callsign;
        Name = name;
    }
}
