using System.ComponentModel.DataAnnotations;


namespace KEDB.Model
{
    public class RubrikValgtFejl
    {
        [Required]
        public int RubrikId { get; set; }
        public virtual Rubrik Rubrik { get; set; }

        [Required]
        public int FejltekstId { get; set; }
        public virtual Fejltekst Fejltekst { get; set; }
    }
}