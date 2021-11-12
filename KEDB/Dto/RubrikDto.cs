using System.Collections.Generic;

namespace KEDB.Dto
{
    public class RubrikDto
    {
        public int RubrikId { get; set; }
        public int RubrikTypeId { get; set; }
        public double RubrikTypeNummer { get; set; }
        public string Type { get; set; }
        public string OriginalVaerdi { get; set; }
        public string KorrigeretVaerdi { get; set; }
        public List<FejltekstDto> MuligeFejl { get; set; }
        public List<FejltekstDto> ValgteFejl { get; set; }
    }
}
