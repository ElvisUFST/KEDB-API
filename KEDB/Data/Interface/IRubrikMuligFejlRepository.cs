using KEDB.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Interface
{
    public interface IRubrikMuligFejlRepository
    {
        public Task<IEnumerable<RubrikMuligFejl>> GetAll();
        public Task<RubrikMuligFejl> GetById(int rubrikTypeId, int profilId, int fejltekstId);
        public Task<RubrikMuligFejl> Add(RubrikMuligFejl rubrikMuligFejl);
        public Task<RubrikMuligFejl> Remove(int rubrikTypeId, int profilId, int fejltekstId);
        public Task<RubrikMuligFejl> Update(RubrikMuligFejl rubrikMuligFejl);
    }
}
