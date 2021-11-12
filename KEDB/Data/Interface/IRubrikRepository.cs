using KEDB.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Interface
{
    public interface IRubrikRepository
    {
        public Task<IEnumerable<Rubrik>> GetAll();
        public Task<Rubrik> GetById(int id);
        public Task<Rubrik> Add(Rubrik rubrik);
        public Task<Rubrik> Update(Rubrik rubrik);

        public Task<RubrikValgtFejl> GetRubrikValgtFejlById(int rubrikId, int fejltekstId);
        public Task<RubrikValgtFejl> AddRubrikValgtFejl(RubrikValgtFejl rubrikValgtFejl);
        public Task<RubrikValgtFejl> RemoveRubrikValgtFejl(int rubrikId, int fejltekstId);
        public Task<RubrikValgtFejl> UpdateRubrikValgtFejl(RubrikValgtFejl rubrikValgtFejl);

    }
}
