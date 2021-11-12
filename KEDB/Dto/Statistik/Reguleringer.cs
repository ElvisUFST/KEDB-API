namespace KEDB.Dto.Statistik
{
    public class Reguleringer
    {
        // [JsonProperty("AntalSagerSomHarMedfortReguleringITold")]
        public int ReguleringCount { get; set; }

        public decimal ReguleringTilbagebetalt { get; set; }
        public decimal ReguleringOpkreavet { get; set; }
        public decimal ReguleringTotal
        {
            get
            {
                return ReguleringTilbagebetalt + ReguleringOpkreavet;
            }
        }

        public decimal[] tilbagebetalt { get; set; }
        public decimal[] opkravet { get; set; }
    }
}
