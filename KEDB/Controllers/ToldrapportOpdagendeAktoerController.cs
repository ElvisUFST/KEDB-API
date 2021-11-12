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
    public class ToldrapportOpdagendeAktoerController : ControllerBase
    {
        private readonly IToldrapportOpdagendeAktoerRepository _toldrapportOpdagendeAktoerRepository;
        private readonly IAuditLog _auditLog;

        public ToldrapportOpdagendeAktoerController(IToldrapportOpdagendeAktoerRepository toldrapportOpdagendeAktoerRepository, IAuditLog auditLog)
        {
            _toldrapportOpdagendeAktoerRepository = toldrapportOpdagendeAktoerRepository;
            _auditLog = auditLog;
        }

        // GET: api/ToldrapportOpdagendeAktoer
        [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToldrapportOpdagendeAktoer>>> GetToldrapportOpdagendeAktoerer()
        {
            var getAllToldrapportOpdagendeAktoerType = await _toldrapportOpdagendeAktoerRepository.GetAll();

            return getAllToldrapportOpdagendeAktoerType.ToList();
        }

        // GET: api/ToldrapportOpdagendeAktoer/5
        [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ToldrapportOpdagendeAktoer>> GetToldrapportOpdagendeAktoer(int id)
        {
            var toldrapportOpdagendeAktoer = await _toldrapportOpdagendeAktoerRepository.GetById(id);

            if (toldrapportOpdagendeAktoer == null)
            {
                return NotFound();
            }

            return toldrapportOpdagendeAktoer;
        }

        // PUT: api/ToldrapportOpdagendeAktoer/5
        [Authorize(Roles = "kedb-super")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToldrapportOpdagendeAktoer(int id, ToldrapportOpdagendeAktoer toldrapportOpdagendeAktoer)
        {
            if (id != toldrapportOpdagendeAktoer.Id)
            {
                return BadRequest();
            }

            await _toldrapportOpdagendeAktoerRepository.Update(toldrapportOpdagendeAktoer);

            await _auditLog.Log(new UserAction(
                User.Identity.Name,
                UserActionType.Update,
                EntityType.Toldrapport_OpdagendeAktører,
                id.ToString(),
                toldrapportOpdagendeAktoer));

            return NoContent();
        }

        // POST: api/ToldrapportOpdagendeAktoer
        [Authorize(Roles = "kedb-super")]
        [HttpPost]
        public async Task<ActionResult<ToldrapportOpdagendeAktoer>> CreateToldrapportOpdagendeAktoer(ToldrapportOpdagendeAktoer toldrapportOpdagendeAktoer)
        {
            await _toldrapportOpdagendeAktoerRepository.Add(toldrapportOpdagendeAktoer);

            await _auditLog.Log(new UserAction(
                User.Identity.Name,
                UserActionType.Create,
                EntityType.Toldrapport_OpdagendeAktører,
                toldrapportOpdagendeAktoer.Id.ToString(),
                toldrapportOpdagendeAktoer));

            return CreatedAtAction("ToldrapportOpdagendeAktoer", new { id = toldrapportOpdagendeAktoer.Id }, toldrapportOpdagendeAktoer);
        }
    }
}
