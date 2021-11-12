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
    public class FejltekstController : ControllerBase
    {
        private readonly IFejltekstRepository _fejltekstRepository;
        private readonly IAuditLog _auditLog;

        public FejltekstController(IFejltekstRepository fejltekstRepository, IAuditLog auditLog)
        {
            _fejltekstRepository = fejltekstRepository;
            _auditLog = auditLog;
        }

        // GET: api/Fejltekst
        // [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fejltekst>>> GetFejltekster()
        {
            var getAllFejltekst = await _fejltekstRepository.GetAll();

            if (getAllFejltekst == null)
            {
                return NotFound();
            }

            return getAllFejltekst.ToList();
        }

        // GET: api/Fejltekst/5
        // [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Fejltekst>> GetFejltekst(int id)
        {
            var fejltekst = await _fejltekstRepository.GetById(id);

            if (fejltekst == null)
            {
                return NotFound();
            }

            return fejltekst;
        }

        // Put: api/Fejltekst/5
        // [Authorize(Roles = "kedb-super")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFejltekst(int id, Fejltekst fejltekst)
        {
            if (id != fejltekst.Id)
            {
                return BadRequest();
            }

            await _fejltekstRepository.Update(fejltekst);

            await _auditLog.Log(new UserAction(
                User.Identity.Name,
                UserActionType.Update,
                EntityType.Fejltekst,
                id.ToString(),
                fejltekst));

            return NoContent();
        }

        // POST: api/Fejltekst
        // [Authorize(Roles = "kedb-super")]
        [HttpPost]
        public async Task<ActionResult<Fejltekst>> CreateFejltekst(Fejltekst fejltekst)
        {
            await _fejltekstRepository.Add(fejltekst);

            await _auditLog.Log(new UserAction(
                User.Identity.Name,
                UserActionType.Create,
                EntityType.Fejltekst,
                fejltekst.Id.ToString(),
                fejltekst));

            return CreatedAtAction("GetFejltekst", new { id = fejltekst.Id }, fejltekst);
        }
    }
}