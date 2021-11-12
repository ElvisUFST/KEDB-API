using KEDB.Data.Interface;
using KEDB.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Repository
{
    public class ToldrapportTransportmiddelRepository : IToldrapportTransportmiddelRepository
    {
        private readonly KEDBContext _context;

        public ToldrapportTransportmiddelRepository(KEDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ToldrapportTransportmiddel>> GetAll()
        {
            return await _context.ToldrapportTransportmiddeler.ToListAsync();
        }
        public async Task<ToldrapportTransportmiddel> GetById(int id)
        {
            return await _context.ToldrapportTransportmiddeler.FindAsync(id);
        }

        public async Task<ToldrapportTransportmiddel> Add(ToldrapportTransportmiddel toldrapportTransportmiddel)
        {
            await _context.ToldrapportTransportmiddeler.AddAsync(toldrapportTransportmiddel);
            await _context.SaveChangesAsync();

            return toldrapportTransportmiddel;
        }

        public async Task<ToldrapportTransportmiddel> Update(ToldrapportTransportmiddel toldrapportTransportmiddel)
        {
            _context.Entry(toldrapportTransportmiddel).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return toldrapportTransportmiddel;
        }
    }
}
