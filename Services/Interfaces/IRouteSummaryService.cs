using System.Collections.Generic;
using System.Threading.Tasks;
using ZoaInfoTool.Models;

namespace ZoaInfoTool.Services.Interfaces;

public interface IRouteSummaryService
{
    public Task<List<RouteSummary>> FetchRouteSummariesAsync(string departureIcao, string arrivalIcao);
}
