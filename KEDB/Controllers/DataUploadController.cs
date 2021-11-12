using KEDB.Data.Interface;
using KEDB_DataParser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KEDB.Controllers
{
    [Route("data")]
    [ApiController]
    public class DataUploadController : ControllerBase
    {
        private readonly IRubrikTypeRepository _rubrikTypeRepository;
        private readonly IKontrolrapportRepository _kontrolrapportRepository;
        private readonly IProfilRepository _profilRepository;
        private readonly IRubrikRepository _rubrikRepository;

        public DataUploadController(IRubrikTypeRepository rubrikTypeRepository, IKontrolrapportRepository kontrolrapportRepository, IProfilRepository profilRepository, IRubrikRepository rubrikRepository)
        {
            _rubrikTypeRepository = rubrikTypeRepository;
            _kontrolrapportRepository = kontrolrapportRepository;
            _profilRepository = profilRepository;
            _rubrikRepository = rubrikRepository;
        }

        public class FileUploadAPI
        {
            public IFormFile files { get; set; }
        }

        [HttpPost("{data}")]
        public async Task<ActionResult> Post(IFormFile data)
        {
            try
            {
                if (data.Length > 0)
                {
                    //DataParser dataParser = new DataParser(_rubrikTypeRepository, _kontrolrapportRepository, _profilRepository);
                    DataParserTMP dataParser = new DataParserTMP(_rubrikTypeRepository, _kontrolrapportRepository, _profilRepository, _rubrikRepository); //til midlertidig dataoverførsel
                    await dataParser.Parse(data);
                    return Ok();
                }
                else return BadRequest("1");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}