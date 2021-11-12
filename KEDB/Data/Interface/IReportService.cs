using KEDB.Model;
using Microsoft.AspNetCore.Mvc;

namespace KEDB.Services
{
    public interface IReportService
    {
        public byte[] GeneratePdfReport(Kontrolrapport kontrolrapport, IUrlHelper url);
    }
}