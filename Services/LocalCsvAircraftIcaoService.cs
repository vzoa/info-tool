using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using ZoaInfoTool.Models;
using ZoaInfoTool.Services.Interfaces;

namespace ZoaInfoTool.Services
{
    public class LocalCsvAircraftIcaoService : IAircraftIcaoService
    {
        private string Filename = Constants.AircraftCsvFilePath;

        public Task<Dictionary<string, List<Aircraft>>> FetchAircraftIcaoCodesAsync()
        {
            return Task.Run(() =>
            {
                var returnDict = new Dictionary<string, List<Aircraft>>();
                using (var reader = new StreamReader(Windows.ApplicationModel.Package.Current.InstalledPath + Filename))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<AircraftMap>();
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
            });
        }
    }

    internal class AircraftMap : ClassMap<Aircraft>
    {
        public AircraftMap()
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
}
