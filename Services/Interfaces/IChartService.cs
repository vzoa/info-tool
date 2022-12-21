using System.Collections.Generic;
using System.Threading.Tasks;
using ZoaInfoTool.Models;

namespace ZoaInfoTool.Services.Interfaces
{
    public interface IChartService
    {
        public Task<List<Chart>> FetchChartsAsync(string airportFaaId);
    }
}
