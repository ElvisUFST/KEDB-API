using AutoMapper;
using KEDB.Audit;
using KEDB.Data.Interface;
using KEDB.Dto;
using KEDB.Model;
using KEDB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace KEDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KontrolrapportController : ControllerBase
    {
        private readonly IKontrolrapportRepository _kontrolrapportRepository;
        private readonly IMapper _mapper;
        private readonly IAuditLog _auditLog;
        private IReportService _reportService;

        public KontrolrapportController(IKontrolrapportRepository kontrolrapportRepository, IMapper mapper, IAuditLog auditLog, IReportService reportService)
        {
            _kontrolrapportRepository = kontrolrapportRepository;
            _mapper = mapper;
            _auditLog = auditLog;
            _reportService = reportService;
        }

        /*****
        *
        * Der bliver ikke brug for at hente alle kontrolrapporter ud paa en gang.
        * Som minimum skal der en graense paa hvor mange der bliver hentet ud ( .Take(x) )
        * Muligvis pagination
        *
        *******/

        // GET: api/Kontrolrapport
        // [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet("filter/{search}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kontrolrapport>>> GetKontrolrapporter([FromQuery] KontrolrapportSearchFilterDto search)
        {
            var kontrolrapporter = await _kontrolrapportRepository.GetAll(search);
            var kontrolrapporterDtoList = new List<KontrolrapporterDto>();

            foreach (var kontrolrapport in kontrolrapporter)
            {
                if (string.IsNullOrEmpty(kontrolrapport.WorkzoneJournalnummer))
                {
                    kontrolrapporterDtoList.Add(new KontrolrapporterDto
                    {
                        Id = kontrolrapport.Id,
                        Referencenummer = kontrolrapport.Referencenummer,
                        Varepostnummer = Int32.Parse(kontrolrapport.Varepostnummer),
                        Profilnummer = kontrolrapport.Profilnummer,
                        AntagetDato = kontrolrapport.AntagetDato,
                        VaremodtagerNavn = kontrolrapport.VaremodtagerNavn,
                        VaremodtagerCVR = kontrolrapport.VaremodtagerCVR,
                        Toldsted = kontrolrapport.Toldsted,
                    });
                }
            }

            return Ok(kontrolrapporterDtoList);
        }

        // GET: api/Kontrolrapport
        // [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet("Afrapporterede/filter/{search}")]
        [HttpGet("Afrapporterede")]
        public async Task<ActionResult<IEnumerable<Kontrolrapport>>> GetAfrapporteredeKontrolrapporter([FromQuery] KontrolrapportSearchFilterDto search)
        {
            var afrapporteredeKontrolrapporter = await _kontrolrapportRepository.GetAll(search);

            var afrapporteredeKontrolrapporterDtoList = new List<AfrapporteredeKontrolrapporterDto>();

            foreach (var afrapporteredeKontrolrapport in afrapporteredeKontrolrapporter)
            {
                if (!string.IsNullOrEmpty(afrapporteredeKontrolrapport.WorkzoneJournalnummer))
                {
                    afrapporteredeKontrolrapporterDtoList.Add(new AfrapporteredeKontrolrapporterDto
                    {
                        Id = afrapporteredeKontrolrapport.Id,
                        Referencenummer = afrapporteredeKontrolrapport.Referencenummer,
                        WorkzoneJournalnummer = afrapporteredeKontrolrapport.WorkzoneJournalnummer,
                        Profilnummer = afrapporteredeKontrolrapport.Profilnummer,
                        AfrapporteretDato = afrapporteredeKontrolrapport.AfrapporteretDato,
                        VaremodtagerNavn = afrapporteredeKontrolrapport.VaremodtagerNavn,
                        VaremodtagerCVR = afrapporteredeKontrolrapport.VaremodtagerCVR,
                        Sagsbehandler = ADUserDto.Parse(afrapporteredeKontrolrapport.Sagsbehandler),
                    });
                }
            }

            return Ok(afrapporteredeKontrolrapporterDtoList);
        }

        // GET: api/Kontrolrapport/5
        // [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet("{id}")]
        public async Task<ActionResult<KontrolrapportDto>> GetKontrolrapport(int id)
        {
            var kontrolrapport = await _kontrolrapportRepository.GetById(id);

            if (kontrolrapport == null)
            {
                return NotFound();
            }

            var kontrolrapportDto = _mapper.Map<KontrolrapportDto>(kontrolrapport);

            return Ok(kontrolrapportDto);
        }

        // GET: api/Kontrolrapport/Toldsteder
        // [Authorize(Roles = "kedb-super, kedb-read, kedb-write")]
        [HttpGet("Toldsteder")]
        public async Task<ActionResult<IEnumerable<int>>> GetToldsteder()
        {
            var toldsteder = await _kontrolrapportRepository.GetToldsteder();

            if (toldsteder == null)
            {
                return NotFound();
            }

            toldsteder.Sort();

            return toldsteder;
        }

        // Put: api/Kontrolrapport/5
        // [Authorize(Roles = "kedb-super, kedb-write")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKontrolrapport(int id, KontrolrapportDto kontrolrapportDto)
        {
            var kontrolrapport = await _kontrolrapportRepository.GetById(id);

            if (id != kontrolrapportDto.KontrolrapportId)
            {
                return BadRequest();
            }

            bool alreadySentForAnalysis = kontrolrapport.OversendtTilAnalyse;

            _mapper.Map<KontrolrapportDto, Kontrolrapport>(kontrolrapportDto, kontrolrapport);

            await _kontrolrapportRepository.Update(kontrolrapport);

            if (!alreadySentForAnalysis && kontrolrapportDto.OversendtTilAnalyse)
            {
                await SendMailToAnalysis(kontrolrapport);
            }

            await _auditLog.Log(new UserAction(
                User.Identity.Name,
                UserActionType.Update,
                EntityType.Kontrolrapport,
                id.ToString(),
                kontrolrapportDto));

            return Ok();
        }

        private async Task SendMailToAnalysis(Kontrolrapport kontrolrapport)
        {
            var UserName = User.Claims.FirstOrDefault(a => a.Type == "name")?.Value;
            string to = "Elvis.Saronjic@ufst.dk";
            string from = "stido@toldst.dk";
            MailMessage message = new MailMessage(from, to);
            message.Subject = "<b> STIDO kontrol vidergivet til analyse <b>";
            message.Body = @"STIDO kontrol vidergivet til analyse  <br />
                            STIDO kontrol på importangivelsen med referencenummer: " + kontrolrapport.Referencenummer + ": varepost: " + kontrolrapport.Varepostnummer + " er blevet vidergivet til analyse.  <br />" +
                            "Sagens journalnummer: " + kontrolrapport.WorkzoneJournalnummer + "<br />" +
                            "Sagen er overdraget af: " + UserName + " - " + User.Identity.Name;

            var pdfFile = _reportService.GeneratePdfReport(kontrolrapport, Url);
            var pdfStream = new MemoryStream(pdfFile);
            string filename = kontrolrapport.Referencenummer + kontrolrapport.Varepostnummer;
            Attachment data = new Attachment(pdfStream, filename, "application/pdf");
            message.Attachments.Add(data);

            SmtpClient client = new SmtpClient("smtp-relay.skat.dk", 25);
            try
            {
                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in SendMailToAnalyse(): {0}",
                    ex.ToString());
            }
        }
    }
}