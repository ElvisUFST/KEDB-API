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
    public class ToldrapportKommunikationController : ControllerBase
    {
        private readonly IToldrapportKommunikationRepository _toldrapportKommunikationRepository;
        private readonly IAuditLog _auditLog;

        public ToldrapportKommunikationController(IToldrapportKommunikationRepository toldrapportKommunikationRepository, IAuditLog auditLog)
        {
            _toldrapportKommunikationRepository = toldrapportKommunikationRepository;
            _auditLog = auditLog;
        }

        // GET: api/ToldrapportKommunikation
        [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToldrapportKommunikation>>> GetToldrapportKommunikationer()
        {
            var getAllToldrapportKommunikation = await _toldrapportKommunikationRepository.GetAll();

            if (getAllToldrapportKommunikation == null)
            {
                return NotFound();
            }

            return getAllToldrapportKommunikation.ToList();
        }

        // GET: api/ToldrapportKommunikation/5
        [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ToldrapportKommunikation>> GetToldrapportKommunikation(int id)
        {
            var toldrapportKommunikation = await _toldrapportKommunikationRepository.GetById(id);

            if (toldrapportKommunikation == null)
            {
                return NotFound();
            }

            return toldrapportKommunikation;
        }

        // Put: api/ToldrapportKommunikation/5
        [Authorize(Roles = "kedb-super")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToldrapportKommunikation(int id, ToldrapportKommunikation toldrapportKommunikation)
        {
            if (id != toldrapportKommunikation.Id)
            {
                return BadRequest();
            }

            await _toldrapportKommunikationRepository.Update(toldrapportKommunikation);

            await _auditLog.Log(new UserAction(
                User.Identity.Name,
                UserActionType.Update,
                EntityType.Toldrapport_Kommunikationsform,
                id.ToString(),
                toldrapportKommunikation));

            return NoContent();
        }

        // POST: api/ToldrapportKommunikation
        [Authorize(Roles = "kedb-super")]
        [HttpPost]
        public async Task<ActionResult<ToldrapportKommunikation>> CreateToldrapportKommunikation(ToldrapportKommunikation toldrapportKommunikation)
        {
            await _toldrapportKommunikationRepository.Add(toldrapportKommunikation);

            await _auditLog.Log(new UserAction(
                User.Identity.Name,
                UserActionType.Create,
                EntityType.Toldrapport_Kommunikationsform,
                toldrapportKommunikation.Id.ToString(),
                toldrapportKommunikation));

            return CreatedAtAction("GetToldrapportKommunikation", new { id = toldrapportKommunikation.Id }, toldrapportKommunikation);
        }
    }
}