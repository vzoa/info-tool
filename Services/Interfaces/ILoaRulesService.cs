using System.Collections.Generic;
using System.Threading.Tasks;
using ZoaInfoTool.Models;

namespace ZoaInfoTool.Services.Interfaces;

public interface ILoaRulesService
{
    public Task<List<LoaRule>> FetchLoaRulesAsync();
}
