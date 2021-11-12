using KEDB.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Interface
{
    public interface IFejltekstRepository
    {
        public Task<IEnumerable<Fejltekst>> GetAll();
        public Task<Fejltekst> GetById(int Id);
        public Task<Fejltekst> Add(Fejltekst fejltekst);
        public Task<Fejltekst> Update(Fejltekst fejltekst);
    }
}
