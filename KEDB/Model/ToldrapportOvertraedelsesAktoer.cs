using System.ComponentModel.DataAnnotations;

namespace KEDB.Model
{
    public class ToldrapportOvertraedelsesAktoer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "\"Tekst\" is required")]
        public string Tekst { get; set; }
    }
}
