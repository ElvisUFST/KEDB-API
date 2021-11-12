using KEDB.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Interface
{
    public interface IProfilRepository
    {
        public Task<IEnumerable<Profil>> GetAll();
        public Task<Profil> GetById(int id);
        public Task<Profil> Add(Profil profil);
        public Task<Profil> Update(Profil profil);

        public Task<Profil> GetByProfilNummer(string profileNummer);
    }
}
