using KEDB.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Interface
{
    public interface IToldrapportOpdagendeAktoerRepository
    {
        public Task<IEnumerable<ToldrapportOpdagendeAktoer>> GetAll();
        public Task<ToldrapportOpdagendeAktoer> GetById(int id);
        public Task<ToldrapportOpdagendeAktoer> Add(ToldrapportOpdagendeAktoer toldrapportOpdagendeAktoer);
        public Task<ToldrapportOpdagendeAktoer> Update(ToldrapportOpdagendeAktoer toldrapportOpdagendeAktoer);
    }
}
