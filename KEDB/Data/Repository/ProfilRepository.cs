using KEDB.Data.Interface;
using KEDB.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KEDB.Data.Repository
{
    public class ProfilRepository : IProfilRepository
    {
        private readonly KEDBContext _context;

        public ProfilRepository(KEDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Profil>> GetAll()
        {
            return await _context.Profiler.ToListAsync();
        }
        public async Task<Profil> GetById(int id)
        {
            return await _context.Profiler.FindAsync(id);
        }

        public async Task<Profil> Add(Profil profil)
        {
            await _context.Profiler.AddAsync(profil);
            await _context.SaveChangesAsync();

            return profil;
        }

        public async Task<Profil> Update(Profil profil)
        {
            _context.Entry(profil).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return profil;
        }

        public async Task<Profil> GetByProfilNummer(string profileNummer)
        {
            return await _context.Profiler.Where(profil => profil.ProfilNummer == profileNummer).FirstOrDefaultAsync();
        }
    }
}
