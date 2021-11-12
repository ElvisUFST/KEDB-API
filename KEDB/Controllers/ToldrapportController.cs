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
    public class ToldrapportController : ControllerBase
    {
        private readonly IToldrapportRepository _toldrapportRepository;

        public ToldrapportController(IToldrapportRepository toldrapportRepository)
        {
            _toldrapportRepository = toldrapportRepository;
        }

        // GET: api/Toldrapport
        [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Toldrapport>>> GetToldrapporter()
        {
            var getAllToldrapportType = await _toldrapportRepository.GetAll();

            return getAllToldrapportType.ToList();
        }

        // GET: api/Toldrapport/5
        [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Toldrapport>> GetToldrapport(int id)
        {
            var toldrapport = await _toldrapportRepository.GetById(id);

            if (toldrapport == null)
            {
                return NotFound();
            }

            return toldrapport;
        }

        // PUT: api/Toldrapport/5
        [Authorize(Roles = "kedb-super, kedb-write")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToldrapport(int id, Toldrapport toldrapport)
        {
            if (id != toldrapport.Id)
            {
                return BadRequest();
            }

            await _toldrapportRepository.Update(toldrapport);

            return NoContent();
        }

        // POST: api/Toldrapport
        [Authorize(Roles = "kedb-super, kedb-write")]
        [HttpPost]
        public async Task<ActionResult<Toldrapport>> CreateToldrapport(Toldrapport toldrapport)
        {
            await _toldrapportRepository.Add(toldrapport);

            return CreatedAtAction("Toldrapport", new { id = toldrapport.Id }, toldrapport);
        }
    }
}
