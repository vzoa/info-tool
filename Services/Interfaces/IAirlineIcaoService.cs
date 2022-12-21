using System.Collections.Generic;
using System.Threading.Tasks;
using ZoaInfoTool.Models;

namespace ZoaInfoTool.Services.Interfaces
{
    public interface IAirlineIcaoService
    {
        public Task<Dictionary<string, Airline>> FetchAirlineIcaoCodesAsync();
    }
}
