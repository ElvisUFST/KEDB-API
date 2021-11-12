using KEDB.Dto;
using KEDB.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Interface
{
    public interface IKontrolrapportRepository
    {
        public Task<IEnumerable<Kontrolrapport>> GetAll(KontrolrapportSearchFilterDto searchFilter);
        public Task<Kontrolrapport> GetById(int id);
        public Task<List<int>> GetToldsteder();
        public Task<Kontrolrapport> Add(Kontrolrapport kontrolrapport);
        public Task<Kontrolrapport> Update(Kontrolrapport kontrolrapport);
        public IEnumerable<Kontrolrapport> GetAllByYear(int year);
    }
}