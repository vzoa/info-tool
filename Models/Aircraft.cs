namespace ZoaInfoTool.Models;

public class Aircraft
{
    public string IcaoId { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public string Description { get; set; }
    public string EngineType { get; set; }
    public string EngineCount { get; set; }
    public string FaaWakeTurbulenceCategory { get; set; }

    public Aircraft() { }

    public Aircraft(string icaoId, string manufacturer, string model, string description, string engineType, string engineCount, string faaWakeTurbulenceCategory)
    {
        IcaoId = icaoId;
        Manufacturer = manufacturer;
        Model = model;
        Description = description;
        EngineType = engineType;
        EngineCount = engineCount;
        FaaWakeTurbulenceCategory = faaWakeTurbulenceCategory;
    }
}
