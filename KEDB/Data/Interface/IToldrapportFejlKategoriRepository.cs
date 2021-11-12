using KEDB.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Interface
{
    public interface IToldrapportFejlKategoriRepository
    {
        public Task<IEnumerable<ToldrapportFejlKategori>> GetAll();
        public Task<ToldrapportFejlKategori> GetById(int id);
        public Task<ToldrapportFejlKategori> Add(ToldrapportFejlKategori toldrapportFejlKategori);
        public Task<ToldrapportFejlKategori> Update(ToldrapportFejlKategori toldrapportFejlKategori);
    }
}