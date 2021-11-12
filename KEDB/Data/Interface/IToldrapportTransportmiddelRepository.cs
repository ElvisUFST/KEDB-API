using KEDB.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Interface
{
    public interface IToldrapportTransportmiddelRepository
    {
        public Task<IEnumerable<ToldrapportTransportmiddel>> GetAll();
        public Task<ToldrapportTransportmiddel> GetById(int id);
        public Task<ToldrapportTransportmiddel> Add(ToldrapportTransportmiddel toldrapportTransportmiddel);
        public Task<ToldrapportTransportmiddel> Update(ToldrapportTransportmiddel toldrapportTransportmiddel);
    }
}
