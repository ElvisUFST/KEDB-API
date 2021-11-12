using KEDB.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Interface
{
    public interface IToldrapportRepository
    {
        public Task<IEnumerable<Toldrapport>> GetAll();
        public Task<Toldrapport> GetById(int id);
        public Task<Toldrapport> Add(Toldrapport rubrikType);
        public Task<Toldrapport> Update(Toldrapport rubrikType);
    }
}
