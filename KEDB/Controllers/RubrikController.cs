using KEDB.Data.Interface;
using KEDB.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KEDB.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RubrikController : ControllerBase
    {
        private readonly IRubrikRepository _RubrikRepository;

        public RubrikController(IRubrikRepository rubrikRepository)
        {
            _RubrikRepository = rubrikRepository;
        }

        // GET: api/Rubrik
        [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rubrik>>> GetRubrikker()
        {
            var getAllRubrikType = await _RubrikRepository.GetAll();

            return getAllRubrikType.ToList();
        }

        // GET: api/Rubrik/5
        [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Rubrik>> GetRubrik(int id)
        {
            var rubrik = await _RubrikRepository.GetById(id);

            if (rubrik == null)
            {
                return NotFound();
            }

            return rubrik;
        }

        // PUT: api/Rubrik/5
        [Authorize(Roles = "kedb-super, kedb-write")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRubrik(int id, Rubrik rubrik)
        {
            if (id != rubrik.Id)
            {
                return BadRequest();
            }

            await _RubrikRepository.Update(rubrik);

            return NoContent();
        }

        //Der må ikke findes Create/delete af rubrikker. Dette skal ikke være muligt.

        // PUT: api/Rubrik/5
        [Authorize(Roles = "kedb-super, kedb-write")]
        [HttpPut("Fejltekst/{rubrikId}/{fejltekstId}")]
        public async Task<IActionResult> UpdateRubrikValgtFejl(int rubrikId, int fejltekstId, RubrikValgtFejl rubrikValgtFejl)
        {
            if (rubrikId != rubrikValgtFejl.RubrikId && fejltekstId != rubrikValgtFejl.FejltekstId)
            {
                return BadRequest();
            }

            await _RubrikRepository.UpdateRubrikValgtFejl(rubrikValgtFejl);

            return NoContent();
        }

        // POST: api/Rubrik/Fejltekst
        //[Authorize(Roles="kedb-super, kedb-write")]
        [HttpPost("Fejltekst")]
        public async Task<ActionResult<RubrikValgtFejl>> CreateRubrikValgtFejl(RubrikValgtFejl rubrikValgtFejl)
        {
            await _RubrikRepository.AddRubrikValgtFejl(rubrikValgtFejl);

            return CreatedAtAction("Rubrik", new { id = rubrikValgtFejl.RubrikId, rubrikValgtFejl.FejltekstId }, rubrikValgtFejl);
        }

        // DELETE: api/Rubrik/Fejltekst/5/5
        //[Authorize(Roles="kedb-super, kedb-write")]
        [HttpDelete("Fejltekst/{rubrikId}/{fejltekstId}")]
        public async Task<IActionResult> DeleteRubrikValgtFejl(int rubrikId, int fejltekstId)
        {
            var rubrik = await _RubrikRepository.GetRubrikValgtFejlById(rubrikId, fejltekstId);
            if (rubrik == null)
            {
                return NotFound();
            }

            await _RubrikRepository.RemoveRubrikValgtFejl(rubrikId, fejltekstId);

            return NoContent();
        }
    }
}