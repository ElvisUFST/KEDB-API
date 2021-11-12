namespace KEDB.Dto.Statistik
{
    public class AntalKontrollerOgFejl
    {
        public double ProcentMedFejl { get; set; } //til cirkeldiagram

        public double procentUdenFejl              //til cirkeldiagram
        {
            get
            {
                return 100 - ProcentMedFejl;
            }
        }

        public int TotalAfrapporteretMedFejl { get; set; }
        public int TotalAfrapporteretUdenFejl { get; set; }


        public int TotalUdtagetTilKontrol { get; set; }
        public int TotalAfrapporteret { get; set; }
    }
}
