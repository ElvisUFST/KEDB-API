using System.ComponentModel.DataAnnotations;

namespace KEDB.Model
{
    public class RubrikMuligFejl
    {
        [Required]
        public int RubrikTypeId { get; set; }
        public virtual RubrikType RubrikType { get; set; }

        [Required]
        public int ProfilId { get; set; }
        public virtual Profil Profil { get; set; }

        [Required]
        public int FejltekstId { get; set; }
        public virtual Fejltekst Fejltekst { get; set; }
    }
}

