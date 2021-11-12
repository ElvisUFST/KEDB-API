using System;

namespace KEDB.Dto
{
    public class AfrapporteredeKontrolrapporterDto
    {
        public int Id { get; set; }
        public string Referencenummer { get; set; }
        public string WorkzoneJournalnummer { get; set; }
        public string Profilnummer { get; set; }
        public DateTime? AfrapporteretDato { get; set; }
        public string VaremodtagerNavn { get; set; }
        public int VaremodtagerCVR { get; set; }
        public ADUserDto Sagsbehandler { get; set; }
    }
}
