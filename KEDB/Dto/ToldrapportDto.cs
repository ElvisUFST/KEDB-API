using System;

namespace KEDB.Dto
{
    public class ToldrapportDto
    {
        public DateTime? DagsDato { get; set; }
        public string AndreDeltagere { get; set; }
        public string TaltMed { get; set; }
        public bool GodkendtOrdning { get; set; }
        public string GodkendtOrdningTekst { get; set; }
        public string Sagsbeskrivelse { get; set; }
        public bool Toldkontrol { get; set; }

        public int ToldrapportTransportmiddelId { get; set; }
        public string ToldrapportTransportmiddel { get; set; }

        public int ToldrapportOpdagendeAktoerId { get; set; }
        public string ToldrapportOpdagendeAktoer { get; set; }

        public int ToldrapportFejlKategoriId { get; set; }
        public string ToldrapportFejlKategori { get; set; }

        public int ToldrapportKommunikationId { get; set; }
        public string ToldrapportKommunikation { get; set; }

        public int ToldrapportOvertraedelsesAktoerId { get; set; }
        public string ToldrapportOvertraedelsesAktoer { get; set; }
    }
}
