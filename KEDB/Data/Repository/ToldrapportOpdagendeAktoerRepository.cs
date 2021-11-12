using KEDB.Data.Interface;
using KEDB.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Repository
{
    public class ToldrapportOpdagendeAktoerRepository : IToldrapportOpdagendeAktoerRepository
    {
        private readonly KEDBContext _context;

        public ToldrapportOpdagendeAktoerRepository(KEDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ToldrapportOpdagendeAktoer>> GetAll()
        {
            return await _context.ToldrapportOpdagendeAktoer.ToListAsync();
        }
        public async Task<ToldrapportOpdagendeAktoer> GetById(int id)
        {
            return await _context.ToldrapportOpdagendeAktoer.FindAsync(id);
        }

        public async Task<ToldrapportOpdagendeAktoer> Add(ToldrapportOpdagendeAktoer toldrapportOpdagendeAktoer)
        {
            await _context.ToldrapportOpdagendeAktoer.AddAsync(toldrapportOpdagendeAktoer);
            await _context.SaveChangesAsync();

            return toldrapportOpdagendeAktoer;
        }

        public async Task<ToldrapportOpdagendeAktoer> Update(ToldrapportOpdagendeAktoer toldrapportOpdagendeAktoer)
        {
            _context.Entry(toldrapportOpdagendeAktoer).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return toldrapportOpdagendeAktoer;
        }
    }
}
