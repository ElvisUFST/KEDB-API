namespace KEDB.Dto
{
    public class RubrikMuligeFejlDto
    {
        public int ProfilId { get; set; }
        public string ProfilNummer { get; set; }
        public int RubrikTypeId { get; set; }
        public string RubrikTypeNavn { get; set; }
        public int FejltekstId { get; set; }
        public string Fejltekst { get; set; }
    }
}
