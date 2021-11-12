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
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToldrapportOvertraedelsesAktoerController : ControllerBase
    {
        private readonly IToldrapportOvertraedelsesAktoerRepository _toldrapportOvertraedelsesAktoerRepository;
        private readonly IAuditLog _auditLog;

        public ToldrapportOvertraedelsesAktoerController(IToldrapportOvertraedelsesAktoerRepository toldrapportOvertraedelsesAktoerRepository, IAuditLog auditLog)
        {
            _toldrapportOvertraedelsesAktoerRepository = toldrapportOvertraedelsesAktoerRepository;
            _auditLog = auditLog;

        }

        // GET: api/ToldrapportOvertraedelsesAktoer
        // [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToldrapportOvertraedelsesAktoer>>> GetToldrapportOvertraedelsesAktoerer()
        {
            var getAllToldrapportOvertraedelsesAktoer = await _toldrapportOvertraedelsesAktoerRepository.GetAll();

            if (getAllToldrapportOvertraedelsesAktoer == null)
            {
                return NotFound();
            }

            return getAllToldrapportOvertraedelsesAktoer.ToList();
        }

        // GET: api/ToldrapportOvertraedelsesAktoer/5
        // [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ToldrapportOvertraedelsesAktoer>> GetToldrapportOvertraedelsesAktoer(int id)
        {
            var toldrapportOvertraedelsesAktoer = await _toldrapportOvertraedelsesAktoerRepository.GetById(id);

            if (toldrapportOvertraedelsesAktoer == null)
            {
                return NotFound();
            }

            return toldrapportOvertraedelsesAktoer;
        }

        // Put: api/ToldrapportOvertraedelsesAktoer/5
        // [Authorize(Roles = "kedb-super")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToldrapportOvertraedelsesAktoer(int id, ToldrapportOvertraedelsesAktoer toldrapportOvertraedelsesAktoer)
        {
            if (id != toldrapportOvertraedelsesAktoer.Id)
            {
                return BadRequest();
            }

            await _toldrapportOvertraedelsesAktoerRepository.Update(toldrapportOvertraedelsesAktoer);

            await _auditLog.Log(new UserAction(
                User.Identity.Name,
                UserActionType.Update,
                EntityType.Toldrapport_Overtrædelsesaktører,
                id.ToString(),
                toldrapportOvertraedelsesAktoer));

            return NoContent();
        }

        // POST: api/ToldrapportOvertraedelsesAktoer
        // [Authorize(Roles = "kedb-super")]
        [HttpPost]
        public async Task<ActionResult<ToldrapportOvertraedelsesAktoer>> CreateToldrapportOvertraedelsesAktoer(ToldrapportOvertraedelsesAktoer toldrapportOvertraedelsesAktoer)
        {
            await _toldrapportOvertraedelsesAktoerRepository.Add(toldrapportOvertraedelsesAktoer);

            await _auditLog.Log(new UserAction(
                User.Identity.Name,
                UserActionType.Create,
                EntityType.Toldrapport_Overtrædelsesaktører,
                toldrapportOvertraedelsesAktoer.Id.ToString(),
                toldrapportOvertraedelsesAktoer));

            return CreatedAtAction("GetToldrapportOvertraedelsesAktoer", new { id = toldrapportOvertraedelsesAktoer.Id }, toldrapportOvertraedelsesAktoer);
        }
    }
}