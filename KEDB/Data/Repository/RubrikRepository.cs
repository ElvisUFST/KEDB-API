using KEDB.Data.Interface;
using KEDB.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Repository
{
    public class RubrikRepository : IRubrikRepository
    {
        private readonly KEDBContext _context;

        public RubrikRepository(KEDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rubrik>> GetAll()
        {
            return await _context.Rubrikker.ToListAsync();
        }
        public async Task<Rubrik> GetById(int id)
        {
            return await _context.Rubrikker.FindAsync(id);
        }

        public async Task<Rubrik> Add(Rubrik rubrik)
        {
            await _context.Rubrikker.AddAsync(rubrik);
            await _context.SaveChangesAsync();

            return rubrik;
        }

        public async Task<Rubrik> Update(Rubrik rubrik)
        {
            _context.Entry(rubrik).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return rubrik;
        }

        public async Task<RubrikValgtFejl> GetRubrikValgtFejlById(int rubrikId, int fejltekstId)
        {
            return await _context.RubrikValgteFejl.FindAsync(rubrikId, fejltekstId);
        }

        public async Task<RubrikValgtFejl> AddRubrikValgtFejl(RubrikValgtFejl rubrikValgtFejl)
        {
            await _context.RubrikValgteFejl.AddAsync(rubrikValgtFejl);
            await _context.SaveChangesAsync();

            return rubrikValgtFejl;
        }

        public async Task<RubrikValgtFejl> RemoveRubrikValgtFejl(int rubrikId, int fejltekstId)
        {
            RubrikValgtFejl rubrikValgtFejl = _context.RubrikValgteFejl.Find(rubrikId, fejltekstId);
            _context.RubrikValgteFejl.Remove(rubrikValgtFejl);
            await _context.SaveChangesAsync();

            return rubrikValgtFejl;
        }

        public async Task<RubrikValgtFejl> UpdateRubrikValgtFejl(RubrikValgtFejl rubrikValgtFejl)
        {
            _context.Entry(rubrikValgtFejl).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return rubrikValgtFejl;
        }
    }
}
