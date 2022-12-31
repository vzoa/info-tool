using System.Collections.Generic;
using System.Threading.Tasks;
using ZoaInfoTool.Models;

namespace ZoaInfoTool.Services.Interfaces;

public interface IAirportIcaoService
{
    public Task<Dictionary<string, Airport>> FetchAirportsAsync();
}
