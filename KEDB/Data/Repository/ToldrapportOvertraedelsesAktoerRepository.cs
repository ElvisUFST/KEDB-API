using KEDB.Data.Interface;
using KEDB.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Repository
{
    public class ToldrapportOvertraedelsesAktoerRepository : IToldrapportOvertraedelsesAktoerRepository
    {
        private readonly KEDBContext _context;

        public ToldrapportOvertraedelsesAktoerRepository(KEDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ToldrapportOvertraedelsesAktoer>> GetAll()
        {
            return await _context.ToldrapportOvertraedelsesAktoer.ToListAsync();
        }
        public async Task<ToldrapportOvertraedelsesAktoer> GetById(int id)
        {
            return await _context.ToldrapportOvertraedelsesAktoer.FindAsync(id);
        }

        public async Task<ToldrapportOvertraedelsesAktoer> Add(ToldrapportOvertraedelsesAktoer toldrapportOvertraedelsesAktoer)
        {
            await _context.ToldrapportOvertraedelsesAktoer.AddAsync(toldrapportOvertraedelsesAktoer);
            _context.SaveChanges();

            return toldrapportOvertraedelsesAktoer;
        }

        public async Task<ToldrapportOvertraedelsesAktoer> Update(ToldrapportOvertraedelsesAktoer toldrapportOvertraedelsesAktoer)
        {
            _context.Entry(toldrapportOvertraedelsesAktoer).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return toldrapportOvertraedelsesAktoer;
        }
    }
}