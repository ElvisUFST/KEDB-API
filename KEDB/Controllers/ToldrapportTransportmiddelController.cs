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
    public class ToldrapportTransportmiddelController : ControllerBase
    {
        private readonly IToldrapportTransportmiddelRepository _toldrapportTransportmiddelRepository;
        private readonly IAuditLog _auditLog;

        public ToldrapportTransportmiddelController(IToldrapportTransportmiddelRepository toldrapportTransportmiddelRepository, IAuditLog auditLog)
        {
            _toldrapportTransportmiddelRepository = toldrapportTransportmiddelRepository;
            _auditLog = auditLog;
        }

        // GET: api/ToldrapportTransportmiddel
        [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToldrapportTransportmiddel>>> GetToldrapportTransportmiddeler()
        {
            var getAllToldrapportTransportmiddel = await _toldrapportTransportmiddelRepository.GetAll();

            return getAllToldrapportTransportmiddel.ToList();
        }

        // GET: api/ToldrapportTransportmiddel/5
        [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ToldrapportTransportmiddel>> GetToldrapportTransportmiddel(int id)
        {
            var toldrapportTransportmiddel = await _toldrapportTransportmiddelRepository.GetById(id);

            if (toldrapportTransportmiddel == null)
            {
                return NotFound();
            }

            return toldrapportTransportmiddel;
        }

        // PUT: api/ToldrapportTransportmiddel/5
        [Authorize(Roles = "kedb-super")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToldrapportTransportmiddel(int id, ToldrapportTransportmiddel toldrapportTransportmiddel)
        {
            if (id != toldrapportTransportmiddel.Id)
            {
                return BadRequest();
            }

            await _toldrapportTransportmiddelRepository.Update(toldrapportTransportmiddel);

            await _auditLog.Log(new UserAction(
                User.Identity.Name,
                UserActionType.Update,
                EntityType.Toldrapport_Transportmidle,
                id.ToString(),
                toldrapportTransportmiddel));

            return NoContent();
        }

        // POST: api/ToldrapportTransportmiddel
        [Authorize(Roles = "kedb-super")]
        [HttpPost]
        public async Task<ActionResult<ToldrapportTransportmiddel>> CreateToldrapportTransportmiddel(ToldrapportTransportmiddel toldrapportTransportmiddel)
        {
            await _toldrapportTransportmiddelRepository.Add(toldrapportTransportmiddel);

            await _auditLog.Log(new UserAction(
                User.Identity.Name,
                UserActionType.Create,
                EntityType.Toldrapport_Transportmidle,
                toldrapportTransportmiddel.Id.ToString(),
                toldrapportTransportmiddel));

            return CreatedAtAction("GetToldrapportTransportmiddel", new { id = toldrapportTransportmiddel.Id }, toldrapportTransportmiddel);
        }
    }
}