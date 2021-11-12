using KEDB.Data.Interface;
using KEDB.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Repository
{
    public class ToldrapportKommunikationRepository : IToldrapportKommunikationRepository
    {
        private readonly KEDBContext _context;

        public ToldrapportKommunikationRepository(KEDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ToldrapportKommunikation>> GetAll()
        {
            return await _context.ToldrapportKommunikationer.ToListAsync();
        }
        public async Task<ToldrapportKommunikation> GetById(int id)
        {
            return await _context.ToldrapportKommunikationer.FindAsync(id);
        }

        public async Task<ToldrapportKommunikation> Add(ToldrapportKommunikation toldrapportKommunikation)
        {
            await _context.ToldrapportKommunikationer.AddAsync(toldrapportKommunikation);
            _context.SaveChanges();

            return toldrapportKommunikation;
        }

        public async Task<ToldrapportKommunikation> Update(ToldrapportKommunikation toldrapportKommunikation)
        {
            _context.Entry(toldrapportKommunikation).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return toldrapportKommunikation;
        }
    }
}