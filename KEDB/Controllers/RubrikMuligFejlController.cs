using KEDB.Data.Interface;
using KEDB.Dto;
using KEDB.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KEDB.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RubrikMuligFejlController : ControllerBase
    {
        private readonly IRubrikMuligFejlRepository _rubrikMuligFejlRepository;

        public RubrikMuligFejlController(IRubrikMuligFejlRepository rubrikMuligFejlRepository)
        {
            _rubrikMuligFejlRepository = rubrikMuligFejlRepository;
        }

        // GET: api/RubrikMuligFejl
        [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RubrikMuligFejl>>> GetAllRubrikMuligeFejl()
        {
            var getAllRubrikMuligeFejl = await _rubrikMuligFejlRepository.GetAll();

            var getAllRunbrikMuligFejelDtoList = new List<RubrikMuligeFejlDto>();

            foreach (var rubrikMuligFejl in getAllRubrikMuligeFejl)
            {
                getAllRunbrikMuligFejelDtoList.Add(new RubrikMuligeFejlDto
                {
                    ProfilId = rubrikMuligFejl.ProfilId,
                    ProfilNummer = rubrikMuligFejl.Profil.ProfilNummer,
                    RubrikTypeId = rubrikMuligFejl.RubrikTypeId,
                    RubrikTypeNavn = rubrikMuligFejl.RubrikType.Navn,
                    FejltekstId = rubrikMuligFejl.FejltekstId,
                    Fejltekst = rubrikMuligFejl.Fejltekst.Tekst
                });
            }
            return Ok(getAllRunbrikMuligFejelDtoList);
        }

        // GET: api/RubrikMuligFejl/5/5/
        [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet("{rubrikTypeId}/{profilId}/{fejltekstId}")]
        public async Task<ActionResult<RubrikMuligFejl>> GetRubrikMuligFejl(int rubrikTypeId, int profilId, int fejltekstId)
        {
            var rubrikMuligFejl = await _rubrikMuligFejlRepository.GetById(rubrikTypeId, profilId, fejltekstId);

            if (rubrikMuligFejl == null)
            {
                return NotFound();
            }

            RubrikMuligeFejlDto rubrikMuligFejlDto = new RubrikMuligeFejlDto
            {
                ProfilId = rubrikMuligFejl.ProfilId,
                ProfilNummer = rubrikMuligFejl.Profil.ProfilNummer,
                RubrikTypeId = rubrikMuligFejl.RubrikTypeId,
                RubrikTypeNavn = rubrikMuligFejl.RubrikType.Navn,
                FejltekstId = rubrikMuligFejl.FejltekstId,
                Fejltekst = rubrikMuligFejl.Fejltekst.Tekst
            };

            return Ok(rubrikMuligFejlDto);
        }

        // PUT: api/RubrikMuligFejl/5/5/5
        [Authorize(Roles = "kedb-super, kedb-write")]
        [HttpPut("{rubrikTypeId}/{profilId}/{fejltekstId}")]
        public async Task<IActionResult> UpdateRubrikMuligFejl(int rubrikTypeId, int profilId, int fejltekstId, RubrikMuligFejl rubrikMuligFejl)
        {
            if (rubrikTypeId != rubrikMuligFejl.RubrikTypeId && profilId != rubrikMuligFejl.ProfilId && fejltekstId != rubrikMuligFejl.FejltekstId)
            {
                return BadRequest();
            }

            await _rubrikMuligFejlRepository.Update(rubrikMuligFejl);

            return NoContent();
        }

        // POST: api/RubrikMuligFejl
        [Authorize(Roles = "kedb-super")]
        [HttpPost]
        public async Task<ActionResult<RubrikMuligFejl>> CreateRubrikMuligFejl(RubrikMuligFejl rubrikMuligFejl)
        {
            await _rubrikMuligFejlRepository.Add(rubrikMuligFejl);

            return CreatedAtAction("GetRubrikMuligFejl", new { id = rubrikMuligFejl.RubrikTypeId, rubrikMuligFejl.ProfilId, rubrikMuligFejl.FejltekstId }, rubrikMuligFejl);
        }

        // DELETE: api/RubrikMuligFejl/5/5/5
        [Authorize(Roles = "kedb-super")]
        [HttpDelete("{rubrikTypeId}/{profilId}/{fejltekstId}")]
        public async Task<IActionResult> DeleteRubrikMuligFejl(int rubrikTypeId, int profilId, int fejltekstId)
        {
            var rubrikMuligFejl = await _rubrikMuligFejlRepository.GetById(rubrikTypeId, profilId, fejltekstId);
            if (rubrikMuligFejl == null)
            {
                return NotFound();
            }

            await _rubrikMuligFejlRepository.Remove(rubrikTypeId, profilId, fejltekstId);

            return NoContent();
        }
    }
}
