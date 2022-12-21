using System.Collections.Generic;
using System.Threading.Tasks;
using ZoaInfoTool.Models;

namespace ZoaInfoTool.Services.Interfaces
{
    public interface IAliasRouteService
    {
        public Task<Dictionary<string, List<AliasRoute>>> FetchAliasRoutesAsync();
    }
}
