using KEDB.Data.Interface;
using KEDB.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Repository
{
    public class FejltekstRepository : IFejltekstRepository
    {
        private readonly KEDBContext _context;

        public FejltekstRepository(KEDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Fejltekst>> GetAll()
        {
            return await _context.Fejltekster.ToListAsync();
        }
        public async Task<Fejltekst> GetById(int Id)
        {
            return await _context.Fejltekster.FindAsync(Id);
        }

        public async Task<Fejltekst> Add(Fejltekst fejltekst)
        {
            await _context.Fejltekster.AddAsync(fejltekst);
            _context.SaveChanges();

            return fejltekst;
        }

        public async Task<Fejltekst> Update(Fejltekst fejltekst)
        {
            _context.Entry(fejltekst).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return fejltekst;
        }
    }
}
