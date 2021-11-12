using KEDB.Data.Interface;
using KEDB.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KEDB.Data.Repository
{
    public class RubrikMuligFejlRepository : IRubrikMuligFejlRepository
    {
        private readonly KEDBContext _context;

        public RubrikMuligFejlRepository(KEDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RubrikMuligFejl>> GetAll()
        {
            var rubrikMuligeFejl = _context.RubrikMuligeFejl
                .Include(rmf => rmf.Fejltekst)
                .Include(rmf => rmf.RubrikType)
                .Include(rmf => rmf.Profil)
                .ToListAsync();

            return await rubrikMuligeFejl;
        }
        public async Task<RubrikMuligFejl> GetById(int rubrikTypeId, int profilId, int fejltekstId)
        {
            //var rubrikMuligFejl = _context.RubrikMuligFejlList.Where(r => r.RubrikTypeID == rubrikTypeId && r.ProfilId == profilId && r.FejltekstId == fejltekstId).FirstOrDefault();

            var rubrikMuligFejl = _context.RubrikMuligeFejl
                .Include(rmf => rmf.Fejltekst)
                .Include(rmf => rmf.RubrikType)
                .Include(rmf => rmf.Profil)
                .Where(rmf => rmf.RubrikTypeId == rubrikTypeId && rmf.ProfilId == profilId && rmf.FejltekstId == fejltekstId)
                .FirstOrDefaultAsync();

            return await rubrikMuligFejl;
        }

        public async Task<RubrikMuligFejl> Add(RubrikMuligFejl rubrikMuligFejl)
        {
            // Validering/Tjekker om der allerede eksister et entety af den RubrikMuligFejl
            // var rubrikMuligFejlValidation = _context.RubrikMuligFejlList.Where(r => r.RubrikTypeID == rubrikMuligFejl.RubrikTypeID && r.ProfilId == rubrikMuligFejl.ProfilId && r.FejltekstId == rubrikMuligFejl.FejltekstId).FirstOrDefault();

            await _context.RubrikMuligeFejl.AddAsync(rubrikMuligFejl);
            await _context.SaveChangesAsync();

            return rubrikMuligFejl;
        }

        public async Task<RubrikMuligFejl> Remove(int rubrikTypeId, int profilId, int fejltekstId)
        {
            //var rubrikMuligFejlId = _context.RubrikMuligFejlList.Where(r => r.RubrikTypeID == rubrikTypeId && r.ProfilId == profilId && r.FejltekstId == fejltekstId).FirstOrDefault();

            RubrikMuligFejl rubrikMuligFejl = _context.RubrikMuligeFejl.Find(rubrikTypeId, profilId, fejltekstId);
            _context.RubrikMuligeFejl.Remove(rubrikMuligFejl);
            await _context.SaveChangesAsync();

            return rubrikMuligFejl;
        }

        public async Task<RubrikMuligFejl> Update(RubrikMuligFejl rubrikMuligFejl)
        {
            _context.Entry(rubrikMuligFejl).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return rubrikMuligFejl;
        }
    }
}
