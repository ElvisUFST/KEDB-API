using KEDB.Model;
using System;
using System.Collections.Generic;

namespace KEDB.Dto
{
    public class KontrolrapportDto
    {
        public int KontrolrapportId { get; set; }
        public string Referencenummer { get; set; }
        public int Varepostnummer { get; set; }
        public int Toldsted { get; set; }
        public String Profilnummer { get; set; }
        public string WorkzoneJournalnummer { get; set; }
        public long Branchekode { get; set; }
        public decimal ToldmaessigAendringOpkraevning { get; set; }
        public decimal ToldmaessigAendringTilbagebetaling { get; set; }
        public bool OversendtTilAnalyse { get; set; }
        public ADUserDto Sagsbehandler { get; set; }
        public bool OversendtTilToldrapport { get; set; }
        public string VaremodtagerNavn { get; set; }
        public int VaremodtagerCVR { get; set; }
        public int KlarererCVR { get; set; }
        public DateTime AntagetDato { get; set; }
        public DateTime? AfrapporteretDato { get; set; }
        public DateTime? RedigeretDato { get; set; }
        public AndreUregelmaessigheder AndreUregelmaessigheder { get; set; }
        public ToldrapportDto Toldrapport { get; set; }
        public List<RubrikDto> Rubrikker { get; set; }
    }
}
