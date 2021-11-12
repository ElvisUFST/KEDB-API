using KEDB.Data.Interface;
using KEDB.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KEDB.Data.Repository
{
    public class RubrikTypeRepository : IRubrikTypeRepository
    {
        private readonly KEDBContext _context;

        public RubrikTypeRepository(KEDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RubrikType>> GetAll()
        {
            return await _context.RubrikTyper.ToListAsync();
        }
        public async Task<RubrikType> GetById(int id)
        {
            return await _context.RubrikTyper.FindAsync(id);
        }

        public async Task<RubrikType> Add(RubrikType rubrikType)
        {
            await _context.RubrikTyper.AddAsync(rubrikType);
            await _context.SaveChangesAsync();

            return rubrikType;
        }

        public async Task<RubrikType> Update(RubrikType rubrikType)
        {
            _context.Entry(rubrikType).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return rubrikType;
        }

        //Bliver brugt ved indlæsning af data
        public RubrikType GetByXmlTag(string xmlTag)
        {
            var rubrikType = _context.RubrikTyper
            .Where(s => s.XmlTag == xmlTag)
            .FirstOrDefault();

            return rubrikType;
        }
    }
}
