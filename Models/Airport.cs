using System.Collections.Generic;

namespace ZoaInfoTool.Models
{
    public class Airport
    {
        public string IcaoId { get; set; }
        public string IataId { get; set; }
        public string LocalId { get; set; }
        public string Name { get; set; }
        public string Fir { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<Atis>? AtisList { get; set; }

        public Airport(string name, List<Atis> atisList = null)
        {
            Name = name;
            AtisList = atisList;
        }

        public Airport(string icaoId, string iataId, string localId, string name, string fir, double latitude, double longitude)
        {
            IcaoId = icaoId;
            IataId = iataId;
            LocalId = localId;
            Name = name;
            Fir = fir;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
