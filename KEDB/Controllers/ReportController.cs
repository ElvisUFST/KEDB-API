using KEDB.Data.Interface;
using KEDB.Model;
using KEDB.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KEDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IKontrolrapportRepository _kontrolrapportRepository;

        public ReportController(IReportService reportService, IKontrolrapportRepository kontrolrapportRepository)
        {
            _reportService = reportService;
            _kontrolrapportRepository = kontrolrapportRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Kontrolrapport kontrolrapport = await _kontrolrapportRepository.GetById(id);

            if (kontrolrapport == null)
            {
                return NotFound();
            }
            string filename = kontrolrapport.Referencenummer + kontrolrapport.Varepostnummer;

            var pdfFile = _reportService.GeneratePdfReport(kontrolrapport, this.Url);
            return File(pdfFile, "application/octet-stream", filename + ".pdf");
        }

        [HttpGet("logo", Name = "Logo")]
        public IActionResult Get()
        {
            var image = System.IO.File.OpenRead("Static/Utils/told-logo.png");
            return File(image, "image/png");
        }
    }
}