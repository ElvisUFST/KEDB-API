using KEDB.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Data.Interface
{
    public interface IRubrikTypeRepository
    {
        public Task<IEnumerable<RubrikType>> GetAll();
        public Task<RubrikType> GetById(int id);
        public Task<RubrikType> Add(RubrikType rubrikType);
        public Task<RubrikType> Update(RubrikType rubrikType);

        public RubrikType GetByXmlTag(string xmlTag); //Bliver brugt ved indlæsning af data
    }
}
