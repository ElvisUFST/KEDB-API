using KEDB.Data.Interface;
using KEDB.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Repository
{
    public class ToldrapportRepository : IToldrapportRepository
    {
        private readonly KEDBContext _context;

        public ToldrapportRepository(KEDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Toldrapport>> GetAll()
        {
            var toldrapporter = _context.Toldrapporter
                .Include(tr => tr.ToldrapportKommunikation)
                .Include(tr => tr.ToldrapportOvertraedelsesAktoer)
                .Include(tr => tr.ToldrapportOpdagendeAktoer)
                .Include(tr => tr.ToldrapportFejlKategori)
                .Include(tr => tr.ToldrapportTransportmiddel)
                .ToListAsync();

            return await toldrapporter;
        }
        public async Task<Toldrapport> GetById(int id)
        {
            var toldrapport = _context.Toldrapporter
                .Include(tr => tr.ToldrapportKommunikation)
                .Include(tr => tr.ToldrapportOvertraedelsesAktoer)
                .Include(tr => tr.ToldrapportOpdagendeAktoer)
                .Include(tr => tr.ToldrapportFejlKategori)
                .Include(tr => tr.ToldrapportTransportmiddel)
                .FirstOrDefaultAsync();

            return await _context.Toldrapporter.FindAsync(id);
        }

        public async Task<Toldrapport> Add(Toldrapport toldrapport)
        {
            await _context.Toldrapporter.AddAsync(toldrapport);
            await _context.SaveChangesAsync();

            return toldrapport;
        }

        public async Task<Toldrapport> Update(Toldrapport toldrapport)
        {
            _context.Entry(toldrapport).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return toldrapport;
        }
    }
}
