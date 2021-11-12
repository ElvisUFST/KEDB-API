using System;

namespace KEDB.Dto
{
    public class KontrolrapporterDto
    {
        public int Id { get; set; }
        public string Referencenummer { get; set; }
        public int Varepostnummer { get; set; }
        public string Profilnummer { get; set; }
        public DateTime AntagetDato { get; set; }
        public string VaremodtagerNavn { get; set; }
        public int VaremodtagerCVR { get; set; }
        public int Toldsted { get; set; }
    }
}
