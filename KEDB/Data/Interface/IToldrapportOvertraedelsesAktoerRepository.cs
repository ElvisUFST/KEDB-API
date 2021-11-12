using KEDB.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Interface
{
    public interface IToldrapportOvertraedelsesAktoerRepository
    {
        public Task<IEnumerable<ToldrapportOvertraedelsesAktoer>> GetAll();
        public Task<ToldrapportOvertraedelsesAktoer> GetById(int id);
        public Task<ToldrapportOvertraedelsesAktoer> Add(ToldrapportOvertraedelsesAktoer toldrapportOvertraedelsesAktoer);
        public Task<ToldrapportOvertraedelsesAktoer> Update(ToldrapportOvertraedelsesAktoer toldrapportOvertraedelsesAktoer);
    }
}
