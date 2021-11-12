using KEDB.Audit;
using KEDB.Data.Interface;
using KEDB.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KEDB.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToldrapportFejlKategoriController : ControllerBase
    {
        private readonly IToldrapportFejlKategoriRepository _toldrapportFejlKategoriRepository;
        private readonly IAuditLog _auditLog;

        public ToldrapportFejlKategoriController(IToldrapportFejlKategoriRepository toldrapportFejlKategoriRepository, IAuditLog auditLog)
        {
            _toldrapportFejlKategoriRepository = toldrapportFejlKategoriRepository;
            _auditLog = auditLog;
        }

        // GET: api/ToldrapportFejlKategori
        [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToldrapportFejlKategori>>> GetToldrapportFejlKategorier()
        {
            var getAllToldrapportFejlKategori = await _toldrapportFejlKategoriRepository.GetAll();

            if (getAllToldrapportFejlKategori == null)
            {
                return NotFound();
            }

            return getAllToldrapportFejlKategori.ToList();
        }

        // GET: api/ToldrapportFejlKategori/5
        [Authorize(Roles = "kedb-super")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ToldrapportFejlKategori>> GetToldrapportFejlKategori(int id)
        {
            var toldrapportFejlKategori = await _toldrapportFejlKategoriRepository.GetById(id);

            if (toldrapportFejlKategori == null)
            {
                return NotFound();
            }

            return toldrapportFejlKategori;
        }

        // Put: api/ToldrapportFejlKategori/5
        [Authorize(Roles = "kedb-super")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToldrapportFejlKategori(int id, ToldrapportFejlKategori toldrapportFejlKategori)
        {
            if (id != toldrapportFejlKategori.Id)
            {
                return BadRequest();
            }

            await _toldrapportFejlKategoriRepository.Update(toldrapportFejlKategori);

            await _auditLog.Log(new UserAction(
                User.Identity.Name,
                UserActionType.Update,
                EntityType.Toldrapport_Fejlkategori,
                id.ToString(),
                toldrapportFejlKategori));

            return NoContent();
        }

        // POST: api/ToldrapportFejlKategori
        [Authorize(Roles = "kedb-super")]
        [HttpPost]
        public async Task<ActionResult<ToldrapportFejlKategori>> CreateToldrapportFejlKategori(ToldrapportFejlKategori toldrapportFejlKategori)
        {
            await _toldrapportFejlKategoriRepository.Add(toldrapportFejlKategori);

            await _auditLog.Log(new UserAction(
                User.Identity.Name,
                UserActionType.Create,
                EntityType.Toldrapport_Fejlkategori,
                toldrapportFejlKategori.Id.ToString(),
                toldrapportFejlKategori));

            return CreatedAtAction("GetToldrapportFejlKategori", new { id = toldrapportFejlKategori.Id }, toldrapportFejlKategori);
        }
    }
}