using System.ComponentModel.DataAnnotations;

namespace KEDB.Model
{
    public class ToldrapportOpdagendeAktoer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "\"Tekst\" is required")]
        public string Tekst { get; set; }
    }
}
