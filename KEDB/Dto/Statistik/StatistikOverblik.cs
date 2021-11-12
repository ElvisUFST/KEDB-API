namespace KEDB.Dto.Statistik
{
    public class StatistikOverblikViewModel
    {
        public AntalKontrollerOgFejl AntalKontrollerOgFejl { get; set; }
        public AfledteEffekter AfledteEffekter { get; set; }
        public Reguleringer Reguleringer { get; set; }
        public Top10 Top10VaremodtagereMedFejl { get; set; }
        public Top10 Top10VaremodtagereUdtaget { get; set; }
        public Top10 Top10KlarererUdtaget { get; set; }
        public Top10 Top10KlarererMedFejl { get; set; }
    }
}
