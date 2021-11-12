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
    public class RubrikTypeController : ControllerBase
    {
        private readonly IRubrikTypeRepository _rubrikTypeRepository;
        private readonly IAuditLog _auditLog;

        public RubrikTypeController(IRubrikTypeRepository rubrikTypeRepository, IAuditLog auditLog)
        {
            _rubrikTypeRepository = rubrikTypeRepository;
            _auditLog = auditLog;
        }

        // GET: api/RubrikType
        [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RubrikType>>> GetRubrikTyper()
        {
            var getAllRubrikType = await _rubrikTypeRepository.GetAll();

            return getAllRubrikType.OrderBy(rt => rt.Nummer).ToList();
        }

        // GET: api/RubrikType/5
        [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet("{id}")]
        public async Task<ActionResult<RubrikType>> GetRubrikType(int id)
        {
            var rubrikType = await _rubrikTypeRepository.GetById(id);

            if (rubrikType == null)
            {
                return NotFound();
            }

            return rubrikType;
        }

        // PUT: api/RubrikType/5
        [Authorize(Roles = "kedb-super, kedb-write")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRubrtikType(int id, RubrikType rubrikType)
        {
            if (id != rubrikType.Id)
            {
                return BadRequest();
            }

            await _auditLog.Log(new UserAction(
                User.Identity.Name,
                UserActionType.Update,
                EntityType.Rubriktype,
                id.ToString(),
                rubrikType));

            await _rubrikTypeRepository.Update(rubrikType);

            return NoContent();
        }

        // POST: api/RubrikType
        [Authorize(Roles = "kedb-super")]
        [HttpPost]
        public async Task<ActionResult<RubrikType>> CreateRubrikType(RubrikType rubrikType)
        {
            await _rubrikTypeRepository.Add(rubrikType);

            await _auditLog.Log(new UserAction(
                User.Identity.Name,
                UserActionType.Create,
                EntityType.Rubriktype,
                rubrikType.Id.ToString(),
                rubrikType));

            return CreatedAtAction("RubrikType", new { id = rubrikType.Id }, rubrikType);
        }
    }
}