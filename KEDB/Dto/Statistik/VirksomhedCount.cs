namespace KEDB.Dto.Statistik
{
    public class VirksomhedCount
    {
        public int Antal { get; set; }
        public long CVR { get; set; }
        public string Navn { get; set; }
        public int AntalMedFejl { get; set; }
        public int AntalUdenFejl { get; set; }

        public double AndelAfSamlet { get; set; }

        public double Fejlprocent
        {
            get
            {
                if (AntalMedFejl == 0)
                    return 0;
                return (((double)AntalMedFejl / (double)Antal) * 100);
            }
        }

        public decimal ReguleringOpkreavet { get; set; }
        public decimal ReguleringTilbagebetalt { get; set; }
    }
}
