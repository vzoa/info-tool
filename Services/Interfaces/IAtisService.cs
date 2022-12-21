using System.Collections.Generic;
using System.Threading.Tasks;
using ZoaInfoTool.Models;

namespace ZoaInfoTool.Services.Interfaces
{
    public interface IAtisService
    {
        public Task<List<Atis>> GetAllAsync();
    }
}
