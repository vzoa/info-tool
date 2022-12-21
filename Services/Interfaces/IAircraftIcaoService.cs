using System.Collections.Generic;
using System.Threading.Tasks;
using ZoaInfoTool.Models;

namespace ZoaInfoTool.Services.Interfaces
{
    public interface IAircraftIcaoService
    {
        public Task<Dictionary<string, List<Aircraft>>> FetchAircraftIcaoCodesAsync();
    }
}
