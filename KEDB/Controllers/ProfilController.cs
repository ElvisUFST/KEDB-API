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
    public class ProfilController : ControllerBase
    {
        private readonly IProfilRepository _profilRepository;
        private readonly IAuditLog _auditLog;

        public ProfilController(IProfilRepository profilRepository, IAuditLog auditLog)
        {
            _profilRepository = profilRepository;
            _auditLog = auditLog;
        }

        // GET: api/Profil
        [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profil>>> GetProfiler()
        {
            var getAllProfil = await _profilRepository.GetAll();

            return getAllProfil.ToList();
        }

        // GET: api/Profil/5
        [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Profil>> GetProfil(int id)
        {
            var profil = await _profilRepository.GetById(id);

            if (profil == null)
            {
                return NotFound();
            }

            return profil;
        }

        // PUT: api/Profil/5
        [Authorize(Roles = "kedb-super")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfil(int id, Profil profil)
        {
            if (id != profil.Id)
            {
                return BadRequest();
            }

            await _profilRepository.Update(profil);

            await _auditLog.Log(new UserAction(
                User.Identity.Name,
                UserActionType.Update,
                EntityType.Profil,
                id.ToString(),
                profil));

            return NoContent();
        }
    }
}
