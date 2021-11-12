using KEDB.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Interface
{
    public interface IToldrapportKommunikationRepository
    {
        public Task<IEnumerable<ToldrapportKommunikation>> GetAll();
        public Task<ToldrapportKommunikation> GetById(int id);
        public Task<ToldrapportKommunikation> Add(ToldrapportKommunikation toldrapportKommunikation);
        public Task<ToldrapportKommunikation> Update(ToldrapportKommunikation toldrapportKommunikation);
    }
}
