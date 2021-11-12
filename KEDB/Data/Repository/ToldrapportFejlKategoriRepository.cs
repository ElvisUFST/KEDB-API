using KEDB.Data.Interface;
using KEDB.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Repository
{
    public class ToldrapportFejlKategoriRepository : IToldrapportFejlKategoriRepository
    {
        private readonly KEDBContext _context;

        public ToldrapportFejlKategoriRepository(KEDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ToldrapportFejlKategori>> GetAll()
        {
            return await _context.ToldrapportFejlKategorier.ToListAsync();
        }
        public async Task<ToldrapportFejlKategori> GetById(int id)
        {
            return await _context.ToldrapportFejlKategorier.FindAsync(id);
        }

        public async Task<ToldrapportFejlKategori> Add(ToldrapportFejlKategori toldrapportFejlKategori)
        {
            await _context.ToldrapportFejlKategorier.AddAsync(toldrapportFejlKategori);
            _context.SaveChanges();

            return toldrapportFejlKategori;
        }

        public async Task<ToldrapportFejlKategori> Update(ToldrapportFejlKategori toldrapportFejlKategori)
        {
            _context.Entry(toldrapportFejlKategori).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return toldrapportFejlKategori;
        }
    }
}
