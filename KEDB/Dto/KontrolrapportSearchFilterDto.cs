using System;

namespace KEDB.Dto
{
    public class KontrolrapportSearchFilterDto
    {
        public string Referencenummer { get; set; }
        public int? VaremodtagerCVR { get; set; }
        public int? KlarererCVR { get; set; }
        public int? Toldsted { get; set; }
        public DateTime? AntagetTilDato { get; set; }
        public DateTime? AntagetFraDato { get; set; }
        public string Profilnummer { get; set; }
        public long? Branchekode { get; set; }
        public string WorkzoneJournalnummer { get; set; }
        public string Oprindelsesland { get; set; }
        public DateTime? AfrapporteretTilDato { get; set; }
        public DateTime? AfrapporteretFraDato { get; set; }
        public string Procedurekode { get; set; }
        public string Sagsbehandler { get; set; }
    }
}
